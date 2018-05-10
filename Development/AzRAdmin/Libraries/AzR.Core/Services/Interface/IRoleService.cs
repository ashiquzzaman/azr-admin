using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.ViewModels.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzR.Core.Services.Interface
{
    public interface IRoleService
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



        //Task<ApplicationRole> GetRoleByIdAsync(int id);
        //Task<ApplicationRole> GetRoleByNameAsync(string name);
        //Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        //Task CreateRoleAsync(ApplicationRole role);
        //Task UpdateRoleAsync(ApplicationRole role);
        //IEnumerable<ApplicationUserRole> GetAllUser(string name);

        //IEnumerable<ApplicationRole> GetAllRoles();
        //ApplicationRole FindByName(string name);
        //RoleViewModel EntityFactory(ApplicationRole model);
        //ApplicationRole ModelFactory(RoleViewModel model);
        //Expression<Func<ApplicationRole, RoleViewModel>> ModelExpression { get; }
        //Task<IEnumerable<ApplicationUserRole>> GetAllUserAsync(string name);
        //Task<IEnumerable<RoleViewModel>> GetRolesAsync();
        //ApplicationRole FindById(int roleId);
    }
}
