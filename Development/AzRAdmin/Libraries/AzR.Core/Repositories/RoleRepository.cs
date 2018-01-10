using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AzR.Core.IdentityConfig;
using AzR.Core.Repository;
using Microsoft.AspNet.Identity;

namespace AzR.Core.Repositories
{
    public class RoleRepository : Repository<ApplicationRole>, IRoleRepository
    {
        private readonly ApplicationRoleStore _store;
        private readonly ApplicationRoleManager _manager;


        public RoleRepository(DbContext context) : base(context)
        {
            _store = new ApplicationRoleStore(context);
            _manager = new ApplicationRoleManager(_store);
        }

        public async Task<ApplicationRole> GetRoleByNameAsync(string name)
        {
            return await _store.FindByNameAsync(name);
        }
        public async Task<ApplicationRole> GetRoleByIdAsync(int id)
        {
            return await _store.FindByIdAsync(id);

        }
        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            return await _store.Roles.ToArrayAsync();
        }
        public IEnumerable<ApplicationUserRole> GetAllUser(string name)
        {
            var users = _manager.FindByName(name).Users;
            return users;
        }

        public async Task CreateRoleAsync(ApplicationRole role)
        {
            await _manager.CreateAsync(role);
        }

        public async Task UpdateRoleAsync(ApplicationRole role)
        {
            await _manager.UpdateAsync(role);
        }

        public ApplicationRole FindByName(string name)
        {
            var users = _manager.FindByName(name);
            return users;
        }

        public IEnumerable<ApplicationRole> GetAllRoles()
        {
            return _store.Roles.ToArray();
        }

    }
}