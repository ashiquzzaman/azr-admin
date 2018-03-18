using AzR.Core.Config;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AzR.Web.Root.Controllers;

namespace AzR.Web.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private IMenuService _menu;
        private IRoleService _role;
        public MenuController(IMenuService menu, IRoleService role)
        {
            _menu = menu;
            _role = role;
        }

        // GET: Admin/Menu
        public async Task<ActionResult> Index(int? page, int pageSize = 30)
        {
            page = page == null || page == 0 ? 1 : page;
            var model = await _menu.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync((int)page, pageSize);
            return PartialView(model);
        }
        public async Task<ActionResult> Details(int id)
        {
            var model = await _menu.GetAsync(id);
            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new MenuViewModel
            {
                ParentId = null,
                IsActive = true,
                ParentList = await _menu.LoadParentAsync(),
                RoleList = await _role.LoadParentAsync()
            };
            return PartialView("Save", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuViewModel model, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                model.ParentList = await _menu.LoadParentAsync();
                model.RoleList = await _role.LoadParentAsync();
                return PartialView("Save", model);
            }
            var result = await _menu.CreateAsync(model);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Menu", new { Area = "Admin", page }),
                        message = result == null ? "Record creation Failed!!!" : "Record created successfully!!!",
                        position = "mainContent"
                    });
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _menu.GetAsync(id);
            model.ParentList = await _menu.LoadParentAsync(id);
            model.RoleList = await _role.LoadParentAsync();
            return PartialView("Save", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MenuViewModel model, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                model.ParentList = await _menu.LoadParentAsync(model.Id);
                model.RoleList = await _role.LoadParentAsync();
                return PartialView("Save", model);
            }
            var result = await _menu.UpdateAsync(model);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Menu", new { Area = "Admin", page }),
                        message = result == null ? "Record update Failed!!!" : "Record update successfully!!!",
                        position = "mainContent"
                    });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _menu.DeActiveAsync(id);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Menu", new { Area = "Admin" }),
                        message = result == 0 ? "Record deleted Failed!!!" : "Record deleted successfully!!!",
                        position = "mainContent"
                    });
        }
    }
}