using System.Data.Entity;
using AzR.Core.Entity;
using AzR.Core.Repository;

namespace AzR.Core.Repositories
{
    public class LoginHistoryRepository : Repository<LoginHistory>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(DbContext context) : base(context)
        {
        }
    }
}
