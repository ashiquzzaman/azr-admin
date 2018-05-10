using AzR.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Config
{
    [IgnoreEntity]
    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        protected AuditableEntity()
        {
            CreatedBranch = 0;
            ModifiedBy = 0;
            ModifiedTime = 0;
            IsActive = true;
            LoginId = "127.0.0.1";
            CreatedBy = 0;
            CreatedTime = 0;
        }
        [IgnoreUpdate]
        [ScaffoldColumn(false)]
        public int CreatedBranch { get; set; }
        [IgnoreUpdate]
        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }
        [IgnoreUpdate]
        [ScaffoldColumn(false)]
        public long CreatedTime { get; set; }
        [ScaffoldColumn(false)]
        public long ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }
        [ScaffoldColumn(false)]
        public long ModifiedTime { get; set; }
    }

}
