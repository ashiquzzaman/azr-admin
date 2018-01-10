using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AzR.Core.IdentityConfig;

namespace AzR.Core.Entity
{
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public Organization Parent { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }
        [Required]
        [StringLength(128)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Address { get; set; }
        public long Created { get; set; }
        public long Expired { get; set; }

        public bool Active { get; set; }

        public ICollection<Organization> Children { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

    }
}
