
namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class KeyValuePairModelDescription : ModelDescription
    {
        private ModelDescription _keyModelDescription;
        private ModelDescription _valueModelDescription;

        public ModelDescription KeyModelDescription
        {
            get
            {
                return _keyModelDescription;
            }

            set
            {
                _keyModelDescription = value;
            }
        }

        public ModelDescription ValueModelDescription
        {
            get
            {
                return _valueModelDescription;
            }

            set
            {
                _valueModelDescription = value;
            }
        }
    }
}