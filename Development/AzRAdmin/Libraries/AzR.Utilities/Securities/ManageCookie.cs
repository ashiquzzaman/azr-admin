using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using AzR.Utilities.Exentions;

namespace AzR.Utilities.Securities
{
    public class ManageCookie
    {
        public void SetNonSecureCookie(string cookieName, Dictionary<string, object> cookieItem, int cookieExpireDate = 30)
        {
            var myCookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            foreach (var item in cookieItem)
            {
                myCookie[item.Key] = item.Value.ToString();
            }
            myCookie.Expires = DateTime.UtcNow.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        public void SetCookie(string cookieName, Dictionary<string, object> cookieItem, int cookieExpireDate = 30)
        {
            var myCookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            foreach (var item in cookieItem)
            {
                myCookie[item.Key] = item.Value.ToString().Md5Encrypt();
            }
            myCookie.Expires = DateTime.UtcNow.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        public void UpdateCookie(string cookieName, Dictionary<string, object> cookieItem, int cookieExpireDate = 30)
        {
            var myCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (myCookie == null) return;
            myCookie.Value = null;
            foreach (var item in cookieItem)
            {
                myCookie[item.Key] = item.Value.ToString().Md5Encrypt();
            }
            myCookie.Expires = DateTime.UtcNow.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        public static void SetCookie(Dictionary<string, object> cookieItem, int cookieExpireDate = 30)
        {
            var cookieName = ConfigurationManager.AppSettings["AppName"];
            var myCookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            foreach (var item in cookieItem)
            {
                myCookie[item.Key] = item.Value.ToString().Md5Encrypt();
            }
            myCookie.Expires = DateTime.UtcNow.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        public void RemoveCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Response.Cookies[cookieName];
            if (cookie == null) return;
            cookie.Expires = DateTime.UtcNow.AddDays(-180);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void RemoveCookie()
        {
            var cookieName = ConfigurationManager.AppSettings["AppName"];
            var cookie = HttpContext.Current.Response.Cookies[cookieName];
            if (cookie == null) return;
            cookie.Expires = DateTime.UtcNow.AddDays(-180);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

    }
}