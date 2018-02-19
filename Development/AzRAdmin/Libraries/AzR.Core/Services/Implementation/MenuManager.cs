using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AzR.Core.Entities;
using AzR.Core.Enumerations;
using AzR.Core.HelperModels;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities;

namespace AzR.Core.Services.Implementation
{
    public class MenuManager : IMenuManager
    {
        private readonly IMenuRepository _menu;
        private readonly IUserPrivilegeRepository _userPrivilege;
        private IRoleRepository _role;
        private readonly List<MenuType> _menuTypes = new List<MenuType> { MenuType.Module, MenuType.Menu };

        public MenuManager(IMenuRepository menu, IUserPrivilegeRepository userPrivilege, IRoleRepository role)
        {
            _menu = menu;
            _userPrivilege = userPrivilege;
            _role = role;
        }

        public IQueryable<MenuViewModel> GetAllAsync()
        {
            var result = from m in _menu.FindAll(m => m.IsActive).Include(p => p.Parent)
                         join r in _role.FindAll(r => r.IsActive && r.IsDisplay) on m.RoleId equals r.Id
                         select new MenuViewModel
                         {
                             Id = m.Id,
                             DisplayName = m.DisplayName,
                             Url = m.Url,
                             ParentId = m.ParentId,
                             ParentName = m.Parent.DisplayName,
                             MenuOrder = m.MenuOrder,
                             Icon = m.Icon,
                             MenuType = m.MenuType,
                             RoleId = m.RoleId,
                             RoleName = r.Name,
                             IsActive = m.IsActive
                         };


            return result;
        }

        public async Task<MenuViewModel> GetAsync(int id)
        {
            var model = _menu.EntityFactory(await _menu.FindAsync(o => o.Id == id));
            return model;
        }

        public async Task<List<DropDownItem>> LoadParentAsync()
        {
            var model = await _menu
                .FindAllAsync(m => m.IsActive && m.ParentId == null);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.DisplayName
            }).ToList();
            return result;
        }

        public async Task<List<DropDownItem>> LoadParentAsync(int id)
        {
            var model = await _menu
                .FindAllAsync(m => m.IsActive && m.ParentId == null && m.Id != id);
            var result = model.Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.DisplayName
            }).ToList();
            return result;
        }

        public async Task<Menu> CreateAsync(MenuViewModel model)
        {

            try
            {
                var result = _menu.ModelFactory(model);
                return await _menu.CreateAsync(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return null;
            }

        }

        public async Task<Menu> UpdateAsync(MenuViewModel model)
        {
            try
            {
                var result = _menu.ModelFactory(model);
                await _menu.UpdateAsync(result);
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
            _menu.First(o => o.Id == id).IsActive = true;
            return await _menu.SaveChangesAsync();
        }

        public async Task<int> DeActiveAsync(int id)
        {
            try
            {
                _menu.First(o => o.Id == id).IsActive = false;
                return await _menu.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return 0;
            }
        }

        private bool SetUserPrivilagesByRole(int roleId, int menuId)
        {
            string query =
                string.Format(
                    @"INSERT INTO UserPrivileges(UserId, MenuId, IsActive, FullPermission, ViewPermission, AddPermission, EditPermission, DeletePermission, DetailViewPermission, ReportViewPermission)
                                           SELECT u.Id as UserId, m.Id as MenuId, 1 as IsActive, 1 as FullPermission, 1 as ViewPermission, 1 as AddPermission, 1 as EditPermission, 1 as DeletePermission, 1 as DetailViewPermission, 1 as ReportViewPermission
                                           FROM Users u
                                           INNER JOIN Menus m
                                           ON u.RoleId = m.RoleId
                                           WHERE u.RoleId = {0} and m.Id = {1}",
                    roleId, menuId);
            return _userPrivilege.ExecuteCommand(query) > 0;
        }

        public IEnumerable<Menu> GetByRole(int roleId)
        {
            var model =
                _menu.FindAll(r => r.RoleId == roleId && _menuTypes.Contains(r.MenuType))
                    .Include(p => p.Role)
                    .Include(p => p.Parent)
                    .Include(p => p.Children)
                    .OrderBy(o => o.MenuType).ThenBy(m => m.MenuOrder).ToList();
            return model;
        }

        public IEnumerable<Menu> GetByUser(int userId)
        {
            var model =
            (from m in _menu.FindAll(r => _menuTypes.Contains(r.MenuType))
                    .Include(p => p.Children)
             join p in _userPrivilege.FindAll(w => w.IsActive && w.UserId == userId
                                                   && (w.FullPermission || w.ViewPermission || w.AddPermission ||
                                                       w.EditPermission || w.DeletePermission ||
                                                       w.DetailViewPermission || w.ReportViewPermission))
             on m.Id equals p.MenuId
             select m
            ).OrderBy(o => o.MenuType).ThenBy(m => m.MenuOrder);


            return model;
        }

        public IEnumerable<Menu> GetMenu(int userId, int roleId, bool accessByUser = true)
        {
            var model = new List<Menu>();
            var modelTemp = accessByUser
                ? GetByUser(userId).Where(m => m.IsActive).ToList()
                : GetByRole(roleId).Where(m => m.IsActive).ToList();
            var hierarchy = modelTemp.Where(c => c.ParentId == null || c.ParentId == 0)
                .Select(c => new Menu
                {
                    Id = c.Id,
                    RoleId = c.RoleId,
                    ParentId = c.ParentId,
                    DisplayName = c.DisplayName,
                    MenuOrder = c.MenuOrder,
                    MenuType = c.MenuType,
                    Icon = c.Icon,
                    IsActive = c.IsActive,
                    Url = c.Url,
                    Children = modelTemp.Where(x => x.ParentId == c.Id).ToList()
                }).ToList();

            foreach (var item in hierarchy)
            {
                if (item.Children.Count == 0 && string.IsNullOrWhiteSpace(item.Url)) continue;
                if (item.Children.Count != 0)
                {
                    model.Add(item);
                    model.AddRange(item.Children);
                }
                else
                {
                    model.Add(item);
                }
            }
            return model.Where(m => m.IsActive);
        }

    }
}
