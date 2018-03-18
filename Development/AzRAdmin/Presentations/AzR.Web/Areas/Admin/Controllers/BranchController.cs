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
    [Authorize]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branch;

        public BranchController(IBranchService branch)
        {
            _branch = branch;
        }

        // GET: admin/Branch
        public async Task<ActionResult> Index(int? page, int pageSize = 30)
        {
            page = page == null || page == 0 ? 1 : page;
            var model = await _branch.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync((int)page, pageSize);
            return PartialView(model);
        }
        public async Task<ActionResult> Details(int id)
        {
            var model = await _branch.GetAsync(id);
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new BranchViewModel
            {
                ParentId = 1,
                Active = true,
                //ParentList = _branch.LoadParents()
            };
            return PartialView("Save", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BranchViewModel model, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                //model.ParentList = _branch.LoadParents();
                return PartialView("Save", model);
            }

            var result = await _branch.CreateAsync(model);

            return
              Json(
                  new
                  {
                      redirectTo = Url.Action("Index", "Branch", new { Area = "Admin", page }),
                      message = result == null ? "Record creation Failed!!!" : "Record created successfully!!!",
                      position = "mainContent"
                  });
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _branch.GetAsync(id);
            // model.ParentList = _branch.LoadParents();
            return PartialView("Save", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BranchViewModel model, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                // model.ParentList = _branch.LoadParents();
                return PartialView("Save", model);
            }

            var result = await _branch.UpdateAsync(model);

            return
              Json(
                  new
                  {
                      redirectTo = Url.Action("Index", "Branch", new { Area = "Admin", page }),
                      message = result == null ? "Record update Failed!!!" : "Record update successfully!!!",
                      position = "mainContent"
                  });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _branch.DeActiveAsync(id);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Branch", new { Area = "Admin" }),
                        message = result == 0 ? "Record deleted Failed!!!" : "Record deleted successfully!!!",
                        position = "mainContent"
                    });
        }
    }
}