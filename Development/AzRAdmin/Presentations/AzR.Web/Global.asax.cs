﻿using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AzR.WebFw.AppStart;
using AzR.WebFw.Caching;

namespace AzR.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {

            var cookie = Request.Cookies["AzRADMINUSER"];
            CookieAuthConfig.SetAuth(cookie);
        }
        protected void Application_Start()
        {
            RecycleConfig.SetupRefreshJob();
            ViewEngines.Engines.Clear();
            var ve = new RazorViewEngine { FileExtensions = new[] { "cshtml" } };
            ve.ViewLocationCache = new TwoLevelViewCache(ve.ViewLocationCache);
            ViewEngines.Engines.Add(ve);
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
