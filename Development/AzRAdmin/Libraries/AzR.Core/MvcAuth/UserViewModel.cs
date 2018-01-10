using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AzR.Core.MvcAuth
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(128, ErrorMessage = "first name can only be {0} characters.")]
        [MinLength(2, ErrorMessage = "first name minimum {0} characters.")]
        public string Name { get; set; }

        [Display(Name = "Username")]
        [StringLength(128)]
        public string UniqueName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        [Display(Name = "Picture")]
        public string ImageUrl { get; set; }
        public string Biography { get; set; }

        [StringLength(128)]
        public string Latitude { get; set; }
        [StringLength(128)]
        public string Longitude { get; set; }
        public string Others { get; set; }

        [Display(Name = "Organization")]
        public int OrgId { get; set; }

        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }


        [Display(Name = "Type")]
        [StringLength(50)]
        public string Types { get; set; }

        public long? Created { get; set; }
        public bool Active { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Display(Name = "Role")]
        public string SelectedRole { get; set; }
        public long? Expired { get; set; }


        public List<SelectListItem> RoleList { get; set; }
        public List<SelectListItem> OrgList { get; set; }
        public List<SelectListItem> InstituteList { get; set; }
    }



}