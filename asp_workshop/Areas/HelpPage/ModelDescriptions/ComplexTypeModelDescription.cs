using global::System.Collections.ObjectModel;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class ComplexTypeModelDescription : ModelDescription
    {
        private Collection<ParameterDescription> _properties;

        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties
        {
            get
            {
                return _properties;
            }

            private set
            {
                _properties = value;
            }
        }
    }
}