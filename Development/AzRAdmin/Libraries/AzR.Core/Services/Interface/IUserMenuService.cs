using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;
using System.Collections.Generic;

namespace AzR.Core.Services.Interface
{
    public interface IUserMenuService
    {
        IEnumerable<UserPrivilegeViewModel> GetByUserId(int id);
        UserMenu Create(UserPrivilegeViewModel model);
        int Update(UserPrivilegeViewModel model);
        List<UserPrivilegeViewModel> GetHierarchy(List<UserPrivilegeViewModel> listPrivileges);
        List<UserPrivilegeViewModel> GetUserwisePrivilages(int userId);
        List<UserPrivilegeViewModel> GetPermissions(int userId);
        List<PermittedUserViewModel> GetAllPermittedUsers(string moduleName);
    }
}