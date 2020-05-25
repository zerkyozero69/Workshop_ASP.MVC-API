using global::System.Collections.ObjectModel;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class ParameterDescription
    {
        private Collection<ParameterAnnotation> _annotations;
        private string _documentation;
        private string _name;
        private ModelDescription _typeDescription;

        public ParameterDescription()
        {
            Annotations = new Collection<ParameterAnnotation>();
        }

        public Collection<ParameterAnnotation> Annotations
        {
            get
            {
                return _annotations;
            }

            private set
            {
                _annotations = value;
            }
        }

        public string Documentation
        {
            get
            {
                return _documentation;
            }

            set
            {
                _documentation = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public ModelDescription TypeDescription
        {
            get
            {
                return _typeDescription;
            }

            set
            {
                _typeDescription = value;
            }
        }
    }
}