using AzR.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AzR.Core.AppContexts;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;

namespace AzR.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {
        private readonly IBranchService _organization;

        public OrganizationController(IBranchService organization, IBaseService general) : base(general)
        {
            _organization = organization;
        }

        // GET: admin/Organization
        public async Task<ActionResult> Index(int? page, int pageSize = 30)
        {
            page = page == null || page == 0 ? 1 : page;
            var model = await _organization.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync((int)page, pageSize);
            return PartialView(model);
        }
        public async Task<ActionResult> Details(int id)
        {
            var model = await _organization.GetAsync(id);
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new OrganizationViewModel
            {
                ParentId = 1,
                Active = true,
                //ParentList = _organization.LoadParents()
            };
            return PartialView("Save", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizationViewModel model, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                //model.ParentList = _organization.LoadParents();
                return PartialView("Save", model);
            }

            var result = await _organization.CreateAsync(model);

            return
              Json(
                  new
                  {
                      redirectTo = Url.Action("Index", "Organization", new { Area = "Admin", page }),
                      message = result == null ? "Record creation Failed!!!" : "Record created successfully!!!",
                      position = "mainContent"
                  });
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _organization.GetAsync(id);
            // model.ParentList = _organization.LoadParents();
            return PartialView("Save", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrganizationViewModel model, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                // model.ParentList = _organization.LoadParents();
                return PartialView("Save", model);
            }

            var result = await _organization.UpdateAsync(model);

            return
              Json(
                  new
                  {
                      redirectTo = Url.Action("Index", "Organization", new { Area = "Admin", page }),
                      message = result == null ? "Record update Failed!!!" : "Record update successfully!!!",
                      position = "mainContent"
                  });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _organization.DeActiveAsync(id);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Organization", new { Area = "Admin" }),
                        message = result == 0 ? "Record deleted Failed!!!" : "Record deleted successfully!!!",
                        position = "mainContent"
                    });
        }
    }
}