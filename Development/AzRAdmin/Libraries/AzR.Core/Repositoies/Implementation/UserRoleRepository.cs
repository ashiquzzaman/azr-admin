using System.Data.Entity;
using AzR.Core.Config;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Interface;

namespace AzR.Core.Repositoies.Implementation
{
    public class UserRoleRepository : Repository<ApplicationUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }
    }
}