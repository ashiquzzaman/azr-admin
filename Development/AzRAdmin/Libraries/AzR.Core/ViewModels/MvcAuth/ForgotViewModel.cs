using System.ComponentModel.DataAnnotations;

namespace AzR.Core.ViewModels.MvcAuth
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}