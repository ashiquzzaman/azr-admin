using AzR.Core.Config;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationUserLogin : IdentityUserLogin<int>, IBaseEntity { }
}