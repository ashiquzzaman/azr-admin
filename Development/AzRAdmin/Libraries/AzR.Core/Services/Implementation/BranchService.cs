using AzR.Core.Entities;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Implementation;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities.Exentions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AzR.Core.Services.Implementation
{
    public class BranchService : IBranchService
    {

        private readonly IBranchRepository _branch;
        private IUserRepository _user;
        private RoleRepository _role;
        public BranchService(IBranchRepository institution, IUserRepository user, RoleRepository role)
        {
            _branch = institution;
            _user = user;
            _role = role;
        }

        public async Task<List<DropDownItem>> LoadBranchsAsync()
        {
            var result = _branch.FindAll(b => b.IsActive && b.Id != 1).Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToListAsync();
            return await result;
        }
        public async Task<List<DropDownItem>> LoadAllOrgsAsync()
        {
            var result = _branch.FindAll(b => b.IsActive && b.Id != 1).Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToListAsync();
            return await result;
        }

        public IQueryable<BranchViewModel> GetAllAsync()
        {
            var result = _branch.FindAll(p => p.IsActive)
                .Select(_branch.ModelExpression).OrderBy(b => b.Id);
            return result;
        }
        public async Task<BranchViewModel> GetAsync(int id)
        {
            var model = _branch.EntityFactory(await _branch.FindAsync(o => o.Id == id));
            return model;
        }

        public async Task<List<DropDownItem>> LoadParentAsync()
        {
            var model = await _branch
                            .FindAllAsync(m => m.IsActive && m.ParentId == null);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
            return result;
        }

        public BranchViewModel GetOwner()
        {
            var model = _branch.FirstOrDefault(i => i.Id == 1);
            if (model == null) return new BranchViewModel();
            return _branch.EntityFactory(model);
        }

        public IEnumerable<ApplicationUser> GetAllUserByRole(int orgId, string roleName)
        {

            var role = _role.GetAllUser(roleName).First();
            var usersInRole =
                _user.GetAllUsers()
                    .Where(u => (u.Roles.Select(r => r.RoleId)
                        .Contains(role.RoleId)) && u.BranchId == orgId).ToList();
            return usersInRole;


        }

        public async Task<Branch> CreateAsync(BranchViewModel model)
        {

            try
            {
                var comapny = _branch.ModelFactory(model);
                return await _branch.CreateAsync(comapny);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return null;
            }

        }

        public async Task<Branch> UpdateAsync(BranchViewModel model)
        {
            try
            {
                var comapny = _branch.ModelFactory(model);
                await _branch.UpdateAsync(comapny);
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
            _branch.First(o => o.Id == id).IsActive = true;
            return await _branch.SaveChangesAsync();
        }
        public async Task<int> DeActiveAsync(int id)
        {
            try
            {
                _branch.First(o => o.Id == id).IsActive = false;
                return await _branch.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return 0;
            }
        }
    }
}
