using System.Collections.Generic;
using System.Threading.Tasks;
using AzR.Core.IdentityConfig;
using AzR.Core.Repository;

namespace AzR.Core.Repositories
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
    }
}