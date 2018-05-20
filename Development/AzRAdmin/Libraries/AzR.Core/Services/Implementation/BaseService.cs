using AzR.Core.AuditLogs;
using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Core.Services.Interface;
using AzR.Utilities.Exentions;
using AzR.Utilities.Helpers;
using AzR.Utilities.Securities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AzR.Core.Services.Implementation
{
    public class BaseService : IBaseService
    {

        private IRepository<LoginHistory> _login;
        private IUserRepository _user;
        private IRepository<Branch> _branch;
        private IRoleRepository _role;
        public BaseService(IRepository<LoginHistory> login, IUserRepository user, IRepository<Branch> branch, IRoleRepository role)
        {
            _login = login;
            _user = user;
            _branch = branch;
            _role = role;
        }

        #region LOGIN SIGNOUT

        public LoginHistory LoginTime(string userName)
        {
            var user = _user.Find(u => u.UserName == userName);
            var id = AppIdentity.AppInfo + "U" + user.Id + "T" + DateTime.UtcNow.ToLong() + "E" +
                     GeneralHelper.UtcNowTicks;

            var model = new LoginHistory
            {
                Id = id,
                UserId = user.Id,
                LoginTime = DateTime.UtcNow.ToLong(),
                LogoutTime = null,
                AgentId = PcUniqueNumber.GetUserAgentInfo,
                ModifiedBy = user.Id,
                UserBranchId = user.BranchId
            };
            return _login.Create(model);
        }

        public string LoginId(int userId)
        {
            return _login.MaxFunc(u => u.Id.ToString(), u => u.UserId == userId);
        }


        public void LogOutTime(int userId)
        {
            var loginId = AppIdentity.AppUser.Id;
            if (!string.IsNullOrEmpty(loginId))
            {
                var model = _login.First(o => o.Id == loginId);
                model.AgentId = PcUniqueNumber.GetUserAgentInfo;
                model.LogoutTime = DateTime.UtcNow.ToLong();
                model.ModifiedBy = userId;
                _login.SaveChanges();
            }

        }

        #endregion

        public Branch GetOwner()
        {
            return _branch.FirstOrDefault(i => i.Id == 1);
        }
        public IEnumerable<ApplicationRole> GetAllRoleByUsers(int userId)
        {

            var roles = from ur in _user.FindAll(u => u.Id == userId).Include(r => r.Roles).SelectMany(y => y.Roles)
                        join r in _role.GetAll on ur.RoleId equals r.Id
                        select r;
            return roles.ToList();

        }
        public void SetCookie(string userName)
        {
            var login = LoginTime(userName);
            var user = AppUser(login);
            var userCookie = user.GetBySerial();
            var cookie = new ManageCookie();
            cookie.RemoveCookie("AzRADMINUSER");
            cookie.SetCookie("AzRADMINUSER", userCookie);

        }

        public AppUserPrincipal AppUser(LoginHistory login)
        {
            var user = _user.Find(u => u.Id == login.UserId);
            var applicationRoles = user.Roles.Select(s => s.Role).ToList();
            var branch = user.Branch;
            var role = applicationRoles.FirstOrDefault() ?? new ApplicationRole("UNKNOWN");
            List<int> branches;
            if (branch.ParentId == null)
            {
                branches = user.Branch.Children
                    .Select(s => s.Id)
                    .ToList();
                branches.Insert(0, 0);
            }
            else
            {
                branches = new List<int> { user.BranchId };
            }

            var model = new AppUserPrincipal(user.UserName)
            {
                Id = login.Id,
                UserId = user.Id,
                UniqueName = user.UserName,
                UserName = user.UserName,
                Name = user.FullName,
                Phone = user.PhoneNumber,
                Email = user.Email,
                ActiveBranchId = user.BranchId,
                BranchId = user.BranchId,
                ParentBranchId = branch.ParentId ?? 0,
                Expired = user.Expired,
                ActiveRoleName = role.Name,
                ActiveRoleId = role.Id,
                RoleIdList = applicationRoles.Select(s => s.Id).ToList(),
                RoleNameList = applicationRoles.Select(s => s.Name).ToList(),
                PermittedBranchList = branches

            };
            return model;
        }

        public async Task<bool> IsActive(string userName)
        {
            var user = _user.Find(p => p.Email == userName || p.UserName == userName);
            if (user == null) return false;
            var isInRole = await _user.IsInRoleAsync(user, "ADMIN");
            return isInRole || user.IsActive;
        }
    }
}
