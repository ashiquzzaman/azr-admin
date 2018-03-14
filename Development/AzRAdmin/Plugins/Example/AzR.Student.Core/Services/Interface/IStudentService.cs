using AzR.Student.Core.ViewModels;
using System.Collections.Generic;

namespace AzR.Student.Core.Services.Interface
{
    public interface IStudentService
    {
        List<StudentViewModel> GetAll();
    }
}
