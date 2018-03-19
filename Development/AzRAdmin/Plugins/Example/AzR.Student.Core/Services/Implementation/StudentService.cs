using AzR.Student.Core.Repositoies.Interface;
using AzR.Student.Core.Services.Interface;
using AzR.Student.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace AzR.Student.Core.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _student;

        public StudentService(IStudentRepository student)
        {
            _student = student;
        }

        public IQueryable<StudentViewModel> GetAllAsync()
        {
            var students = _student.GetAll.Select(s => new StudentViewModel
            {
                Id = s.Id,
                Name = s.Name,
            });
            return students;
        }

        public async Task<StudentViewModel> GetAsync(int id)
        {
            var student = await _student.FindAsync(s => s.Id == id);

            var result = new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name,
            };
            return result;
        }

        public async Task<Models.Student> CreateOrUpdateAsync(StudentViewModel model)
        {
            if (model.Id > 0)
            {
                var student = _student.Find(s => s.Id == model.Id);
                student.Name = model.Name;
                await _student.SaveChangesAsync();
                return student;
            }
            else
            {
                var student = new Models.Student
                {
                    Name = model.Name
                };
                return await _student.CreateAsync(student);
            }
        }
    }
}
