using AzR.Utilities.Exentions;
using AzR.Utilities.Securities;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.WebPages;

namespace AzR.WebFw.AppStart
{
    public class AuthConfig
    {
        public static void Initialize()
        {
            ClaimAuth();
            //if (HttpContext.Current.Request.Path.Contains("api") && HttpContext.Current.Request.Cookies["AzRADMINUSER"] == null)
            //{
            //    ClaimAuth();
            //}
            //else
            //{
            //    CookieAuth();
            //}
        }

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

                var newUser = new AppUserPrincipal(user.Name)
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
                HttpContext.Current.Items["AzRADMINUSER"] = newUser;
                Thread.CurrentPrincipal = newUser;
            }


        }

        public static void CookieAuth()
        {
            var request = HttpContext.Current.Request;
            var cookie = request.Cookies["AzRADMINUSER"];

            if (HttpContext.Current.User.Identity.IsAuthenticated && cookie != null)
            {
                var newUser = new AppUserPrincipal(cookie.Values.Get(2).Md5Decrypt())
                {
                    Id = cookie.Values.Get(0).Md5Decrypt(),
                    UserId = cookie.Values.Get(1).Md5Decrypt().AsInt(),
                    UserName = cookie.Values.Get(2).Md5Decrypt(),
                    Name = cookie.Values.Get(3).Md5Decrypt(),
                    Phone = cookie.Values.Get(4).Md5Decrypt(),
                    Email = cookie.Values.Get(5).Md5Decrypt(),
                    Expired = cookie.Values.Get(6).Md5Decrypt().AsInt(),
                    ActiveBranchId = cookie.Values.Get(7).Md5Decrypt().AsInt(),
                    ParentBranchId = cookie.Values.Get(8).Md5Decrypt().AsInt(),
                    ActiveRoleName = cookie.Values.Get(9).Md5Decrypt(),
                    RoleNames = cookie.Values.Get(10).Md5Decrypt(),
                    ActiveRoleId = cookie.Values.Get(11).Md5Decrypt().AsInt(),
                    RoleIds = cookie.Values.Get(12).Md5Decrypt(),
                    PermittedBranchs = cookie.Values.Get(13).Md5Decrypt(),
                    BranchId = cookie.Values.Get(14).Md5Decrypt().AsInt(),

                };
                HttpContext.Current.Items["AzRADMINUSER"] = newUser;
                Thread.CurrentPrincipal = newUser;
            }
        }

    }
}
