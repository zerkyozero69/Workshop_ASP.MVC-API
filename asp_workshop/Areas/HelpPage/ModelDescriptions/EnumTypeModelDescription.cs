using global::System.Collections.ObjectModel;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        private Collection<EnumValueDescription> _values;

        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values
        {
            get
            {
                return _values;
            }

            private set
            {
                _values = value;
            }
        }
    }
}