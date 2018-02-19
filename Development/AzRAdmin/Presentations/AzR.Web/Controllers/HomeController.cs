using AzR.Core.Services.Interface;
using System.Web.Mvc;

namespace AzR.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IBaseManager general) : base(general)
        {

        }



        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

    }
}
