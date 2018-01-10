using System.ComponentModel.DataAnnotations;

namespace AzR.Core.MvcAuth
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}