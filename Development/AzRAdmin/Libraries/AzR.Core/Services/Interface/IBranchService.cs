using AzR.Core.Entities;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.ViewModels.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzR.Core.Services.Interface
{
    public interface IBranchService
    {
        IQueryable<BranchViewModel> GetAllAsync();
        Task<BranchViewModel> GetAsync(int id);
        BranchViewModel GetOwner();
        IEnumerable<ApplicationUser> GetAllUserByRole(int orgId, string roleName);
        Task<int> ActiveAsync(int id);
        Task<int> DeActiveAsync(int id);
        Task<List<DropDownItem>> LoadParentAsync();
        Task<Branch> CreateAsync(BranchViewModel model);
        Task<Branch> UpdateAsync(BranchViewModel model);
        Task<List<DropDownItem>> LoadBranchsAsync();
        Task<List<DropDownItem>> LoadAllOrgsAsync();
    }
}
