using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace AzR.Admin.Web
{
    public class RecycleConfig
    {
        public static void PingSite()
        {
            using (var refresh = new WebClient())
            {
                try
                {
                    refresh.DownloadString(ConfigurationManager.AppSettings["SiteUrl"]);
                }
                catch (Exception ex)
                {
                    // ex.WriteLog();
                }
            }
        }

        public static void SetupRefreshJob()
        {
            if (HttpContext.Current == null) return;

            //remove a previous job
            Action remove = HttpContext.Current.Cache["Refresh"] as Action;
            if (remove is Action)
            {
                HttpContext.Current.Cache.Remove("Refresh");
                remove.EndInvoke(null);
            }

            //get the worker
            Action work = () =>
            {
                while (true)
                {
                    Thread.Sleep(600000);
                    PingSite();
                }
            };
            work.BeginInvoke(null, null);

            //add this job to the cache
            HttpContext.Current.Cache.Add("Refresh",
                         work,
                         null,
                         Cache.NoAbsoluteExpiration,
                         Cache.NoSlidingExpiration,
                         CacheItemPriority.Normal,
                         (s, o, r) => { SetupRefreshJob(); }
              );
        }
    }
}