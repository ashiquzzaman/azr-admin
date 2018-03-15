using System.ComponentModel.DataAnnotations;

namespace AzR.Core.Config
{
    public abstract class Entity<T> : IEntity<T>
    {
        public virtual T Id { get; set; }
        [Required]
        [StringLength(128)]
        public string LoginId { get; set; }
    }

}
