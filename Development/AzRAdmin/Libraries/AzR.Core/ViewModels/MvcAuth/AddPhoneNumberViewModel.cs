using System.ComponentModel.DataAnnotations;

namespace AzR.Core.ViewModels.MvcAuth
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}