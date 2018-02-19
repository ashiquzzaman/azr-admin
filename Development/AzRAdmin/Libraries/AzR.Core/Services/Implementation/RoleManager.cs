using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities;

namespace AzR.Core.Services.Implementation
{

    public class RoleManager : IRoleManager
    {
        private IRoleRepository _role;

        public RoleManager(IRoleRepository role)
        {
            _role = role;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllAsync()
        {
            var roles = await _role.GetRolesAsync();
            return roles;
        }

        public async Task<bool> IsExistAsync(string name, int id = 0)
        {
            var roles = await _role.IsExistAsync(s => s.Name == name && s.Id != id);
            return roles;
        }
        public async Task<RoleViewModel> GetAsync(int id)
        {
            var role = await _role.GetRoleByIdAsync(id);
            var result = _role.EntityFactory(role);
            return result;
        }

        public async Task<ApplicationRole> CreateAsync(RoleViewModel model)
        {
            try
            {
                var result = _role.ModelFactory(model);
                return await _role.CreateAsync(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return null;
            }
        }

        public async Task<ApplicationRole> UpdateAsync(RoleViewModel model)
        {
            try
            {
                var result = _role.ModelFactory(model);
                await _role.UpdateAsync(result);
                return result;
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return null;
            }
        }

        public async Task<int> ActiveAsync(int id)
        {
            var role = _role.First(o => o.Id == id);
            role.IsActive = true;
            role.IsDisplay = true;
            return await _role.SaveChangesAsync();
        }
        public async Task<int> DeActiveAsync(int id)
        {
            try
            {
                var role = _role.First(o => o.Id == id);
                role.IsActive = false;
                role.IsDisplay = false;
                return await _role.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return 0;
            }
        }

        public async Task<List<DropDownItem>> LoadParentAsync()
        {
            var model = await _role.FindAllAsync(m => m.IsActive && m.IsDisplay);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
            return result;
        }
        public async Task<List<DropDownItem>> LoadParentAsync(int id)
        {
            var model = await _role.FindAllAsync(m => m.IsActive && m.IsDisplay && m.Id != id);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
            return result;
        }

        public async Task<List<DropDownItem>> LoadRoleByNameAsync()
        {
            var model = await _role.FindAllAsync(m => m.IsActive && m.IsDisplay);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
            return result;
        }

    }


}
