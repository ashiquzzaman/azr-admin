using AzR.Core.Config;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationUserRole : IdentityUserRole<int>, IBaseEntity
    {

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
    }
}