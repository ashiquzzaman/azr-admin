using AzR.Core.AuditLogs;
using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Interface;
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

        private ILoginHistoryRepository _login;
        private IUserRepository _user;
        private IBranchRepository _branch;
        private IRoleRepository _role;
        public BaseService(ILoginHistoryRepository login, IUserRepository user, IBranchRepository branch, IRoleRepository role)
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
            var date = DateTime.UtcNow.ToLong().ToString();
            var model = new LoginHistory
            {
                Id = date,
                UserId = user.Id,
                LoginTime = DateTime.UtcNow.ToLong(),
                LogoutTime = null,
                AgentId = PcUniqueNumber.GetUserAgentInfo,
            };
            return _login.Create(model);
        }

        public string LoginId(int userId)
        {
            return _login.MaxValue(u => u.Id.ToString(), u => u.UserId == userId);
        }

        public LoginHistory LoginTime(int shopId, int userId)
        {
            var date = DateTime.UtcNow.ToLong().ToString();
            var model = new LoginHistory
            {
                Id = date,
                UserId = userId,
                LoginTime = DateTime.UtcNow.ToLong(),
                LogoutTime = null,
                AgentId = PcUniqueNumber.GetUserAgentInfo,
            };
            return _login.Create(model);
        }

        public void LogOutTime(int userId)
        {
            var loginId = _login.MaxValue(u => u.Id.ToString(), u => u.UserId == userId);
            if (!string.IsNullOrEmpty(loginId))
            {
                var model = _login.First(o => o.Id == loginId);
                model.AgentId = PcUniqueNumber.GetUserAgentInfo;
                model.LogoutTime = DateTime.UtcNow.ToLong();
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
            var userCookie = user.GetDictionary(user);
            var cookie = new ManageCookie();
            cookie.RemoveCookie("AzRADMINUSER");
            cookie.SetCookie("AzRADMINUSER", userCookie);

        }

        public AppUserPrincipal AppUser(LoginHistory login)
        {
            var user = _user.Find(u => u.Id == login.UserId);
            var roles = GetAllRoleByUsers(user.Id);
            var applicationRoles = roles as List<ApplicationRole> ?? roles.ToList();
            var branch = _branch.Find(i => i.Id == user.BranchId);
            var role = applicationRoles.FirstOrDefault() ?? new ApplicationRole("UNKNOWN");
            List<int> branches;
            if (branch.ParentId == null)
            {
                branches = _branch.FindAll(s => s.Id == user.BranchId || s.ParentId == user.BranchId)
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
                UserName = user.UserName,
                Name = user.FullName,
                Phone = user.PhoneNumber,
                Email = user.Email,
                ActiveBranchId = user.BranchId,
                BranchId = user.BranchId,
                ParentBranchId = branch.ParentId ?? 0,
                Expaired = 0,
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
