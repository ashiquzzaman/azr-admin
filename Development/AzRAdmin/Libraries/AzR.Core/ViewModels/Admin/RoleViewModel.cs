using System.ComponentModel.DataAnnotations;

namespace AzR.Core.ViewModels.Admin
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Role Id")]
        [Required]
        [StringLength(50)]
        public string RoleCode { get; set; }
        [Required]
        [RegularExpression(@"^[0-9a-zA-Z_][0-9a-zA-Z \-/_.']{1,150}$", ErrorMessage = "Only alphabets, numbers and [-/_.'] special characters are allowed.")]
        [Display(Name = "Role Name")]
        [StringLength(128)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(250)]
        public string Description { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
