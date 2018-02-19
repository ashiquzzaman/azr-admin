using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using AzR.Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [StringLength(256)]
        public string FullName { get; set; }

        public int OrgId { get; set; }

        [ForeignKey("OrgId")]
        public Organization Organization { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        public long Created { get; set; }

        public long Expired { get; set; }
        public bool InVacation { get; set; }

        public bool IsActive { get; set; }

        [StringLength(100)]
        public string AgentId { get; set; }

        public long Modified { get; set; }
        public IList<UserPrivilege> Permissions { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}