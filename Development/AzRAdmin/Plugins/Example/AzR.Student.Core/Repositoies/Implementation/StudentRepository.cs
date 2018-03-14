using AzR.Core.AppContexts;
using AzR.Student.Core.Repositoies.Interface;
using System.Data.Entity;

namespace AzR.Student.Core.Repositoies.Implementation
{
    public class StudentRepository : Repository<Models.Student>, IStudentRepository
    {
        public StudentRepository(DbContext studentDbContext) : base(studentDbContext)
        {
        }
    }
}
