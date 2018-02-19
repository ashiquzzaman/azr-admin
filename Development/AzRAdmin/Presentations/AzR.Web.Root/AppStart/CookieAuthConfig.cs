using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AzR.Core.HelperModels;
using AzR.Utilities;

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
            if (cookie == null) return;
            var newUser = new CmsUserViewModel
            {

                UserId = cookie.Values.Get(0).Md5Decrypt().AsInt(),
                Name = cookie.Values.Get(1).Md5Decrypt(),
                Phone = cookie.Values.Get(2).Md5Decrypt(),
                Email = cookie.Values.Get(3).Md5Decrypt(),
                Expaired = Convert.ToInt64(cookie.Values.Get(4).Md5Decrypt()),
                UserImage = cookie.Values.Get(5).Md5Decrypt(),
                OrgId = cookie.Values.Get(6).Md5Decrypt().AsInt(),
                OrgName = cookie.Values.Get(7).Md5Decrypt(),
                RoleIds = cookie.Values.Get(9).Md5Decrypt(),
                RoleNames = cookie.Values.Get(11).Md5Decrypt()
            };
            HttpContext.Current.Items["APPUSER"] = newUser;
        }
    }
}