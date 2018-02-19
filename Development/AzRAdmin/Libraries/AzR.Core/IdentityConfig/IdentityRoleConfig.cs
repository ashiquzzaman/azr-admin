using System.Data.Entity;
using AzR.Core.AppContexts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AzR.Core.IdentityConfig
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
    public class ApplicationRoleManager : RoleManager<ApplicationRole, int>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, int> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<ApplicationRole, int, ApplicationUserRole>(context.Get<ApplicationDbContext>()));
        }
    }
    /*
     
        var roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

    const string roleName = "Admin";

    //Create Role Admin if it does not exist
    var role = roleManager.FindByName(roleName);
    if (role == null)
    {
        role = new CustomRole();
        role.Id = 1; // this will be integer
        role.Name = roleName;

        var roleresult = roleManager.Create(role);
    } 

     */

}
