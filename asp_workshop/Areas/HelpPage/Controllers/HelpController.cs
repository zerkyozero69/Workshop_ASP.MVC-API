using global::System.Web.Http;
using global::System.Web.Mvc;
using global::asp_workshop.Areas.HelpPage.ModelDescriptions;

namespace asp_workshop.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";
        private HttpConfiguration httpConfiguration;

        public HelpController() : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration
        {
            get
            {
                return httpConfiguration;
            }

            private set
            {
                httpConfiguration = value;
            }
        }

        public ActionResult Index()
        {
            ViewData["DocumentationProvider"] = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!string.IsNullOrEmpty(apiId))
            {
                var apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel is object)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!string.IsNullOrEmpty(modelName))
            {
                var modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription = null;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}