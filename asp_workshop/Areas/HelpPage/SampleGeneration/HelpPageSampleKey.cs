using global::System;
using global::System.Collections.Generic;
using global::System.ComponentModel;
using global::System.Net.Http.Headers;
using Microsoft.VisualBasic.CompilerServices;

namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// This is used to identify the place where the sample should be applied.
    /// </summary>
    public class HelpPageSampleKey
    {
        private string _actionName;
        private string _controllerName;
        private MediaTypeHeaderValue _mediaType;
        private HashSet<string> _parameterNames;
        private Type _parameterType;
        private SampleDirection? _sampleDirection;

        /// <summary>
        /// Creates a new <see cref="HelpPageSampleKey"/> based on media type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        public HelpPageSampleKey(MediaTypeHeaderValue mediaType)
        {
            if (mediaType is null)
            {
                throw new ArgumentNullException("mediaType");
            }

            _actionName = string.Empty;
            _controllerName = string.Empty;
            _parameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _mediaType = mediaType;
        }

        /// <summary>
        /// Creates a new <see cref="HelpPageSampleKey"/> based on media type and CLR type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <param name="type">The CLR type.</param>
        public HelpPageSampleKey(MediaTypeHeaderValue mediaType, Type type) : this(mediaType)
        {
            if (type is null)
            {
                throw new ArgumentNullException("type");
            }

            _parameterType = type;
        }

        /// <summary>
        /// Creates a new <see cref="HelpPageSampleKey"/> based on <see cref="SampleDirection"/>, controller name, action name and parameter names.
        /// </summary>
        /// <param name="sampleDirection">The <see cref="SampleDirection"/>.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public HelpPageSampleKey(SampleDirection sampleDirection, string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            if (!Enum.IsDefined(typeof(SampleDirection), sampleDirection))
            {
                throw new InvalidEnumArgumentException("sampleDirection", Conversions.ToInteger(sampleDirection), typeof(SampleDirection));
            }

            if (controllerName is null)
            {
                throw new ArgumentNullException("controllerName");
            }

            if (actionName is null)
            {
                throw new ArgumentNullException("actionName");
            }

            if (parameterNames is null)
            {
                throw new ArgumentNullException("parameterNames");
            }

            _controllerName = controllerName;
            _actionName = actionName;
            _parameterNames = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            _sampleDirection = sampleDirection;
        }

        /// <summary>
        /// Creates a new <see cref="HelpPageSampleKey"/> based on media type, <see cref="SampleDirection"/>, controller name, action name and parameter names.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <param name="sampleDirection">The <see cref="SampleDirection"/>.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public HelpPageSampleKey(MediaTypeHeaderValue mediaType, SampleDirection sampleDirection, string controllerName, string actionName, IEnumerable<string> parameterNames) : this(sampleDirection, controllerName, actionName, parameterNames)
        {
            if (mediaType is null)
            {
                throw new ArgumentNullException("mediaType");
            }

            _mediaType = mediaType;
        }

        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName
        {
            get
            {
                return _controllerName;
            }
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName
        {
            get
            {
                return _actionName;
            }
        }

        /// <summary>
        /// Gets the media type.
        /// </summary>
        /// <value>
        /// The media type.
        /// </value>
        public MediaTypeHeaderValue MediaType
        {
            get
            {
                return _mediaType;
            }
        }

        /// <summary>
        /// Gets the parameter names.
        /// </summary>
        public HashSet<string> ParameterNames
        {
            get
            {
                return _parameterNames;
            }
        }

        public Type ParameterType
        {
            get
            {
                return _parameterType;
            }
        }

        /// <summary>
        /// Gets the <see cref="SampleDirection"/>.
        /// </summary>
        public SampleDirection? SampleDirection
        {
            get
            {
                return _sampleDirection;
            }
        }

        public override bool Equals(object obj)
        {
            HelpPageSampleKey otherKey = obj as HelpPageSampleKey;
            if (otherKey is null)
            {
                return false;
            }

            return string.Equals(ControllerName, otherKey.ControllerName, StringComparison.OrdinalIgnoreCase) & string.Equals(ActionName, otherKey.ActionName, StringComparison.OrdinalIgnoreCase) & (MediaType == otherKey.MediaType | (MediaType is object && MediaType.Equals(otherKey.MediaType))) & ParameterType == otherKey.ParameterType & SampleDirection.Equals(otherKey.SampleDirection) & ParameterNames.SetEquals(otherKey.ParameterNames);
        }

        public override int GetHashCode()
        {
            int hashCode = ControllerName.ToUpperInvariant().GetHashCode() ^ ActionName.ToUpperInvariant().GetHashCode();
            if (MediaType is object)
            {
                hashCode = hashCode ^ MediaType.GetHashCode();
            }

            if (SampleDirection.HasValue)
            {
                hashCode = hashCode ^ SampleDirection.GetHashCode();
            }

            if (ParameterType is object)
            {
                hashCode = hashCode ^ ParameterType.GetHashCode();
            }

            foreach (string parameterName in ParameterNames)
                hashCode = hashCode ^ parameterName.ToUpperInvariant().GetHashCode();
            return hashCode;
        }
    }
}