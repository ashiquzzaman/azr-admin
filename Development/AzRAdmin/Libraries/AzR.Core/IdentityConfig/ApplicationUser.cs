using AzR.Core.AuditLogs;
using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Utilities.Exentions;
using AzR.Utilities.Helpers;
using AzR.Utilities.Securities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IEntity<int>
    {
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [StringLength(256)]
        public string FullName { get; set; }

        public int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        public long Created { get; set; }

        public long Expired { get; set; }
        public bool InVacation { get; set; }

        public bool IsActive { get; set; }
        [StringLength(100)]
        public string LoginId { get; set; }

        public long Modified { get; set; }
        public IList<UserMenu> Permissions { get; set; }
        public virtual IList<LoginHistory> LoginHistories { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            var claims = AppUser(CreateLogin())
                        .GetByName()
                        .Select(item => new Claim(item.Key, item.Value));

            userIdentity.AddClaims(claims);

            return userIdentity;
        }

        public AppUserPrincipal CurrentAppUser()
        {
            return AppUser(AppIdentity.AppUser.Id);
        }

        public AppUserPrincipal AppUser()
        {
            var login =
                LoginHistories.FirstOrDefault(l => l.LogoutTime == null && l.AgentId == AppIdentity.AgentInfo);
            var loginId = login != null ? login.Id : CreateLogin();
            return AppUser(this, loginId);
        }

        public AppUserPrincipal AppUser(string loginId)
        {
            return AppUser(this, loginId);
        }

        public AppUserPrincipal AppUser(ApplicationUser user, string loginId)
        {
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
                Id = loginId,
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

        public string CreateLogin()
        {
            string id;
            var user = this;

            using (var context = ApplicationDbContext.Create())
            {
                var loginHistory = context.Set<LoginHistory>();

                var existLogin = loginHistory.FirstOrDefault(l =>
                    l.LogoutTime == null && l.AgentId == AppIdentity.AgentInfo && l.UserId == user.Id);

                if (existLogin != null)
                {
                    return existLogin.Id;
                }

                id = AppIdentity.AppInfo + "U" + this.Id + "T" + DateTime.UtcNow.ToLong() + "E" +
                         GeneralHelper.UtcNowTicks;
                var login = new LoginHistory
                {
                    Id = id,
                    UserId = user.Id,
                    LoginTime = DateTime.UtcNow.ToLong(),
                    LogoutTime = null,
                    AgentId = AppIdentity.AgentInfo,
                    ModifiedBy = user.Id,
                    UserBranchId = user.BranchId
                };
                loginHistory.Add(login);
                context.SaveChanges();
            }

            return id;
        }

        public bool CreateLogOut(string loginId)
        {
            if (string.IsNullOrWhiteSpace(loginId))
            {
                return false;
            }
            using (var context = ApplicationDbContext.Create())
            {

                var login = context.Set<LoginHistory>();
                var model = login.First(o => o.Id == loginId);
                model.AgentId = PcUniqueNumber.GetUserAgentInfo;
                model.LogoutTime = DateTime.UtcNow.ToLong();
                model.ModifiedBy = this.Id;
                context.SaveChanges();
                return true;
            }



        }

    }
}