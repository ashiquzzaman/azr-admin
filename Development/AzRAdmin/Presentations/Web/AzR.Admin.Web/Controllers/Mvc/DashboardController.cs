using AzR.Core.Business;
using System.Web.Mvc;

namespace AzR.Admin.Web.Controllers.Mvc
{
    [Authorize]
    public class DashboardController : BaseController
    {
        public DashboardController(IBaseService general) : base(general)
        {

        }



        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }


    }
}
