using System;
using System.Configuration;
using System.Threading;
using System.Web;
using AzR.Utilities.Exentions;
using AzR.Utilities.Helpers;
using AzR.Utilities.Securities;

namespace AzR.Utilities
{
    public class AppIdentity
    {
        public static AppUserPrincipal AppUser
        {
            get
            {
                return HttpContext.Current != null
                    ? HttpContext.Current.Items[AppName] as AppUserPrincipal
                    : Thread.CurrentPrincipal as AppUserPrincipal;
            }
        }
        public static string AuditId
        {
            get
            {
                return AppInfo + "U" + AppUser.UserId + "T" +
                       DateTime.UtcNow.ToLong() + "E" + GeneralHelper.UtcNowTicks;
            }
        }
        public static string AgentInfo { get { return PcUniqueNumber.GetUserAgentInfo ?? "127.0.0.1"; } }
        public static string AppInfo { get { return ConfigurationManager.AppSettings["AppInfo"]; } }
        public static string AppName { get { return ConfigurationManager.AppSettings["AppName"]; } }
        public static string AppFullName { get { return ConfigurationManager.AppSettings["AppFullName"]; } }
        public static string AppSlogan { get { return ConfigurationManager.AppSettings["AppSlogan"]; } }
        public static string AppType { get { return ConfigurationManager.AppSettings["AppType"]; } }
        public static string AppVersion { get { return ConfigurationManager.AppSettings["AppVersion"]; } }
        public static string AppUrl { get { return ConfigurationManager.AppSettings["AppUrl"]; } }
        public static string AppPath { get { return ConfigurationManager.AppSettings["AppPath"]; } }
        public static string DefaultController { get { return ConfigurationManager.AppSettings["DefaultController"]; } }
    }
}
