using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AzR.Utilities.Attributes;

namespace AzR.Core.AuditLogs
{
    [IgnoreLog]
    public class LoginHistory
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(128)]
        public string Id { get; set; }
        public long UserId { get; set; }
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
