using AzR.Core.Config;
using AzR.Core.IdentityConfig;
using AzR.Utilities.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzR.Core.AuditLogs
{
    [IgnoreLog]
    public class LoginHistory : IBaseEntity
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(128)]
        public string Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int UserBranchId { get; set; }
        public long LoginTime { get; set; }
        public long? LogoutTime { get; set; }
        public long ModifiedBy { get; set; }
        [Required]
        [StringLength(100)]
        [ScaffoldColumn(false)]
        public string AgentId { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
    }
}
