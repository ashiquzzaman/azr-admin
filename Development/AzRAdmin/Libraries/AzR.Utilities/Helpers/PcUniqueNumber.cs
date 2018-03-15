using System;
using System.Collections.Generic;
using System.Web;
using AzR.Utilities.CultureHelpers;
using AzR.Utilities.Exentions;

namespace AzR.Utilities.Helpers
{
    public static class PcUniqueNumber
    {
        public static string GetUserIp
        {
            get
            {
                try
                {
                    string strIp;
                    var httpReq = HttpContext.Current.Request;
                    if (httpReq.ServerVariables["HTTP_CLIENT_IP"] != null)
                    {
                        strIp = httpReq.ServerVariables["HTTP_CLIENT_IP"];
                    }
                    else if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        strIp = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    }
                    else if (httpReq.UserHostAddress == "::1" || httpReq.UserHostAddress == "localhost")
                    {
                        strIp = "127.0.0.1";
                    }
                    else if (httpReq.UserHostAddress != "::1" || httpReq.UserHostAddress != "localhost")
                    {
                        strIp = httpReq.UserHostAddress;
                    }
                    else
                    {
                        strIp = "127.0.0.1";
                    }
                    return strIp;
                }
                catch (Exception ex)
                {
                    ex.WriteLog();
                    return "127.0.0.1";
                }
            }
        }

        public static string GetUserPc
        {
            get
            {
                var osList = new Dictionary<string, string>
            {
                {"Windows NT 10.0", "Windows 10"},
                {"Windows NT 6.3", "Windows 8.1"},
                {"Windows NT 6.2", "Windows 8"},
                {"Windows NT 6.1", "Windows 7"},
                {"Windows NT 6.0", "Windows Vista"},
                {"Windows NT 5.2", "Windows Server 2003"},
                {"Windows NT 5.1", "Windows XP"},
                {"Windows NT 5.0", "Windows 2000"}
            };

                var userAgentText = HttpContext.Current.Request.UserAgent;

                if (userAgentText == null) return HttpContext.Current.Request.Browser.Platform;
                var startPoint = userAgentText.IndexOf('(') + 1;
                var endPoint = userAgentText.IndexOf(';');

                var osVersion = userAgentText.Substring(startPoint, (endPoint - startPoint));
                var friendlyOsName = osList[osVersion];
                return friendlyOsName ?? osVersion;
            }
        }
        public static string GetUserPlatform(HttpRequest request)
        {
            var ua = request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "Web");
        }

        public static string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }
        public static string GetUserAgentInfo
        {
            get
            {
                try
                {
                    var userBrowser = HttpContext.Current.Request.Browser;
                    var platform = HttpContext.Current.Request.UserAgent;
                    var pf = platform != null && platform.Contains("Mobile") ? " Mobile " : "Web";

                    var cultureName = HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length > 0 ?
                        HttpContext.Current.Request.UserLanguages[0] : null;


                    var result = "OS:" + GetUserPlatform(HttpContext.Current.Request) +
                                 "|PF:" + pf +
                                 "|BN:" + userBrowser.Browser +
                                 "|BV:" + userBrowser.Version +
                                 "|CL:" + CultureHelper.GetImplementedCulture(cultureName) +
                                 "|IP:" + GetUserIp;
                    return result;
                }
                catch (Exception ex)
                {
                    ex.WriteLog();
                    return "127.0.0.1";
                }
            }
        }
    }
}

