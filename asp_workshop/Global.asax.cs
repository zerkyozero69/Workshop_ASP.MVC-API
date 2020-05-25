using System.Web;
using global::System.Web.Http;
using System.Web.Mvc;
using global::System.Web.Optimization;
using System.Web.Routing;

namespace asp_workshop
{
    public class WebApiApplication : HttpApplication
    {
        public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}