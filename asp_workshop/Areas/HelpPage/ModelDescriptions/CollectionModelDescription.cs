
namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public class CollectionModelDescription : ModelDescription
    {
        private ModelDescription _elementDescription;

        public ModelDescription ElementDescription
        {
            get
            {
                return _elementDescription;
            }

            set
            {
                _elementDescription = value;
            }
        }
    }
}