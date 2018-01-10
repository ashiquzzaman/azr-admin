using System;
using AzR.Core.Entity;
using AzR.Core.ModelConfig;
using AzR.Core.Repositories;
using AzR.Core.Utilities;

namespace AzR.Core.Business
{
    public class BaseService : IBaseService
    {

        private ILoginHistoryRepository _login;
        private IUserRepository _user;
        private IOrganizationRepository _organization;

        public BaseService(ILoginHistoryRepository login, IUserRepository user, IOrganizationRepository organization)
        {
            _login = login;
            _user = user;
            _organization = organization;
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

        public CmsUserViewModel CmsUser(string userName)
        {
            var user = _user.Find(u => u.UserName == userName);
            var inst = _organization.Find(i => i.Id == user.OrgId);
            var model = new CmsUserViewModel
            {
                Name = user.Name,
                Phone = user.PhoneNumber ?? "",
                Email = user.Email,
                Types = (user.Types ?? "").ToUpper(),
                Expaired = user.Expired,
                UserImage = user.ImageUrl ?? "/Images/user.png",
                OrgId = user.OrgId,
                OrgName = inst.Name
            };
            return model;
        }

        public bool IsActive(string userName)
        {
            return _user.IsExist(p => p.Active && (p.Email == userName || p.UserName == userName));
        }
    }
}
