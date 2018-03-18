using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
using AzR.Web.Root.Controllers;

namespace AzR.Web.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private IRoleService _role;
        public RoleController(IRoleService role)
        {
            _role = role;
        }

        // GET: Admin/Role
        public async Task<ActionResult> Index()
        {
            var models = await _role.GetAllAsync();
            return PartialView(models);
        }

        public async Task<ActionResult> Details(int id)
        {
            var model = await _role.GetAsync(id);
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new RoleViewModel
            {
                IsActive = true
            };
            return PartialView("Save", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model, int page = 1)
        {
            var exist = await _role.IsExistAsync(model.Name);

            if (exist) ModelState.AddModelError("Name", @"Role Name already exist");
            if (!ModelState.IsValid)
            {
                return PartialView("Save", model);
            }

            var result = await _role.CreateAsync(model);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Role", new { Area = "Admin", page }),
                        message = result == null ? "Record creation Failed!!!" : "Record created successfully!!!",
                        position = "mainContent"
                    });
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _role.GetAsync(id);
            return PartialView("Save", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel model, int page = 1)
        {
            var exist = await _role.IsExistAsync(model.Name, model.Id);

            if (exist) ModelState.AddModelError("DisplayName", @"Role Name already exist");

            if (!ModelState.IsValid)
            {
                return PartialView("Save", model);
            }

            var result = await _role.UpdateAsync(model);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Role", new { Area = "Admin", page }),
                        message = result == null ? "Record update Failed!!!" : "Record update successfully!!!",
                        position = "mainContent"
                    });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _role.DeActiveAsync(id);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Role", new { Area = "Admin" }),
                        message = result == 0 ? "Record deleted Failed!!!" : "Record deleted successfully!!!",
                        position = "mainContent"
                    });
        }

    }
}