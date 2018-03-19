using AzR.Core.Config;
using AzR.Student.Core.Services.Interface;
using AzR.Student.Core.ViewModels;
using AzR.Web.Root.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AzR.Web.Areas.Example.Controllers
{
    public class StudentController : BaseController
    {
        private IStudentService _student;
        public StudentController(IStudentService student)
        {
            _student = student;
        }
        // GET: Example/Student
        public async Task<ActionResult> Index(int? page, int pageSize = 30)
        {
            page = page == null || page == 0 ? 1 : page;
            var model = await _student.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync((int)page, pageSize);
            return PartialView(model);
        }

        public async Task<ActionResult> Save(int? id)
        {
            var model = id != null && id > 0
                ? await _student.GetAsync((int)id)
                : new StudentViewModel
                {
                    Id = -1,
                };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(StudentViewModel model, int page = 1)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("Save", model);
            }

            var result = await _student.CreateOrUpdateAsync(model);

            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Student", new { Area = "Example", page }),
                        message = result == null ? "Record update Failed!!!" : "Record update successfully!!!",
                        position = "mainContent"
                    });
        }

    }
}