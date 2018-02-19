using System.ComponentModel.DataAnnotations;

namespace AzR.Core.ViewModels.Admin
{
    public class UserMenuViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public bool IsActive { get; set; }
        public int MenuId { get; set; }
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }
    }
}
