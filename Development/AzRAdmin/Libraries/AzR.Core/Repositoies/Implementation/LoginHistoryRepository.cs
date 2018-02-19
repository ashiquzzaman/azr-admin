using System.Data.Entity;
using AzR.Core.AppContexts;
using AzR.Core.Entities;
using AzR.Core.Repositoies.Interface;

namespace AzR.Core.Repositoies.Implementation
{
    public class LoginHistoryRepository : Repository<LoginHistory>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(DbContext context) : base(context)
        {
        }
    }
}
