using System.Collections.Generic;
using AzR.Core.AppContexts;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;

namespace AzR.Core.Repositoies.Interface
{
    public interface IUserPrivilegeRepository : IRepository<UserPrivilege>
    {
        List<PermittedUserViewModel> GetAllPermittedUsers(string moduleName);
    }
}