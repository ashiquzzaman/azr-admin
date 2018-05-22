using AzR.Utilities.Exentions;
using AzR.Utilities.Securities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.WebPages;

namespace AzR.WebFw.AppStart
{
    public class AuthConfig
    {

        public static void ClaimAuth()
        {
            var user = HttpContext.Current.User.Identity;
            if (user.IsAuthenticated)
            {
                //var principal = actionContext.Request.GetRequestContext().Principal as ClaimsPrincipal;
                var claims = ((ClaimsIdentity)HttpContext.Current.User.Identity)
                    .Claims
                    .Select(x => new { Key = x.Type, Value = x.Value })
                    .ToDictionary(t => t.Key, t => t.Value);

                var newUser = AppUser(user.Name, claims);
                HttpContext.Current.Items["AzRADMINUSER"] = newUser;
                Thread.CurrentPrincipal = newUser;
            }

        }
        public static AppUserPrincipal AppUser(string userName, Dictionary<string, string> claims)
        {
            return new AppUserPrincipal(userName)
            {
                Id = claims["Id"],
                UserId = claims["UserId"].AsInt(),
                UserName = claims["UserName"],
                Name = claims["Name"],
                Phone = claims["Phone"],
                Email = claims["Email"],
                Expired = claims["Expired"].As<long>(),
                ActiveBranchId = claims["ActiveBranchId"].AsInt(),
                ParentBranchId = claims["ParentBranchId"].AsInt(),
                ActiveRoleName = claims["ActiveRoleName"],
                RoleNames = claims["RoleNames"],
                ActiveRoleId = claims["ActiveRoleId"].AsInt(),
                RoleIds = claims["RoleIds"],
                PermittedBranchs = claims["PermittedBranchs"],
                BranchId = claims["BranchId"].AsInt(),

            };
        }


    }
}
