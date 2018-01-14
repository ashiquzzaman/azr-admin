using AzR.Core.ModelConfig;
using AzR.Core.Utilities;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace AzR.Admin.Web
{
    public class CookieAuthConfig
    {
        public static void RedirectLogin(HttpCookie cookie)
        {
            if (cookie == null)
            {
                HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var routeData = urlHelper.RouteCollection.GetRouteData(currentContext);
                if (routeData != null)
                {
                    //var action = routeData.Values["action"] as string;
                    var controller = routeData.Values["controller"] as string;
                    if (controller != null && controller.ToLower() != "login")
                    {
                        var baseUrl = GeneralHelper.FullyQualifiedApplicationPath(currentContext.Request) + "Account/login";
                        GeneralHelper.Redirect(baseUrl);
                    }
                }
            }
        }

        public static void SetAuth(HttpCookie cookie)
        {
            if (cookie != null)
            {
                var newUser = new CmsUserViewModel
                {
                    Name = cookie.Values.Get(0).Md5Decrypt(),
                    Phone = cookie.Values.Get(1).Md5Decrypt(),
                    Email = cookie.Values.Get(2).Md5Decrypt(),
                    Types = cookie.Values.Get(3).Md5Decrypt(),
                    Expaired = Convert.ToInt64(cookie.Values.Get(4).Md5Decrypt()),
                    UserImage = cookie.Values.Get(5).Md5Decrypt(),
                    OrgId = cookie.Values.Get(6).Md5Decrypt().AsInt(),
                    OrgName = cookie.Values.Get(7).Md5Decrypt(),
                };
                HttpContext.Current.Items["APPUSER"] = newUser;
            }
        }
    }
}