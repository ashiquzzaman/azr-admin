using AzR.Web.Providers;
using AzR.WebFw.Controllers;
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

        public string Test(string id)
        {
            User.UpdateClaim("Expired", id);
            return "Test";
        }
    }
}
