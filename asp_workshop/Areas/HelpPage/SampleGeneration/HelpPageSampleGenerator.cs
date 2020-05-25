﻿using global::System;
using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using global::System.ComponentModel;
using System.Data;
using global::System.Diagnostics.CodeAnalysis;
using global::System.Globalization;
using global::System.IO;
using global::System.Linq;
using global::System.Net.Http;
using global::System.Net.Http.Formatting;
using global::System.Net.Http.Headers;
using global::System.Web.Http.Description;
using global::System.Xml.Linq;
using Microsoft.VisualBasic.CompilerServices;
using global::Newtonsoft.Json;

namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// This class will generate the samples for the help page.
    /// </summary>
    public class HelpPageSampleGenerator
    {
        private IDictionary<HelpPageSampleKey, Type> _actualHttpMessageTypes;
        private IDictionary<HelpPageSampleKey, object> _actionSamples;
        private IDictionary<Type, object> _sampleObjects;
        private IList<Func<HelpPageSampleGenerator, Type, object>> _sampleObjectFactories;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPageSampleGenerator"/> class.
        /// </summary>
        public HelpPageSampleGenerator()
        {
            ActualHttpMessageTypes = new Dictionary<HelpPageSampleKey, Type>();
            ActionSamples = new Dictionary<HelpPageSampleKey, object>();
            SampleObjects = new Dictionary<Type, object>();
            SampleObjectFactories = new List<Func<HelpPageSampleGenerator, Type, object>>();
            SampleObjectFactories.Add(DefaultSampleObjectFactory);
        }

        /// <summary>
        /// Gets CLR types that are used as the content of <see cref="HttpRequestMessage"/> or <see cref="HttpResponseMessage"/>.
        /// </summary>
        public IDictionary<HelpPageSampleKey, Type> ActualHttpMessageTypes
        {
            get
            {
                return _actualHttpMessageTypes;
            }

            internal set
            {
                _actualHttpMessageTypes = value;
            }
        }

        /// <summary>
        /// Gets the objects that are used directly as samples for certain actions.
        /// </summary>
        public IDictionary<HelpPageSampleKey, object> ActionSamples
        {
            get
            {
                return _actionSamples;
            }

            internal set
            {
                _actionSamples = value;
            }
        }

        /// <summary>
        /// Gets the objects that are serialized as samples by the supported formatters.
        /// </summary>
        public IDictionary<Type, object> SampleObjects
        {
            get
            {
                return _sampleObjects;
            }

            internal set
            {
                _sampleObjects = value;
            }
        }

        /// <summary>
        /// Gets factories for the objects that the supported formatters will serialize as samples. Processed in order,
        /// stopping when the factory successfully returns a non-<see langref="null"/> object.
        /// </summary>
        /// <remarks>
        /// Collection includes just <see cref="ObjectGenerator.GenerateObject"/> initially. Use
        /// <code>SampleObjectFactories.Insert(0, func)</code> to provide an override and
        /// <code>SampleObjectFactories.Add(func)</code> to provide a fallback.</remarks>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public IList<Func<HelpPageSampleGenerator, Type, object>> SampleObjectFactories
        {
            get
            {
                return _sampleObjectFactories;
            }

            private set
            {
                _sampleObjectFactories = value;
            }
        }

        /// <summary>
        /// Gets the request body samples for a given <see cref="ApiDescription"/>.
        /// </summary>
        /// <param name="api">The <see cref="ApiDescription"/>.</param>
        /// <returns>The samples keyed by media type.</returns>
        public IDictionary<MediaTypeHeaderValue, object> GetSampleRequests(ApiDescription api)
        {
            return GetSample(api, SampleDirection.Request);
        }

        /// <summary>
        /// Gets the response body samples for a given <see cref="ApiDescription"/>.
        /// </summary>
        /// <param name="api">The <see cref="ApiDescription"/>.</param>
        /// <returns>The samples keyed by media type.</returns>
        public IDictionary<MediaTypeHeaderValue, object> GetSampleResponses(ApiDescription api)
        {
            return GetSample(api, SampleDirection.Response);
        }

        /// <summary>
        /// Gets the request or response body samples.
        /// </summary>
        /// <param name="api">The <see cref="ApiDescription"/>.</param>
        /// <param name="sampleDirection">The value indicating whether the sample is for a request or for a response.</param>
        /// <returns>The samples keyed by media type.</returns>
        public virtual IDictionary<MediaTypeHeaderValue, object> GetSample(ApiDescription api, SampleDirection sampleDirection)
        {
            if (api is null)
            {
                throw new ArgumentNullException("api");
            }

            string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = api.ActionDescriptor.ActionName;
            var parameterNames = api.ParameterDescriptions.Select(p => p.Name);
            var formatters = new Collection<MediaTypeFormatter>();
            var type = ResolveType(api, controllerName, actionName, parameterNames, sampleDirection, out formatters);
            var samples = new Dictionary<MediaTypeHeaderValue, object>();

            // Use the samples provided directly for actions
            var actionSamples = GetAllActionSamples(controllerName, actionName, parameterNames, sampleDirection);
            foreach (var actionSample in actionSamples)
                samples.Add(actionSample.Key.MediaType, WrapSampleIfString(actionSample.Value));

            // Do the sample generation based on formatters only if an action doesn't return an HttpResponseMessage.
            // Here we cannot rely on formatters because we don't know what's in the HttpResponseMessage, it might not even use formatters.
            if (type is object && !typeof(HttpResponseMessage).IsAssignableFrom(type))
            {
                var sampleObject = GetSampleObject(type);
                foreach (var formatter in formatters)
                {
                    foreach (MediaTypeHeaderValue mediaType in formatter.SupportedMediaTypes)
                    {
                        if (!samples.ContainsKey(mediaType))
                        {
                            var sample = GetActionSample(controllerName, actionName, parameterNames, type, formatter, mediaType, sampleDirection);
                            // If no sample found, try generate sample using formatter and sample object
                            if (sample is null & sampleObject is object)
                            {
                                sample = WriteSampleObjectUsingFormatter(formatter, sampleObject, type, mediaType);
                            }

                            samples.Add(mediaType, WrapSampleIfString(sample));
                        }
                    }
                }
            }

            return samples;
        }

        /// <summary>
        /// Search for samples that are provided directly through <see cref="ActionSamples"/>.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <param name="type">The CLR type.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="sampleDirection">The value indicating whether the sample is for a request or for a response.</param>
        /// <returns>The sample that matches the parameters.</returns>
        public virtual object GetActionSample(string controllerName, string actionName, IEnumerable<string> parameterNames, Type type, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType, SampleDirection sampleDirection)
        {
            var sample = new object();

            // First, try to get the sample provided for the specified mediaType, sampleDirection, controllerName, actionName and parameterNames.
            // If not found, try to get the sample provided for the specified mediaType, sampleDirection, controllerName and actionName regardless of the parameterNames.
            // If still not found, try to get the sample provided for the specified mediaType and type.
            // Finally, try to get the sample provided for the specified mediaType.
            if (ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, sampleDirection, controllerName, actionName, parameterNames), out sample) || ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, sampleDirection, controllerName, actionName, new string[] { "*" }), out sample) || ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, type), out sample) || ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType), out sample))
            {
                return sample;
            }

            return null;
        }

        /// <summary>
        /// Gets the sample object that will be serialized by the formatters.
        /// First, it will look at the <see cref="SampleObjects"/>. If no sample object is found, it will try to create
        /// one using <see cref="DefaultSampleObjectFactory"/> (which wraps an <see cref="ObjectGenerator"/>) and other
        /// factories in <see cref="SampleObjectFactories"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The sample object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Even if all items in SampleObjectFactories throw, problem will be visible as missing sample.")]
        public virtual object GetSampleObject(Type type)
        {
            var sampleObject = new object();
            if (!SampleObjects.TryGetValue(type, out sampleObject))
            {
                // No specific object available, try our factories.
                foreach (Func<HelpPageSampleGenerator, Type, object> factory in SampleObjectFactories)
                {
                    if (factory is null)
                    {
                        continue;
                    }

                    try
                    {
                        sampleObject = factory(this, type);
                        if (sampleObject is object)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        // Ignore any problems encountered in the factory; go on to the next one (if any).
                    }
                }
            }

            return sampleObject;
        }

        /// <summary>
        /// Resolves the actual type of <see cref="System.Net.Http.ObjectContent(Of T)"/> passed to the <see cref="System.Net.Http.HttpRequestMessage"/> in an action.
        /// </summary>
        /// <param name="api">The <see cref="ApiDescription"/>.</param>
        /// <returns>The type.</returns>
        public virtual Type ResolveHttpRequestMessageType(ApiDescription api)
        {
            string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = api.ActionDescriptor.ActionName;
            var parameterNames = api.ParameterDescriptions.Select(p => p.Name);
            Collection<MediaTypeFormatter> formatters = null;
            return ResolveType(api, controllerName, actionName, parameterNames, SampleDirection.Request, out formatters);
        }

        /// <summary>
        /// Resolves the type of the action parameter or return value when <see cref="HttpRequestMessage"/> or <see cref="HttpResponseMessage"/> is used.
        /// </summary>
        /// <param name="api">The <see cref="ApiDescription"/>.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <param name="sampleDirection">The value indicating whether the sample is for a request or a response.</param>
        /// <param name="formatters">The formatters.</param>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "This is only used in advanced scenarios.")]
        public virtual Type ResolveType(ApiDescription api, string controllerName, string actionName, IEnumerable<string> parameterNames, SampleDirection sampleDirection, out Collection<MediaTypeFormatter> formatters)
        {
            if (!Enum.IsDefined(typeof(SampleDirection), sampleDirection))
            {
                throw new InvalidEnumArgumentException("sampleDirection", Conversions.ToInteger(sampleDirection), typeof(SampleDirection));
            }

            if (api is null)
            {
                throw new ArgumentNullException("api");
            }

            var type = typeof(object);
            if (ActualHttpMessageTypes.TryGetValue(new HelpPageSampleKey(sampleDirection, controllerName, actionName, parameterNames), out type) || ActualHttpMessageTypes.TryGetValue(new HelpPageSampleKey(sampleDirection, controllerName, actionName, new string[] { "*" }), out type))
            {
                // Re-compute the supported formatters based on type
                var newFormatters = new Collection<MediaTypeFormatter>();
                foreach (var formatter in api.ActionDescriptor.Configuration.Formatters)
                {
                    if (IsFormatSupported(sampleDirection, formatter, type))
                    {
                        newFormatters.Add(formatter);
                    }
                }

                formatters = newFormatters;
            }
            else
            {
                switch (sampleDirection)
                {
                    case SampleDirection.Request:
                        {
                            var requestBodyParameter = api.ParameterDescriptions.FirstOrDefault(p => p.Source == ApiParameterSource.FromBody);
                            type = requestBodyParameter is null ? null : requestBodyParameter.ParameterDescriptor.ParameterType;
                            formatters = api.SupportedRequestBodyFormatters;
                            break;
                        }

                    default:
                        {
                            // Case sampleDirection.Response
                            type = api.ResponseDescription.ResponseType ?? api.ResponseDescription.DeclaredType;
                            formatters = api.SupportedResponseFormatters;
                            break;
                        }
                }
            }

            return type;
        }

        /// <summary>
        /// Writes the sample object using formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The exception is recorded as InvalidSample.")]
        public virtual object WriteSampleObjectUsingFormatter(MediaTypeFormatter formatter, object value, Type type, MediaTypeHeaderValue mediaType)
        {
            if (formatter is null)
            {
                throw new ArgumentNullException("formatter");
            }

            if (mediaType is null)
            {
                throw new ArgumentNullException("mediaType");
            }

            object sample = string.Empty;
            MemoryStream MS = null;
            HttpContent content = null;
            try
            {
                if (formatter.CanWriteType(type))
                {
                    MS = new MemoryStream();
                    content = new ObjectContent(type, value, formatter, mediaType);
                    formatter.WriteToStreamAsync(type, value, MS, content, null).Wait();
                    MS.Position = 0;
                    var reader = new StreamReader(MS);
                    string serializedSampleString = reader.ReadToEnd();
                    if (mediaType.MediaType.ToUpperInvariant().Contains("XML"))
                    {
                        serializedSampleString = TryFormatXml(serializedSampleString);
                    }
                    else if (mediaType.MediaType.ToUpperInvariant().Contains("JSON"))
                    {
                        serializedSampleString = TryFormatJson(serializedSampleString);
                    }

                    sample = new TextSample(serializedSampleString);
                }
                else
                {
                    sample = new InvalidSample(string.Format(CultureInfo.CurrentCulture, "Failed to generate the sample for media type '{0}'. Cannot use formatter '{1}' to write type '{2}'.", mediaType, formatter.GetType().Name, type.Name));
                }
            }
            catch (Exception e)
            {
                sample = new InvalidSample(string.Format(CultureInfo.CurrentCulture, "An exception has occurred while using the formatter '{0}' to generate sample for media type '{1}'. Exception message: {2}", formatter.GetType().Name, mediaType.MediaType, UnwrapException(e).Message));
            }
            finally
            {
                if (MS is object)
                {
                    MS.Dispose();
                }

                if (content is object)
                {
                    content.Dispose();
                }
            }

            return sample;
        }

        internal static Exception UnwrapException(Exception exception)
        {
            AggregateException aggregateException = exception as AggregateException;
            if (aggregateException is object)
            {
                return aggregateException.Flatten().InnerException;
            }

            return exception;
        }

        private static object DefaultSampleObjectFactory(HelpPageSampleGenerator sampleGenerator, Type type)
        {
            // Try create a default sample object
            var objectGenerator = new ObjectGenerator();
            return objectGenerator.GenerateObject(type);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Handling the failure by returning the original string.")]
        private static string TryFormatJson(string str)
        {
            try
            {
                var parsedJson = JsonConvert.DeserializeObject(str);
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch
            {
                // can't parse JSON, return the original string
                return str;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Handling the failure by returning the original string.")]
        private static string TryFormatXml(string str)
        {
            try
            {
                var Xml = XDocument.Parse(str);
                return Xml.ToString();
            }
            catch
            {
                // can't parse XML, return the original string
                return str;
            }
        }

        private static bool IsFormatSupported(SampleDirection sampleDirection, MediaTypeFormatter formatter, Type type)
        {
            switch (sampleDirection)
            {
                case SampleDirection.Request:
                    {
                        return formatter.CanReadType(type);
                    }

                case SampleDirection.Response:
                    {
                        return formatter.CanWriteType(type);
                    }
            }

            return false;
        }

        private IEnumerable<KeyValuePair<HelpPageSampleKey, object>> GetAllActionSamples(string controllerName, string actionName, IEnumerable<string> parameterNames, SampleDirection sampleDirection)
        {
            var parameterNamesSet = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            foreach (var sample in ActionSamples)
            {
                var sampleKey = sample.Key;
                if ((string.Equals(controllerName, sampleKey.ControllerName, StringComparison.OrdinalIgnoreCase) & string.Equals(actionName, sampleKey.ActionName, StringComparison.OrdinalIgnoreCase) & (sampleKey.ParameterNames.SetEquals(new string[] { "*" }) | parameterNamesSet.SetEquals(sampleKey.ParameterNames)) & (int?)sampleDirection == (int?)sampleKey.SampleDirection) == true)
                {
                    yield return sample;
                }
            }
        }

        private static object WrapSampleIfString(object sample)
        {
            string stringSample = sample as string;
            if (stringSample is object)
            {
                return new TextSample(stringSample);
            }

            return sample;
        }
    }
}