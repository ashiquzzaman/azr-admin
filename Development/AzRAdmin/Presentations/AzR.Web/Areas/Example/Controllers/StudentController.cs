using AzR.Student.Core.Services.Interface;
using AzR.Web.Controllers;
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
        public ActionResult Index()
        {
            var model = _student.GetAll();
            return PartialView(model);
        }

        public ActionResult Create()
        {
            throw new System.NotImplementedException();
        }
    }
}