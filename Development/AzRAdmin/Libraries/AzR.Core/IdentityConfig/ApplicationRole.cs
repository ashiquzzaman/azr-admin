using AzR.Core.Config;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IEntity<int>
    {
        public ApplicationRole() { }

        public ApplicationRole(string name)
        {
            Name = name;
        }
        [Required]
        [StringLength(50)]
        public string RoleCode { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsActive { get; set; }
        public string LoginId { get; set; }
    }
}
