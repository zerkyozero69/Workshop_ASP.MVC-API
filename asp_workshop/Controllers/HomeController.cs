using System.Web.Mvc;

namespace asp_workshop
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "Home Page";
            return View();
        }
    }
}