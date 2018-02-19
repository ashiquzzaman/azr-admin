using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzR.Core.Entities;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Services.Interface
{
    public interface IOrganizationManager
    {
        IQueryable<OrganizationViewModel> GetAllAsync();
        Task<OrganizationViewModel> GetAsync(int id);
        OrganizationViewModel GetOwner();
        IEnumerable<ApplicationUser> GetAllUserByRole(int orgId, string roleName);
        Task<int> ActiveAsync(int id);
        Task<int> DeActiveAsync(int id);
        Task<List<DropDownItem>> LoadParentAsync();
        Task<Organization> CreateAsync(OrganizationViewModel model);
        Task<Organization> UpdateAsync(OrganizationViewModel model);
        Task<List<DropDownItem>> LoadBranchsAsync();
        Task<List<DropDownItem>> LoadAllOrgsAsync();
    }
}
