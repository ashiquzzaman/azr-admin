using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AzR.Core.Entities;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Implementation;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities;

namespace AzR.Core.Services.Implementation
{
    public class OrganizationManager : IOrganizationManager
    {

        private readonly IOrganizationRepository _organization;
        private IUserRepository _user;
        private RoleRepository _role;
        public OrganizationManager(IOrganizationRepository institution, IUserRepository user, RoleRepository role)
        {
            _organization = institution;
            _user = user;
            _role = role;
        }

        public async Task<List<DropDownItem>> LoadBranchsAsync()
        {
            var result = _organization.FindAll(b => b.IsActive && b.Id != 1).Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToListAsync();
            return await result;
        }
        public async Task<List<DropDownItem>> LoadAllOrgsAsync()
        {
            var result = _organization.FindAll(b => b.IsActive && b.Id != 1).Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToListAsync();
            return await result;
        }

        public IQueryable<OrganizationViewModel> GetAllAsync()
        {
            var result = _organization.FindAll(p => p.IsActive)
                .Select(_organization.ModelExpression).OrderBy(b => b.Id);
            return result;
        }
        public async Task<OrganizationViewModel> GetAsync(int id)
        {
            var model = _organization.EntityFactory(await _organization.FindAsync(o => o.Id == id));
            return model;
        }

        public async Task<List<DropDownItem>> LoadParentAsync()
        {
            var model = await _organization
                            .FindAllAsync(m => m.IsActive && m.ParentId == null);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
            return result;
        }

        public OrganizationViewModel GetOwner()
        {
            var model = _organization.FirstOrDefault(i => i.Id == 1);
            if (model == null) return new OrganizationViewModel();
            return _organization.EntityFactory(model);
        }

        public IEnumerable<ApplicationUser> GetAllUserByRole(int orgId, string roleName)
        {

            var role = _role.GetAllUser(roleName).First();
            var usersInRole =
                _user.GetAllUsers()
                    .Where(u => (u.Roles.Select(r => r.RoleId)
                        .Contains(role.RoleId)) && u.OrgId == orgId).ToList();
            return usersInRole;


        }

        public async Task<Organization> CreateAsync(OrganizationViewModel model)
        {

            try
            {
                var comapny = _organization.ModelFactory(model);
                return await _organization.CreateAsync(comapny);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return null;
            }

        }

        public async Task<Organization> UpdateAsync(OrganizationViewModel model)
        {
            try
            {
                var comapny = _organization.ModelFactory(model);
                await _organization.UpdateAsync(comapny);
                return comapny;
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return null;
            }
        }

        public async Task<int> ActiveAsync(int id)
        {
            _organization.First(o => o.Id == id).IsActive = true;
            return await _organization.SaveChangesAsync();
        }
        public async Task<int> DeActiveAsync(int id)
        {
            try
            {
                _organization.First(o => o.Id == id).IsActive = false;
                return await _organization.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return 0;
            }
        }
    }
}
