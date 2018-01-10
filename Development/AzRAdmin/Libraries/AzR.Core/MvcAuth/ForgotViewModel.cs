using System.ComponentModel.DataAnnotations;

namespace AzR.Core.MvcAuth
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}