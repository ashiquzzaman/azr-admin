using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.Repositoies.Interface;
using AzR.Core.ViewModels.Admin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AzR.Core.Repositoies.Implementation
{
    public class UserMenuRepository : Repository<UserMenu>, IUserMenuRepository
    {
        public UserMenuRepository(DbContext context) : base(context)
        {
        }
        public List<PermittedUserViewModel> GetAllPermittedUsers(string moduleName)
        {
            var where1 = !string.IsNullOrWhiteSpace(moduleName) ? string.Format("AND m.ModuleName = '{0}'", moduleName) : "";
            var where = !string.IsNullOrWhiteSpace(moduleName) ? string.Format("AND rp.ModuleName = '{0}'", moduleName) : "";

            var query = string.Format(
                " SELECT cast ((CASE WHEN up.UserId IS NULL THEN 0 ELSE 1 END)as bit) AS IsPermitted "
                + ",u.Id AS UserId, u.FullName, u.UserName, u.RoleId, r.RoleName, rp.ReviewLevel,rp.ModuleName "
                + "FROM Users u "
                + "LEFT JOIN "
                + "(SELECT DISTINCT up.UserId FROM UserPrivileges up "
                + "INNER JOIN MenuConfigs AS m ON up.MenuId = m.Id "
                + "WHERE up.IsActive = 1 AND m.Active = 1 {0}) AS up ON u.Id = up.UserId "
                + "INNER JOIN Roles r ON u.RoleId = r.Id "
                + "INNER JOIN RolePrivilages rp ON u.RoleId = rp.RoleId "
                + "WHERE u.ActiveStatus = 1 AND rp.IsActive = 1 {1}", where1, where);

            var result = ExecuteQuery<PermittedUserViewModel>(query).ToList();
            return result;
        }

    }
}