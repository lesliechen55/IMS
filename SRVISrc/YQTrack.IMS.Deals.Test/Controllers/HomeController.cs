using System.Web.Mvc;

namespace YQTrack.IMS.Deals.Test.Controllers
{
    public class HomeController : Controller
    {
        [PermissionCode("Home_" + nameof(Index))]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}