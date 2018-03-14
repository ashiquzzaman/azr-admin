using AzR.Core.Entities;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AzR.Core.Services.Implementation
{
    public class UserPrivilegeService : IUserPrivilegeService
    {
        private IUserPrivilegeRepository _userPrivilege;
        private IUserRepository _user;
        private IMenuRepository _menu;
        public UserPrivilegeService(IUserPrivilegeRepository userPrivilege
            , IUserRepository user
            , IMenuRepository menu
        )
        {
            _userPrivilege = userPrivilege;
            _user = user;
            _menu = menu;
        }

        public IEnumerable<UserPrivilegeViewModel> GetByUserId(int id)
        {
            try
            {

                var usrInfo = from u in _user.All()
                              join r in _user.All().Include(s => s.Roles).SelectMany(s => s.Roles) on u.Id equals r.UserId
                              join m in _menu.All() on r.RoleId equals m.RoleId
                              where u.Id == id
                              select new UserMenuViewModel
                              {
                                  UserId = u.Id,
                                  UserName = u.FullName,
                                  MenuId = m.Id,
                                  MenuName = m.DisplayName
                              };

                var model = (from u in usrInfo
                             join mc in _menu.All() on u.MenuId equals mc.Id
                             join pp in _userPrivilege.All() on new { u.UserId, u.MenuId } equals new { pp.UserId, pp.MenuId } into lgj
                             from p in lgj.DefaultIfEmpty()
                             select new UserPrivilegeViewModel
                             {
                                 Id = p == null ? 0 : p.Id,
                                 MenuId = u.MenuId,
                                 MenuType = mc.MenuType,
                                 MenuName = u.MenuName,
                                 ParentId = mc.ParentId ?? 0,
                                 UserId = u.UserId,
                                 UserName = u.UserName,
                                 IsActive = p != null && p.IsActive,
                                 FullPermission = p != null && p.FullPermission,
                                 AddPermission = p != null && p.AddPermission,
                                 EditPermission = p != null && p.EditPermission,
                                 ViewPermission = p != null && p.ViewPermission,
                                 DeletePermission = p != null && p.DeletePermission,
                                 DetailViewPermission = p != null && p.DetailViewPermission,
                                 ReportViewPermission = p != null && p.ReportViewPermission,
                                 Url = mc.Url
                             }).Where(w => w.UserId == id).AsEnumerable();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public UserPrivilege Create(UserPrivilegeViewModel model)
        {
            var itm = new UserPrivilege
            {
                UserId = model.UserId,
                MenuId = model.MenuId,
                IsActive = model.IsActive,
                FullPermission = model.FullPermission,
                ViewPermission = model.ViewPermission,
                AddPermission = model.AddPermission,
                EditPermission = model.EditPermission,
                DeletePermission = model.DeletePermission,
                DetailViewPermission = model.DetailViewPermission,
                ReportViewPermission = model.ReportViewPermission
            };

            var tablename = "UserPrivileges";
            return _userPrivilege.Create(itm);
        }

        public int Update(UserPrivilegeViewModel model)
        {
            var itm = new UserPrivilege()
            {
                Id = model.Id,
                UserId = model.UserId,
                MenuId = model.MenuId,
                IsActive = model.IsActive,
                FullPermission = model.FullPermission,
                ViewPermission = model.ViewPermission,
                AddPermission = model.AddPermission,
                EditPermission = model.EditPermission,
                DeletePermission = model.DeletePermission,
                DetailViewPermission = model.DetailViewPermission,
                ReportViewPermission = model.ReportViewPermission
            };
            var tablename = "UserPrivileges";
            return _userPrivilege.Update(itm);
        }

        public List<UserPrivilegeViewModel> GetHierarchy(List<UserPrivilegeViewModel> listPrivileges)
        {
            var hierarchy = listPrivileges.Where(module => module.ParentId == null || module.ParentId == 0).Select(module => new UserPrivilegeViewModel
            {
                Id = module.Id,
                MenuName = module.MenuName,
                ParentId = module.ParentId,
                UserId = module.UserId,
                UserName = module.UserName,
                MenuId = module.MenuId,
                MenuType = module.MenuType,
                IsActive = module.IsActive,
                FullPermission = module.FullPermission,
                AddPermission = module.AddPermission,
                ViewPermission = module.ViewPermission,
                EditPermission = module.EditPermission,
                DeletePermission = module.DeletePermission,
                DetailViewPermission = module.DetailViewPermission,
                ReportViewPermission = module.ReportViewPermission,
                Children = listPrivileges.Where(menu => menu.ParentId == module.MenuId).Select(menu => new UserPrivilegeViewModel
                {
                    Id = menu.Id,
                    MenuName = menu.MenuName,
                    ParentId = menu.ParentId,
                    UserId = menu.UserId,
                    UserName = menu.UserName,
                    MenuId = menu.MenuId,
                    MenuType = menu.MenuType,
                    IsActive = menu.IsActive,
                    FullPermission = menu.FullPermission,
                    AddPermission = menu.AddPermission,
                    ViewPermission = menu.ViewPermission,
                    EditPermission = menu.EditPermission,
                    DeletePermission = menu.DeletePermission,
                    DetailViewPermission = menu.DetailViewPermission,
                    ReportViewPermission = menu.ReportViewPermission,
                    Children = listPrivileges.Where(tab => tab.ParentId == menu.MenuId).Select(tab => new UserPrivilegeViewModel
                    {
                        Id = tab.Id,
                        MenuName = tab.MenuName,
                        ParentId = tab.ParentId,
                        UserId = tab.UserId,
                        UserName = tab.UserName,
                        MenuId = tab.MenuId,
                        MenuType = tab.MenuType,
                        IsActive = tab.IsActive,
                        FullPermission = tab.FullPermission,
                        AddPermission = tab.AddPermission,
                        ViewPermission = tab.ViewPermission,
                        EditPermission = tab.EditPermission,
                        DeletePermission = tab.DeletePermission,
                        DetailViewPermission = tab.DetailViewPermission,
                        ReportViewPermission = tab.ReportViewPermission,
                        Children = null
                    }).ToList()
                }).ToList()
            }).ToList();

            return hierarchy;
        }

        public List<UserPrivilegeViewModel> GetUserwisePrivilages(int userId)
        {
            var lstUserPrivilege = new List<UserPrivilegeViewModel>();
            List<UserPrivilegeViewModel> finalList = new List<UserPrivilegeViewModel>();
            lstUserPrivilege = GetByUserId(userId).OrderBy(x => x.ParentId).ToList();
            var hierarchy = GetHierarchy(lstUserPrivilege);
            foreach (var item in hierarchy)
            {
                if (item.Children.Count != 0)
                {
                    finalList.Add(item);
                    foreach (var subItem in item.Children)
                    {
                        finalList.Add(subItem);
                        finalList.AddRange(subItem.Children);
                    }
                }
                else
                {
                    finalList.Add(item);
                }
            }
            return finalList;
        }
        public List<UserPrivilegeViewModel> GetPermissions(int userId)
        {

            var lstUserPrivilege = GetByUserId(userId).OrderBy(x => x.ParentId).ToList();
            return lstUserPrivilege;
        }

        public List<PermittedUserViewModel> GetAllPermittedUsers(string moduleName)
        {
            var result = _userPrivilege.GetAllPermittedUsers(moduleName);
            return result;
        }


    }

}
