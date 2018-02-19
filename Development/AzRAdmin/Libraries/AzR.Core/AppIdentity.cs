using System.Configuration;
using System.Threading;
using AzR.Utilities;
using Microsoft.AspNet.Identity;

namespace AzR.Core
{
    public class AppIdentity
    {
        public static string AppUserName
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.Name;
            }
        }
        public static long AppUserId
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.GetUserId<long>();
            }
        }

        public static string AgentInfo { get { return PcUniqueNumber.GetUserAgentInfo; } }
        public static string AppInfo { get { return ConfigurationManager.AppSettings["AppInfo"]; } }
        public static string AppName { get { return ConfigurationManager.AppSettings["AppName"]; } }
        public static string AppType { get { return ConfigurationManager.AppSettings["AppType"]; } }
        public static string AppVersion { get { return ConfigurationManager.AppSettings["AppVersion"]; } }
        public static string AppUrl { get { return ConfigurationManager.AppSettings["AppUrl"]; } }
        public static string AppPath { get { return ConfigurationManager.AppSettings["AppPath"]; } }

    }
}
