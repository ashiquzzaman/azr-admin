using System.Web.Mvc;
using AzR.Web.Root.Controllers;

namespace AzR.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController()
        {

        }
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Board()
        {
            return PartialView();
        }


    }
}
