using System.Collections.Generic;
using System.Threading.Tasks;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Services.Interface
{
    public interface IRoleManager
    {
        Task<IEnumerable<RoleViewModel>> GetAllAsync();
        Task<RoleViewModel> GetAsync(int id);
        Task<ApplicationRole> CreateAsync(RoleViewModel model);
        Task<ApplicationRole> UpdateAsync(RoleViewModel model);
        Task<int> DeActiveAsync(int id);
        Task<int> ActiveAsync(int id);
        Task<bool> IsExistAsync(string name, int id = 0);
        Task<List<DropDownItem>> LoadParentAsync();
        Task<List<DropDownItem>> LoadParentAsync(int id);
        Task<List<DropDownItem>> LoadRoleByNameAsync();
    }
}
