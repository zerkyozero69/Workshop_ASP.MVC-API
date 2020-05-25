using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using global::System.Net.Http.Headers;
using global::System.Web.Http.Description;
using global::asp_workshop.Areas.HelpPage.ModelDescriptions;

namespace asp_workshop.Areas.HelpPage.Models
{
    /// <summary>
    /// The model that represents an API displayed on the help page.
    /// </summary>
    public class HelpPageApiModel
    {
        private IDictionary<MediaTypeHeaderValue, object> _sampleRequests;
        private IDictionary<MediaTypeHeaderValue, object> _sampleResponses;
        private Collection<string> _errorMessages;
        private ApiDescription _apiDescription;
        private Collection<ParameterDescription> _uriParameters;
        private ModelDescription _requestModelDescription;
        private ModelDescription _resourceDescription;
        private string _requestDocumentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPageApiModel"/> class.
        /// </summary>
        public HelpPageApiModel()
        {
            UriParameters = new Collection<ParameterDescription>();
            SampleRequests = new Dictionary<MediaTypeHeaderValue, object>();
            SampleResponses = new Dictionary<MediaTypeHeaderValue, object>();
            ErrorMessages = new Collection<string>();
        }

        /// <summary>
        /// Gets or sets the <see cref="ApiDescription"/> that describes the API.
        /// </summary>
        public ApiDescription ApiDescription
        {
            get
            {
                return _apiDescription;
            }

            set
            {
                _apiDescription = value;
            }
        }


        /// <summary>
        /// Gets or sets the <see cref="ParameterDescription"/> collection that describes the URI parameters for the API.
        /// </summary>
        public Collection<ParameterDescription> UriParameters
        {
            get
            {
                return _uriParameters;
            }

            private set
            {
                _uriParameters = value;
            }
        }

        /// <summary>
        /// Gets or sets the documentation for the request.
        /// </summary>
        public string RequestDocumentation
        {
            get
            {
                return _requestDocumentation;
            }

            set
            {
                _requestDocumentation = value;
            }
        }

        /// <summary>
        /// Gets or sets the model description of the request body.
        /// </summary>
        public ModelDescription RequestModelDescription
        {
            get
            {
                return _requestModelDescription;
            }

            set
            {
                _requestModelDescription = value;
            }
        }

        /// <summary>
        /// Gets the request body parameter descriptions.
        /// </summary>
        public IList<ParameterDescription> RequestBodyParameters
        {
            get
            {
                return GetParameterDescriptions(RequestModelDescription);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ModelDescription"/> that describes the resource.
        /// </summary>
        public ModelDescription ResourceDescription
        {
            get
            {
                return _resourceDescription;
            }

            set
            {
                _resourceDescription = value;
            }
        }

        /// <summary>
        /// Gets the resource property descriptions.
        /// </summary>
        public IList<ParameterDescription> ResourceProperties
        {
            get
            {
                return GetParameterDescriptions(ResourceDescription);
            }
        }

        /// <summary>
        /// Gets the sample requests associated with the API.
        /// </summary>
        public IDictionary<MediaTypeHeaderValue, object> SampleRequests
        {
            get
            {
                return _sampleRequests;
            }

            private set
            {
                _sampleRequests = value;
            }
        }

        /// <summary>
        /// Gets the sample responses associated with the API.
        /// </summary>
        public IDictionary<MediaTypeHeaderValue, object> SampleResponses
        {
            get
            {
                return _sampleResponses;
            }

            private set
            {
                _sampleResponses = value;
            }
        }

        /// <summary>
        /// Gets the error messages associated with this model.
        /// </summary>
        public Collection<string> ErrorMessages
        {
            get
            {
                return _errorMessages;
            }

            private set
            {
                _errorMessages = value;
            }
        }

        private static IList<ParameterDescription> GetParameterDescriptions(ModelDescription modelDescription)
        {
            ComplexTypeModelDescription complexTypeModelDescription = modelDescription as ComplexTypeModelDescription;
            if (complexTypeModelDescription is object)
            {
                return complexTypeModelDescription.Properties;
            }

            CollectionModelDescription collectionModelDescription = modelDescription as CollectionModelDescription;
            if (collectionModelDescription is object)
            {
                complexTypeModelDescription = collectionModelDescription.ElementDescription as ComplexTypeModelDescription;
                if (complexTypeModelDescription is object)
                {
                    return complexTypeModelDescription.Properties;
                }
            }

            return null;
        }
    }
}