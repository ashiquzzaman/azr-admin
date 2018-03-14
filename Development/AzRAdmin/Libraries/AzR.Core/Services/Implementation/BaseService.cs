using AzR.Core.Entities;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Utilities;
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
        private IOrganizationRepository _organization;
        private IRoleRepository _role;
        public BaseService(ILoginHistoryRepository login, IUserRepository user, IOrganizationRepository organization, IRoleRepository role)
        {
            _login = login;
            _user = user;
            _organization = organization;
            _role = role;
        }

        #region LOGIN SIGNOUT

        public void LoginTime(string userName)
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
            _login.Create(model);
        }

        public string LoginId(int userId)
        {
            return _login.MaxFunc(u => u.Id.ToString(), u => u.UserId == userId);
        }

        public void LoginTime(int shopId, int userId)
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
            _login.Create(model);
        }

        public void LogOutTime(int userId)
        {
            var loginId = _login.MaxFunc(u => u.Id.ToString(), u => u.UserId == userId);
            if (!string.IsNullOrEmpty(loginId))
            {
                var model = _login.First(o => o.Id == loginId);
                model.AgentId = PcUniqueNumber.GetUserAgentInfo;
                model.LogoutTime = DateTime.UtcNow.ToLong();
                _login.SaveChanges();
            }

        }

        #endregion

        public Organization GetOwner()
        {
            return _organization.FirstOrDefault(i => i.Id == 1);
        }

        public void SetCookie(string userName)
        {
            var userCookie = CmsUser(userName).ToDictionary();
            var cookie = new ManageCookie();
            cookie.RemoveCookie("APPUSER");
            cookie.SetCookie("APPUSER", userCookie);
            LoginTime(userName);
        }
        public IEnumerable<ApplicationRole> GetAllRoleByUsers(int userId)
        {

            var roles = from ur in _user.FindAll(u => u.Id == userId).Include(r => r.Roles).SelectMany(y => y.Roles)
                        join r in _role.All() on ur.RoleId equals r.Id
                        select r;
            return roles.ToList();

        }
        public CmsUserViewModel CmsUser(string userName)
        {
            var user = _user.Find(u => u.UserName == userName);
            var inst = _organization.Find(i => i.Id == user.OrgId);
            var roles = GetAllRoleByUsers(user.Id);
            var model = new CmsUserViewModel
            {
                UserId = user.Id,
                Name = user.FullName,
                Phone = user.PhoneNumber ?? "",
                Email = user.Email,
                Expaired = user.Expired,
                UserImage = user.ImageUrl ?? "/Images/user.png",
                OrgId = user.OrgId,
                OrgName = inst.Name,
                RoleIdList = roles.Select(s => s.Id).ToList(),
                RoleNameList = roles.Select(s => s.Name).ToList()
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
