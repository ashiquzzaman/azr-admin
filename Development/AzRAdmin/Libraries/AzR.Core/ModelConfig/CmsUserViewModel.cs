using System.ComponentModel.DataAnnotations;

namespace AzR.Core.ModelConfig
{
    public class CmsUserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Types { get; set; }
        public long Expaired { get; set; }
        public string UserImage { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }

    }
}
