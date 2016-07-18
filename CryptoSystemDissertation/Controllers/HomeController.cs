using CryptoSystemDissertation.Common;
using System.Web.Mvc;

namespace CryptoSystemDissertation.Controllers
{
    public class HomeController : Controller
    {
        [ExitHttpsIfNotRequiredAttribute]
        public ActionResult Index()
        {
           return View();
        }

        [ExitHttpsIfNotRequiredAttribute]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [ExitHttpsIfNotRequiredAttribute]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}