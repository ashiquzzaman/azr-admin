using System.ComponentModel.DataAnnotations;

namespace AzR.Core.MvcAuth
{
    public class ExternalLoginConfirmationViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}