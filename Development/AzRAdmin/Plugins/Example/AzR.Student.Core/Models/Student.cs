using System.ComponentModel.DataAnnotations;

namespace AzR.Student.Core.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
    }
}
