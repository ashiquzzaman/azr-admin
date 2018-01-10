using System.Data.Entity;
using System.Linq;
using AzR.Core.Entity;
using AzR.Core.Repository;

namespace AzR.Core.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(DbContext context) : base(context) { }
        public IQueryable<Organization> GetAllActive()
        {
            return FindAll(r => r.Active == true);
        }
        public IQueryable<Organization> GetAllDeactive()
        {
            return FindAll(r => r.Active == false);
        }
        public Organization GetBy(int id)
        {
            return Find(r => r.Id == id && r.Active == true);
        }


        public IQueryable<Organization> TakeBy(int number)
        {
            return FindAll(a => a.Active == true).Take(number);
        }


    }
}
