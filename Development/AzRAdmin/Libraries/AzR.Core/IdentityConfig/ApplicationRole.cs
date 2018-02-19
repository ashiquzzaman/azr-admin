using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
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
    }
}
