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

        [StringLength(128)]
        public string DisplayName { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
