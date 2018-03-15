using AzR.Utilities.Exentions;
using AzR.Utilities.Helpers;
using AzR.Utilities.Securities;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace AzR.Web.Root.AppStart
{
    public class CookieAuthConfig
    {
        public static void RedirectLogin(HttpCookie cookie)
        {
            if (cookie != null) return;
            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var routeData = urlHelper.RouteCollection.GetRouteData(currentContext);
            if (routeData == null) return;
            //var action = routeData.Values["action"] as string;
            var controller = routeData.Values["controller"] as string;
            if (controller == null || controller.ToLower() == "login") return;
            var baseUrl = GeneralHelper.FullyQualifiedApplicationPath(currentContext.Request) + "UserAuth/login";
            GeneralHelper.Redirect(baseUrl);
        }

        public static void SetAuth(HttpCookie cookie)
        {
            if (cookie != null)
            {
                var newUser = new AppUserPrincipal(cookie.Values.Get(2).Md5Decrypt())
                {
                    Id = cookie.Values.Get(0).Md5Decrypt(),
                    UserId = cookie.Values.Get(1).Md5Decrypt().AsInt(),
                    UserName = cookie.Values.Get(2).Md5Decrypt(),
                    Name = cookie.Values.Get(3).Md5Decrypt(),
                    Phone = cookie.Values.Get(4).Md5Decrypt(),
                    Email = cookie.Values.Get(5).Md5Decrypt(),
                    Expaired = cookie.Values.Get(6).Md5Decrypt().AsInt(),
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
                System.Threading.Thread.CurrentPrincipal = newUser;
            }
        }
    }
}