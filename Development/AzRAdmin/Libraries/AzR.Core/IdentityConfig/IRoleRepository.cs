using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AzR.Core.Config;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.IdentityConfig
{
    public interface IRoleRepository : IRepository<ApplicationRole>
    {
        Task<ApplicationRole> GetRoleByIdAsync(int id);
        Task<ApplicationRole> GetRoleByNameAsync(string name);
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        Task CreateRoleAsync(ApplicationRole role);
        Task UpdateRoleAsync(ApplicationRole role);
        IEnumerable<ApplicationUserRole> GetAllUser(string name);

        IEnumerable<ApplicationRole> GetAllRoles();
        ApplicationRole FindByName(string name);
        RoleViewModel EntityFactory(ApplicationRole model);
        ApplicationRole ModelFactory(RoleViewModel model);
        Expression<Func<ApplicationRole, RoleViewModel>> ModelExpression { get; }
        Task<IEnumerable<ApplicationUserRole>> GetAllUserAsync(string name);
        Task<IEnumerable<RoleViewModel>> GetRolesAsync();
        ApplicationRole FindById(int roleId);
    }
}