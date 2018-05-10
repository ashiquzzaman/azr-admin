using AzR.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Config
{
    [IgnoreEntity]
    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        public virtual T Id { get; set; }
        [Required]
        [StringLength(128)]
        public string LoginId { get; set; }
    }

}
