using System.ComponentModel.DataAnnotations;

namespace AzR.Core.ViewModels.Admin
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Branch Id")]
        [Required]
        [StringLength(50)]
        public string OrgCode { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        [MaxLength(256, ErrorMessage = "name can only be {0} characters.")]
        [MinLength(2, ErrorMessage = "name minimum {0} characters.")]
        public string Name { get; set; }

        //[Display(Name = "Logo")]
        //[MaxLength(255, ErrorMessage = "Picture can only be {0} characters.")]
        //[MinLength(3, ErrorMessage = "Picture minimum {0} characters.")]
        //public string LogoUrl { get; set; }

        // public HttpPostedFileBase Picture { get; set; }

        [StringLength(128)]
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [StringLength(128)]
        public string Phone { get; set; }


        [DataType(DataType.MultilineText)]
        [MaxLength(300, ErrorMessage = "Address can only be {0} characters.")]
        [MinLength(3, ErrorMessage = "Address minimum {0} characters.")]
        public string Address { get; set; }

        public long? Created { get; set; }

        public long? Expired { get; set; }

        [Display(Name = "Parent")]
        public int? ParentId { get; set; }

        public bool Active { get; set; }

    }
}
