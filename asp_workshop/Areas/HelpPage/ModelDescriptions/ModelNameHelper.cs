using global::System;
using System.Data;
using global::System.Globalization;
using global::System.Linq;
using global::System.Reflection;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    internal sealed class ModelNameHelper
    {
        private ModelNameHelper()
        {
        }

        // Modify this to provide custom model name mapping.
        public static string GetModelName(Type type)
        {
            var modelNameAttribute = type.GetCustomAttribute<ModelNameAttribute>();
            if (modelNameAttribute is object && !string.IsNullOrEmpty(modelNameAttribute.Name))
            {
                return modelNameAttribute.Name;
            }

            string modelName = type.Name;
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: GenericOfAgurment1AndArgument2
                var genericType = type.GetGenericTypeDefinition();
                var genericArguments = type.GetGenericArguments();
                string genericTypeName = genericType.Name;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                var argumentTypeNames = genericArguments.Select(t => GetModelName(t)).ToArray();
                modelName = string.Format(CultureInfo.InvariantCulture, "{0}Of{1}", genericTypeName, string.Join("And", argumentTypeNames));
            }

            return modelName;
        }
    }
}