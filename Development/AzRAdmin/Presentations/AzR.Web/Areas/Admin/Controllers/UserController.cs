using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Web.Controllers;
using PagedList;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AzR.WebFw.Controllers;

namespace AzR.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private IUserService _user;
        private IRoleService _role;
        private IBranchService _branch;
        public UserController(IUserService user, IRoleService role, IBranchService branch)
        {
            _user = user;
            _role = role;
            _branch = branch;
        }

        // GET: Institute/User
        public ActionResult Index(int? page, int pageSize = 30)
        {
            page = page == 0 ? 1 : page;
            var model = _user.GetAll().OrderBy(u => u.Id).ToPagedList(page ?? 1, pageSize);
            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new UserViewModel
            {
                OrgId = CmsUser.ActiveBranchId,
                Active = true,
                OrgList = await _branch.LoadBranchsAsync(),
                RoleList = await _role.LoadRoleByNameAsync(),
            };

            return PartialView("Save", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model, int page = 1)
        {
            if (model.OrgId == 0 || model.OrgId == null)
            {
                model.OrgId = 1;
            }
            if (!ModelState.IsValid)
            {
                model.OrgList = await _branch.LoadBranchsAsync();
                model.RoleList = await _role.LoadRoleByNameAsync();
                return PartialView("Save", model);
            }

            var result = _user.Create(model);
            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "User", new { Area = "Admin", page }),
                        message = result != null ? "Record created successfully!!!" : "Record creation Failed!!!",
                        position = "mainContent"
                    });
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = _user.GetUserById(id);
            model.OrgList = await _branch.LoadBranchsAsync();
            model.RoleList = await _role.LoadRoleByNameAsync();
            return PartialView("Save", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model, int page = 1)
        {
            if (model.OrgId == 0 || model.OrgId == null)
            {
                model.OrgId = 1;
            }

            if (!ModelState.IsValid)
            {

                model.OrgList = await _branch.LoadBranchsAsync();
                model.RoleList = await _role.LoadRoleByNameAsync();
                return PartialView("Save", model);
            }

            var result = _user.Update(model);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "User", new { Area = "Admin", page }),
                        message = result != null ? "Record updated successfully!!!" : "Record update Failed!!!",
                        position = "mainContent"
                    });
        }


        public ActionResult Delete(int id)
        {
            _user.DeActive(id);
            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "User", new { Area = "Admin" }),
                        message = "Record DeActivated successfully!!!",
                        position = "mainContent"
                    });
        }

        public ActionResult Details(int id)
        {
            var model = _user.GetUserById(id);
            return PartialView(model);
        }
    }
}