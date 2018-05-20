using AzR.Core.Config;
using AzR.Core.IdentityConfig;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzR.Core.Entities
{
    public class Branch : AuditableEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string BranchCode { get; set; }
        public int? ParentId { get; set; }
        public virtual Branch Parent { get; set; }
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
        public bool IsBranch { get; set; }

        public long Created { get; set; }
        public long Expired { get; set; }

        public virtual IList<Branch> Children { get; set; }
        public IList<ApplicationUser> Users { get; set; }

    }
}
