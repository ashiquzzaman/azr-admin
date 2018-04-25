using AzR.Web.Root.Controllers;
using System.Web.Mvc;

namespace AzR.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

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
