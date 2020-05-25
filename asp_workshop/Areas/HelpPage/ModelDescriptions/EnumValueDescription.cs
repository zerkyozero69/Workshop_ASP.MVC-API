
namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class EnumValueDescription
    {
        private string _documentation;
        private string _value;
        private string _name;

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

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
    }
}