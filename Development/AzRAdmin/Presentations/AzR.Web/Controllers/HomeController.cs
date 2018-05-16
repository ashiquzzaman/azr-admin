using System.Web.Mvc;
using AzR.WebFw.Controllers;

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
