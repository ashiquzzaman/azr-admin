using AzR.Core.IdentityConfig;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzR.Core.Entities
{
    public class UserMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
        public bool FullPermission { get; set; }
        public bool AddPermission { get; set; }
        public bool ViewPermission { get; set; }
        public bool EditPermission { get; set; }
        public bool DeletePermission { get; set; }
        public bool DetailViewPermission { get; set; }
        public bool ReportViewPermission { get; set; }
        public bool IsActive { get; set; }
    }
}
