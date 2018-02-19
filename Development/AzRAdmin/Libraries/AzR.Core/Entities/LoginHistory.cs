using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Entities
{
    public class LoginHistory
    {
        [Key]
        [StringLength(128)]
        public string Id { get; set; }
        public long LoginTime { get; set; }
        public long? LogoutTime { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string AgentId { get; set; }
    }
}
