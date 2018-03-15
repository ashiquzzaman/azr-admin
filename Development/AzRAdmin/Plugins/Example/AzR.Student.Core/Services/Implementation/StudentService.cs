using AzR.Student.Core.Repositoies.Interface;
using AzR.Student.Core.Services.Interface;
using AzR.Student.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AzR.Student.Core.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _student;

        public StudentService(IStudentRepository student)
        {
            _student = student;
        }

        public List<StudentViewModel> GetAll()
        {
            var students = _student.GetAll.Select(s => new StudentViewModel
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();
            return students;
        }
    }
}
