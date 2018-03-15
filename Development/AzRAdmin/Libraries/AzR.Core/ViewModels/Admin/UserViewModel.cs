using AzR.Core.HelperModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AzR.Core.ViewModels.Admin
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Employee Id")]
        [Required(ErrorMessage = "Please enter your Employee Id")]
        [MaxLength(50, ErrorMessage = "Employee Id can only be {0} characters.")]
        [MinLength(1, ErrorMessage = "Employee Id minimum {0} characters.")]
        public string EmployeeId { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter your full name")]
        [MaxLength(128, ErrorMessage = "full name can only be {0} characters.")]
        [MinLength(2, ErrorMessage = "full name minimum {0} characters.")]
        public string FullName { get; set; }


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

        [Display(Name = "Branch")]
        public int? OrgId { get; set; }

        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }


        public long? Created { get; set; }
        public bool Active { get; set; }
        [Display(Name = "In Vacation")]
        public bool InVacation { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }
        private List<string> _roleName = new List<string>();
        [Display(Name = "Role")]
        public List<string> RoleNameList
        {
            get { return _roleName; }
            set { _roleName = value; }
        }
        [Display(Name = "Role")]
        public string RoleNames
        {
            get { return string.Join(",", _roleName); }
            set
            {
                _roleName = value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : new List<string> { value };
            }
        }

        public long? Expired { get; set; }


        public List<DropDownItem> RoleList { get; set; }
        public List<DropDownItem> OrgList { get; set; }

    }



}