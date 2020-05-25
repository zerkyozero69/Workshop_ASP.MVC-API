using global::System;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class ParameterAnnotation
    {
        private Attribute _annotationAttribute;
        private string _documentation;

        public Attribute AnnotationAttribute
        {
            get
            {
                return _annotationAttribute;
            }

            set
            {
                _annotationAttribute = value;
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
    }
}