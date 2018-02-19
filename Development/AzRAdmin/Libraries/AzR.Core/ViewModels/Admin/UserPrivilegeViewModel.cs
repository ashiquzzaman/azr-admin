using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AzR.Core.Enumerations;

namespace AzR.Core.ViewModels.Admin
{
    public class UserPrivilegeViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public int MenuId { get; set; }
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }
        public MenuType MenuType { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool FullPermission { get; set; }
        public bool AddPermission { get; set; }
        public bool ViewPermission { get; set; }
        public bool EditPermission { get; set; }
        public bool DeletePermission { get; set; }
        public bool DetailViewPermission { get; set; }
        public bool ReportViewPermission { get; set; }
        public List<UserPrivilegeViewModel> Children { get; set; }

    }
}
