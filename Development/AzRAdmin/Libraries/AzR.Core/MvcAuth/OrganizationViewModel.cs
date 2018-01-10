using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using AzR.Core.ModelConfig;

namespace AzR.Core.MvcAuth
{
    public class OrganizationViewModel
    {
        public int? Id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        [MaxLength(256, ErrorMessage = "name can only be {0} characters.")]
        [MinLength(2, ErrorMessage = "name minimum {0} characters.")]
        public string Name { get; set; }


        [Display(Name = "Logo")]
        [MaxLength(255, ErrorMessage = "Picture can only be {0} characters.")]
        [MinLength(3, ErrorMessage = "Picture minimum {0} characters.")]
        public string LogoUrl { get; set; }

        public HttpPostedFileBase Picture { get; set; }

        [Display(Name = "Banner")]
        //[Required(ErrorMessage = "Please enter your Banner")]
        [MaxLength(255, ErrorMessage = "Banner can only be {0} characters.")]
        [MinLength(3, ErrorMessage = "Banner minimum {0} characters.")]
        public string BannerUrl { get; set; }

        public HttpPostedFileBase Banner { get; set; }

        [StringLength(128)]
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [StringLength(128)]
        public string Phone { get; set; }

        [Display(Name = "Established")]
        public long? Established { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Established")]
        public DateTime? EstablishedDate { get; set; }


        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [MaxLength(500, ErrorMessage = "Biography can only be {0} characters.")]
        [MinLength(3, ErrorMessage = "Biography minimum {0} characters.")]
        public string About { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [MaxLength(300, ErrorMessage = "Address can only be {0} characters.")]
        [MinLength(3, ErrorMessage = "Address minimum {0} characters.")]
        public string Address { get; set; }


        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Contact { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [StringLength(250)]
        public string Mission { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [StringLength(250)]
        public string Vision { get; set; }

        public long? Created { get; set; }

        [StringLength(128)]
        public string Latitude { get; set; }

        [StringLength(128)]
        public string Longitude { get; set; }

        [Display(Name = "Type")]
        [StringLength(50)]
        public string Types { get; set; }

        public long? Expired { get; set; }


        public string Others { get; set; }


        [Display(Name = "Parent")]
        public int? ParentId { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Parent")]
        public string ParentText { get; set; }

        [Display(Name = "VAT")]
        [StringLength(50)]
        public string VatRegNo { get; set; }
        [Display(Name = "TIN")]
        [StringLength(50)]
        public string Tin { get; set; }
        [Display(Name = "Registration No")]
        [StringLength(50)]
        public string RegistrationNo { get; set; }
        public IEnumerable<SelectListItem> ParentList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get { return new SelectList(DropDownValue.InstituteList); } }
    }
}
