using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AzR.Core.Enumerations;
using AzR.Core.IdentityConfig;

namespace AzR.Core.Entities
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; }
        [Required]
        [StringLength(500)]
        public string Url { get; set; }
        [Required]
        public int MenuOrder { get; set; }
        [StringLength(120)]
        public string Icon { get; set; }
        public MenuType MenuType { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public ApplicationRole Role { get; set; }
        public int? ParentId { get; set; }
        public Menu Parent { get; set; }
        public IList<Menu> Children { get; set; }
        public bool IsActive { get; set; }
        public IList<UserPrivilege> Permissions { get; set; }

    }
}
