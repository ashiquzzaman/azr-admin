using System.Collections.Generic;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Services.Interface
{
    public interface IUserPrivilegeManager
    {
        IEnumerable<UserPrivilegeViewModel> GetByUserId(int id);
        UserPrivilege Create(UserPrivilegeViewModel model);
        int Update(UserPrivilegeViewModel model);
        List<UserPrivilegeViewModel> GetHierarchy(List<UserPrivilegeViewModel> listPrivileges);
        List<UserPrivilegeViewModel> GetUserwisePrivilages(int userId);
        List<UserPrivilegeViewModel> GetPermissions(int userId);
        List<PermittedUserViewModel> GetAllPermittedUsers(string moduleName);
    }
}