using AzR.Core.Config;
using System.ComponentModel.DataAnnotations;

namespace AzR.Student.Core.Models
{
    public class Student : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(128)]
        public string Phone { get; set; }

    }
}
