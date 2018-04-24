using AzR.Core.Repositoies.Interface;
using AzR.Web.Root.Controllers;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace AzR.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IUserRoleRepository _user;
        public HomeController(IUserRoleRepository user)
        {
            _user = user;
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId<int>();
            var user = _user.Find(s => s.UserId == id);

            return View();
        }
        public ActionResult Board()
        {
            return PartialView();
        }


    }
}
