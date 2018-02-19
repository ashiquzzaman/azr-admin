using System;
using System.Collections.Generic;
using System.Web;

namespace AzR.Utilities
{
    public class ManageCookie
    {
        public void SetCookie(string cookieName, Dictionary<string, object> cookieItem, int cookieExpireDate = 30)
        {
            var myCookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            foreach (var item in cookieItem)
            {
                myCookie[item.Key] = item.Value.ToString().Md5Encrypt();
            }
            myCookie.Expires = DateTime.Now.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public void RemoveCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Response.Cookies[cookieName];
            if (cookie == null) return;
            cookie.Expires = DateTime.Now.AddDays(-180);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

    }
}