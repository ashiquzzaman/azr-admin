using AzR.Core.Config;
using AzR.Core.Enumerations;
using AzR.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzR.Core.AuditLogs
{
    [IgnoreLog]
    public class AuditLog : IBaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(128)]
        public string Id { get; set; }
        [StringLength(128)]
        public string LoginId { get; set; }
        [ForeignKey("LoginId")]
        public virtual LoginHistory LoginHistory { get; set; }
        [StringLength(128)]
        public string KeyFieldId { get; set; }
        public long ActionTime { get; set; }
        [StringLength(256)]
        public string EntityName { get; set; }
        [StringLength(500)]
        public string EntityFullName { get; set; }
        [StringLength(500)]
        public string ActionUrl { get; set; }
        public int BranchId { get; set; }

        public string ValueBefore { get; set; }

        public string ValueAfter { get; set; }

        public string ValueChange { get; set; }
        public ActionType ActionType { get; set; }
        public long UserId { get; set; }
        [StringLength(500)]
        public string ActionUser { get; set; }

        [StringLength(128)]
        public string ActionAgent { get; set; }

    }
}
