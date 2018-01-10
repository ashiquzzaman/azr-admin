using System.Web.Mvc;
using AzR.Core.Business;

namespace VelocityWorkFlow.Web.Controllers.Mvc
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
