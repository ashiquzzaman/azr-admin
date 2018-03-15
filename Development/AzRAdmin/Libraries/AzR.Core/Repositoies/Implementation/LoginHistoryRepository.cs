using AzR.Core.AuditLogs;
using AzR.Core.Config;
using AzR.Core.Repositoies.Interface;
using System.Data.Entity;

namespace AzR.Core.Repositoies.Implementation
{
    public class LoginHistoryRepository : Repository<LoginHistory>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(DbContext context) : base(context)
        {
        }
    }
}
