using System.Collections.Generic;
using System.Threading.Tasks;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Services.Interface
{
    public interface IUserManager
    {
        ApplicationUser GetById(int id);
        IEnumerable<UserViewModel> GetAllInstituteUsers(int instituteId);
        bool Create(UserViewModel model, int uid = 0, int sid = 0);
        ApplicationUser Create(UserViewModel model);
        IEnumerable<UserViewModel> GetAll();
        ApplicationUser Update(UserViewModel model, int uid = 0, int sid = 0);
        UserViewModel GetUserByName(string name);
        UserViewModel GetUserById(int id);
        IEnumerable<ApplicationUser> GetUsersInRole(string roleName);
        IEnumerable<UserViewModel> GetAllUserByRole(string roleName);
        IEnumerable<UserViewModel> GetAllUserByRole(string roleName, int orgId);
        void DeActive(int id);
        IEnumerable<DropDownItem> LoadUser(int orgId, string role);
        string GetNameById(int id);
        List<DropDownItem> LoadUsers();

        Task<IEnumerable<ApplicationRole>> GetAllRoleByUsersAsync(int userId);
        bool IsInRole(int userId, string name);
        Task<int> DeActiveAsync(int id);
    }
}