using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AzR.Core.AppContexts;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Interface;
using AzR.Core.ViewModels.Admin;
using Microsoft.AspNet.Identity;

namespace AzR.Core.Repositoies.Implementation
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
        public async Task<IEnumerable<ApplicationUserRole>> GetAllUserAsync(string name)
        {
            var users = await _manager.FindByNameAsync(name);
            return users.Users;
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
        public ApplicationRole FindById(int roleId)
        {
            var users = _manager.FindById(roleId);
            return users;
        }

        public IEnumerable<ApplicationRole> GetAllRoles()
        {
            return _store.Roles.ToArray();
        }

        public async Task<IEnumerable<RoleViewModel>> GetRolesAsync()
        {
            var roles = _store.Roles.Where(r => r.IsActive && r.IsDisplay).Select(ModelExpression);
            return await roles.ToArrayAsync();
        }

        public RoleViewModel EntityFactory(ApplicationRole model)
        {
            return new RoleViewModel
            {
                Id = model.Id,
                RoleCode = model.RoleCode,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive

            };
        }
        public ApplicationRole ModelFactory(RoleViewModel model)
        {
            return new ApplicationRole
            {
                Id = model.Id,
                RoleCode = model.RoleCode,
                Name = model.Name,
                Description = model.Description,
                IsDisplay = model.IsActive,
                IsActive = model.IsActive

            };
        }
        public Expression<Func<ApplicationRole, RoleViewModel>> ModelExpression
        {
            get
            {
                return model => new RoleViewModel
                {
                    Id = model.Id,
                    RoleCode = model.RoleCode,
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = model.IsActive
                };
            }
        }

    }
}