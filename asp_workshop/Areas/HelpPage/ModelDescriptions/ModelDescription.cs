using global::System;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Describes a type model.
    /// </summary>
    public abstract class ModelDescription
    {
        private string _name;
        private string _documentation;
        private Type _modelType;

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

        public Type ModelType
        {
            get
            {
                return _modelType;
            }

            set
            {
                _modelType = value;
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
    }
}