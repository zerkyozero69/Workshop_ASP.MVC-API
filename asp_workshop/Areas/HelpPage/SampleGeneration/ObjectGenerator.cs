using global::System;
using global::System.Collections;
using global::System.Collections.Generic;
using global::System.Diagnostics.CodeAnalysis;
using global::System.Globalization;
using global::System.Linq;
using global::System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// This class will create an object of a given type and populate it with sample data.
    /// </summary>
    public class ObjectGenerator
    {
        internal const int DefaultCollectionSize = 2;
        private readonly SimpleTypeObjectGenerator SimpleObjectGenerator = new SimpleTypeObjectGenerator();

        /// <summary>
        /// Generates an object for a given type. The type needs to be public, have a public default constructor and settable public properties/fields. Currently it supports the following types:
        /// Simple types: <see cref="int"/>, <see cref="string"/>, <see cref="[Enum]"/>, <see cref="DateTime"/>, <see cref="Uri"/>, etc.
        /// Complex types: POCO types.
        /// Nullables: <see cref="Nullable(Of T)"/>.
        /// Arrays: arrays of simple types or complex types.
        /// Key value pairs: <see cref="KeyValuePair(Of TKey,TValue)"/>
        /// Tuples: <see cref="Tuple(Of T1)"/>, <see cref="Tuple(Of T1,T2)"/>, etc
        /// Dictionaries: <see cref="IDictionary(Of TKey,TValue)"/> or anything deriving from <see cref="IDictionary(Of TKey,TValue)"/>.
        /// Collections: <see cref="IList(Of T)"/>, <see cref="IEnumerable(Of T)"/>, <see cref="ICollection(Of T)"/>, <see cref="IList"/>, <see cref="IEnumerable"/>, <see cref="ICollection"/> or anything deriving from <see cref="ICollection(Of T)"/> or <see cref="IList"/>.
        /// Queryables: <see cref="System.Linq.IQueryable"/>, <see cref="IQueryable(Of T)"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An object of the given type.</returns>
        public object GenerateObject(Type type)
        {
            object GenerateObjectRet = default;
            GenerateObjectRet = GenerateObject(type, new Dictionary<Type, object>());
            return GenerateObjectRet;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Here we just want to return null if anything goes wrong.")]
        private object GenerateObject(Type type, Dictionary<Type, object> createdObjectReferences)
        {
            try
            {
                if (SimpleTypeObjectGenerator.CanGenerateObject(type))
                {
                    return SimpleObjectGenerator.GenerateObject(type);
                }

                if (type.IsArray)
                {
                    return GenerateArray(type, DefaultCollectionSize, createdObjectReferences);
                }

                if (type.IsGenericType)
                {
                    return GenerateGenericType(type, DefaultCollectionSize, createdObjectReferences);
                }

                if (type == typeof(IDictionary))
                {
                    return GenerateDictionary(typeof(Hashtable), DefaultCollectionSize, createdObjectReferences);
                }

                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    return GenerateDictionary(type, DefaultCollectionSize, createdObjectReferences);
                }

                if (type == typeof(IList) | type == typeof(IEnumerable) | type == typeof(ICollection))
                {
                    return GenerateCollection(typeof(ArrayList), DefaultCollectionSize, createdObjectReferences);
                }

                if (typeof(IList).IsAssignableFrom(type))
                {
                    return GenerateCollection(type, DefaultCollectionSize, createdObjectReferences);
                }

                if (type == typeof(IQueryable))
                {
                    return GenerateQueryable(type, DefaultCollectionSize, createdObjectReferences);
                }

                if (type.IsEnum)
                {
                    return GenerateEnum(type);
                }

                if (type.IsPublic | type.IsNestedPublic)
                {
                    return GenerateComplexObject(type, createdObjectReferences);
                }
            }
            catch
            {
                // Returns Nothing if anything fails
                return null;
            }

            return null;
        }

        private static object GenerateGenericType(Type type, int collectionSize, Dictionary<Type, object> createdObjectReferences)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(Nullable[]) | genericTypeDefinition == typeof(object?))
            {
                return GenerateNullable(type, createdObjectReferences);
            }

            if (genericTypeDefinition == typeof(KeyValuePair<,>))
            {
                return GenerateKeyValuePair(type, createdObjectReferences);
            }

            if (IsTuple(genericTypeDefinition))
            {
                return GenerateTuple(type, createdObjectReferences);
            }

            var genericArguments = type.GetGenericArguments();
            if (genericArguments.Length == 1)
            {
                if (genericTypeDefinition == typeof(IList<>) | genericTypeDefinition == typeof(IEnumerable<>) | genericTypeDefinition == typeof(ICollection<>))
                {
                    var collectionType = typeof(List<>).MakeGenericType(genericArguments);
                    return GenerateCollection(collectionType, collectionSize, createdObjectReferences);
                }

                if (genericTypeDefinition == typeof(IQueryable<>))
                {
                    return GenerateQueryable(type, collectionSize, createdObjectReferences);
                }

                var closedCollectionType = typeof(ICollection<>).MakeGenericType(genericArguments[0]);
                if (closedCollectionType.IsAssignableFrom(type))
                {
                    return GenerateCollection(type, collectionSize, createdObjectReferences);
                }
            }

            if (genericArguments.Length == 2)
            {
                if (genericTypeDefinition == typeof(IDictionary<,>))
                {
                    var dictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);
                    return GenerateDictionary(dictionaryType, collectionSize, createdObjectReferences);
                }

                var closedDictionaryType = typeof(IDictionary<,>).MakeGenericType(genericArguments[0], genericArguments[1]);
                if (closedDictionaryType.IsAssignableFrom(type))
                {
                    return GenerateDictionary(type, collectionSize, createdObjectReferences);
                }
            }

            if (type.IsPublic | type.IsNestedPublic)
            {
                return GenerateComplexObject(type, createdObjectReferences);
            }

            return null;
        }

        private static object GenerateTuple(Type type, Dictionary<Type, object> createdObjectReferences)
        {
            var genericArgs = type.GetGenericArguments();
            var parameterValues = new object[genericArgs.Length];
            bool failedToCreateTuple = true;
            var objectGenerator = new ObjectGenerator();
            for (int i = 0, loopTo = genericArgs.Length - 1; i <= loopTo; i++)
            {
                parameterValues[i] = objectGenerator.GenerateObject(genericArgs[i], createdObjectReferences);
                failedToCreateTuple = failedToCreateTuple & parameterValues[i] is null;
            }

            if (failedToCreateTuple)
            {
                return null;
            }

            return Activator.CreateInstance(type, parameterValues);
        }

        private static bool IsTuple(Type genericTypeDefinition)
        {
            return genericTypeDefinition == typeof(Tuple<>) | genericTypeDefinition == typeof(Tuple<,>) | genericTypeDefinition == typeof(Tuple<,,>) | genericTypeDefinition == typeof(Tuple<,,,>) | genericTypeDefinition == typeof(Tuple<,,,,>) | genericTypeDefinition == typeof(Tuple<,,,,,>) | genericTypeDefinition == typeof(Tuple<,,,,,,>) | genericTypeDefinition == typeof(Tuple<,,,,,,,>);
        }

        private static object GenerateKeyValuePair(Type keyValuePairType, Dictionary<Type, object> createdObjectReferences)
        {
            var genericArgs = keyValuePairType.GetGenericArguments();
            var typeK = genericArgs[0];
            var typeV = genericArgs[1];
            var objectGenerator = new ObjectGenerator();
            var keyObject = objectGenerator.GenerateObject(typeK, createdObjectReferences);
            var valueObject = objectGenerator.GenerateObject(typeV, createdObjectReferences);
            if (keyObject is null & valueObject is null)
            {
                // Failed to create key and values
                return null;
            }

            return Activator.CreateInstance(keyValuePairType, keyObject, valueObject);
        }

        private static object GenerateArray(Type arrayType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            var type = arrayType.GetElementType();
            var result = Array.CreateInstance(type, size);
            bool areAllElementsNothing = true;
            var objectGenerator = new ObjectGenerator();
            for (int i = 0, loopTo = size - 1; i <= loopTo; i++)
            {
                var element = objectGenerator.GenerateObject(type, createdObjectReferences);
                result.SetValue(element, i);
                areAllElementsNothing = areAllElementsNothing & element is null;
            }

            if (areAllElementsNothing)
            {
                return null;
            }

            return result;
        }

        private static object GenerateDictionary(Type dictionaryType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            var typeK = typeof(object);
            var typeV = typeof(object);
            if (dictionaryType.IsGenericType)
            {
                var genericArgs = dictionaryType.GetGenericArguments();
                typeK = genericArgs[0];
                typeV = genericArgs[1];
            }

            var result = Activator.CreateInstance(dictionaryType);
            var addMethod = dictionaryType.GetMethod("Add") ?? dictionaryType.GetMethod("TryAdd");
            var containsMethod = dictionaryType.GetMethod("Contains") ?? dictionaryType.GetMethod("ContainsKey");
            var objectGenerator = new ObjectGenerator();
            for (int i = 0, loopTo = size - 1; i <= loopTo; i++)
            {
                var newKey = objectGenerator.GenerateObject(typeK, createdObjectReferences);
                if (newKey is null)
                {
                    // Cannot generate a valid key
                    return null;
                }

                bool containsKey = (bool)containsMethod.Invoke(result, new object[] { newKey });
                if (!containsKey)
                {
                    var newValue = objectGenerator.GenerateObject(typeV, createdObjectReferences);
                    addMethod.Invoke(result, new object[] { newKey, newValue });
                }
            }

            return result;
        }

        private static object GenerateEnum(Type enumType)
        {
            var possibleValues = Enum.GetValues(enumType);
            if (possibleValues.Length > 0)
            {
                return possibleValues.GetValue(0);
            }

            return null;
        }

        private static object GenerateQueryable(Type queryableType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            bool isGeneric = queryableType.IsGenericType;
            object list = null;
            if (isGeneric)
            {
                var listType = typeof(List<>).MakeGenericType(queryableType.GetGenericArguments());
                list = GenerateCollection(listType, size, createdObjectReferences);
            }
            else
            {
                list = GenerateArray(typeof(object[]), size, createdObjectReferences);
            }

            if (list is null)
            {
                return null;
            }

            if (isGeneric)
            {
                var argumentType = typeof(IEnumerable<>).MakeGenericType(queryableType.GetGenericArguments());
                var asQueryableMethod = typeof(Queryable).GetMethod("AsQueryable", new Type[] { argumentType });
                return asQueryableMethod.Invoke(null, new object[] { list });
            }

            return ((IEnumerable)list).AsQueryable();
        }

        private static object GenerateCollection(Type collectionType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            var type = collectionType.IsGenericType ? collectionType.GetGenericArguments()[0] : typeof(object);
            var result = Activator.CreateInstance(collectionType);
            var addMethod = collectionType.GetMethod("Add");
            bool areAllElementsNothing = true;
            var objectGenerator = new ObjectGenerator();
            for (int i = 0, loopTo = size - 1; i <= loopTo; i++)
            {
                var element = objectGenerator.GenerateObject(type, createdObjectReferences);
                addMethod.Invoke(result, new object[] { element });
                areAllElementsNothing = areAllElementsNothing & element is null;
            }

            if (areAllElementsNothing)
            {
                return null;
            }

            return result;
        }

        private static object GenerateNullable(Type nullableType, Dictionary<Type, object> createdObjectReferences)
        {
            var type = nullableType.GetGenericArguments()[0];
            var objectGenerator = new ObjectGenerator();
            return objectGenerator.GenerateObject(type, createdObjectReferences);
        }

        private static object GenerateComplexObject(Type type, Dictionary<Type, object> createdObjectReferences)
        {
            object result = null;
            if (createdObjectReferences.TryGetValue(type, out result))
            {
                // The object has been created already, just return it. This will handle the circular reference case.
                return result;
            }

            if (type.IsValueType)
            {
                result = Activator.CreateInstance(type);
            }
            else
            {
                var defaultCtor = type.GetConstructor(Type.EmptyTypes);
                if (defaultCtor is null)
                {
                    // Cannot instantiate the type because it doesn't have a default constructor
                    return null;
                }

                result = defaultCtor.Invoke(new object[] { });
            }

            createdObjectReferences.Add(type, result);
            SetPublicProperties(type, result, createdObjectReferences);
            SetPublicFields(type, result, createdObjectReferences);
            return result;
        }

        private static void SetPublicProperties(Type type, object obj, Dictionary<Type, object> createdObjectReferences)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var objectGenerator = new ObjectGenerator();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.CanWrite)
                {
                    var propertyValue = objectGenerator.GenerateObject(prop.PropertyType, createdObjectReferences);
                    prop.SetValue(obj, propertyValue, null);
                }
            }
        }

        private static void SetPublicFields(Type type, object obj, Dictionary<Type, object> createdObjectReferences)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var objectGenerator = new ObjectGenerator();
            foreach (FieldInfo field in fields)
            {
                var fieldValue = objectGenerator.GenerateObject(field.FieldType, createdObjectReferences);
                field.SetValue(obj, fieldValue);
            }
        }

        private class SimpleTypeObjectGenerator
        {
            private long _index = 0;
            private static readonly Dictionary<Type, Func<long, object>> DefaultGenerators = InitializeGenerators();

            [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These are simple type factories and cannot be split up.")]
            private static Dictionary<Type, Func<long, object>> InitializeGenerators()
            {
                return new Dictionary<Type, Func<long, object>>() { { typeof(bool), (long index) => true }, { typeof(byte), (long index) => Conversions.ToByte(64) }, { typeof(char), (long index) => (char)65 }, { typeof(DateTime), (long index) => DateTime.Now }, { typeof(DateTimeOffset), (long index) => new DateTimeOffset(DateTime.Now) }, { typeof(DBNull), (long index) => DBNull.Value }, { typeof(decimal), (long index) => Conversions.ToDecimal(index) }, { typeof(double), (long index) => Conversions.ToDouble(index) + 0.1 }, { typeof(Guid), (long index) => Guid.NewGuid() }, { typeof(short), (long index) => Conversions.ToShort(index % short.MaxValue) }, { typeof(int), (long index) => Conversions.ToInteger(index % int.MaxValue) }, { typeof(long), (long index) => index }, { typeof(object), (long index) => new object() }, { typeof(sbyte), (long index) => Conversions.ToSByte(64) }, { typeof(float), (long index) => Conversions.ToSingle(index + 0.1) }, { typeof(string), (long index) => string.Format(CultureInfo.CurrentCulture, "sample string {0}", index) }, { typeof(TimeSpan), (long index) => TimeSpan.FromTicks(1234567) }, { typeof(ushort), (long index) => Conversions.ToUShort(index % ushort.MaxValue) }, { typeof(uint), (long index) => Conversions.ToUInteger(index % uint.MaxValue) }, { typeof(ulong), (long index) => Conversions.ToULong(index) }, { typeof(Uri), (long index) => new Uri(string.Format(CultureInfo.CurrentCulture, "http://webapihelppage{0}.com", index)) } };
            }

            public static bool CanGenerateObject(Type type)
            {
                return DefaultGenerators.ContainsKey(type);
            }

            public object GenerateObject(Type type)
            {
                _index += 1;
                return DefaultGenerators[type](_index);
            }
        }
    }
}