using System.Data.Entity;

namespace AzR.Student.Core
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext() : base("StudentConnection")//base(Util.GetTheConnectionString())
        {
        }
        public DbSet<Models.Student> Students { get; set; }

    }
}
