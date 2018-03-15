using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.ViewModels.Admin;
using System.Collections.Generic;

namespace AzR.Core.Repositoies.Interface
{
    public interface IUserMenuRepository : IRepository<UserMenu>
    {
        List<PermittedUserViewModel> GetAllPermittedUsers(string moduleName);
    }
}