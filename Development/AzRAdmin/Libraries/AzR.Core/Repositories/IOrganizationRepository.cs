using System.Linq;
using AzR.Core.Entity;
using AzR.Core.Repository;

namespace AzR.Core.Repositories
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        IQueryable<Organization> GetAllActive();
        IQueryable<Organization> GetAllDeactive();
        Organization GetBy(int id);
        IQueryable<Organization> TakeBy(int number);

    }
}
