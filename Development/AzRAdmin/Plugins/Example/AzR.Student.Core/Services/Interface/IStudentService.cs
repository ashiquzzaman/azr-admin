using AzR.Student.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace AzR.Student.Core.Services.Interface
{
    public interface IStudentService
    {
        IQueryable<StudentViewModel> GetAllAsync();
        Task<StudentViewModel> GetAsync(int id);
        Task<Models.Student> CreateOrUpdateAsync(StudentViewModel model);
    }
}
