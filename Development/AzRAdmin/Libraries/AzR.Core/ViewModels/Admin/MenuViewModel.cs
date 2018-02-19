using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AzR.Core.Enumerations;
using AzR.Core.HelperModels;

namespace AzR.Core.ViewModels.Admin
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Menu Name")]
        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; }
        [Display(Name = "Menu URL")]
        [StringLength(500)]
        public string Url { get; set; }
        [Display(Name = "Menu Order")]
        public int MenuOrder { get; set; }
        [StringLength(120)]
        public string Icon { get; set; }
        [Display(Name = "Menu Type")]
        public MenuType MenuType { get; set; }
        [Display(Name = "Role Name")]
        public int RoleId { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Parent Menu")]
        public int? ParentId { get; set; }
        [Display(Name = "Parent Menu")]
        public string ParentName { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public IEnumerable<DropDownItem> ParentList { get; set; }
        public IEnumerable<DropDownItem> RoleList { get; set; }
    }
}
