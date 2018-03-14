using AzR.Core.Entities;
using AzR.Core.HelperModels;
using AzR.Core.ViewModels.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzR.Core.Services.Interface
{
    public interface IMenuService
    {
        IQueryable<MenuViewModel> GetAllAsync();
        Task<MenuViewModel> GetAsync(int id);
        Task<List<DropDownItem>> LoadParentAsync();
        Task<List<DropDownItem>> LoadParentAsync(int id);
        Task<Menu> CreateAsync(MenuViewModel model);
        Task<Menu> UpdateAsync(MenuViewModel model);
        Task<int> ActiveAsync(int id);
        Task<int> DeActiveAsync(int id);
        IEnumerable<Menu> GetByRole(int roleId);
        IEnumerable<Menu> GetByUser(int userId);
        IEnumerable<Menu> GetMenu(int userId, int roleId, bool accessByUser = true);
    }
}