using global::System.Web.Http;
using global::System.Web.Mvc;

namespace asp_workshop.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("HelpPage_Default", "Help/{action}/{apiId}", new { Controller = "Help", action = "Index", apiId = UrlParameter.Optional });
            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}