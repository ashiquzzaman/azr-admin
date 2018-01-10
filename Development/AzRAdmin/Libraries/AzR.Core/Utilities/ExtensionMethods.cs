using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AzR.Core.Utilities
{
    public static class ExtensionMethods
    {
        public static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(source, null));
        }
        public static string ToFriendlyDateTime(this long value)
        {

            var dateTime = DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var localDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();
            var span = DateTime.UtcNow - localDateTime;

            if (span > TimeSpan.FromHours(24))
            {
                return dateTime.ToString("MMM dd");
            }

            if (span > TimeSpan.FromMinutes(60))
            {
                return string.Format("{0}h", span.Hours);
            }

            return span > TimeSpan.FromSeconds(60) ? string.Format("{0}m ago", span.Minutes) : "Just now";
        }
        public static string GetBaseUrl(this HttpRequestBase request)
        {
            if (request.Url == (Uri)null)
                return string.Empty;
            else
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }
        public static string FriendlyUtcTimestamp(this DateTime dateTime)
        {
            var span = DateTime.UtcNow - dateTime;

            if (span > TimeSpan.FromHours(24))
            {
                return dateTime.ToString("MMM dd");
            }

            if (span > TimeSpan.FromMinutes(60))
            {
                return string.Format("{0}h", span.Hours);
            }

            if (span > TimeSpan.FromSeconds(60))
            {
                return string.Format("{0}m", span.Minutes);
            }

            return "now";

        }
        public static long ToLong(this DateTime dateTime)
        {
            var date = dateTime.ToString("yyyyMMddHHmmss");
            var span = Convert.ToInt64(date);
            return span;
        }
        public static DateTime ToDateTime(this long value)
        {
            var span = DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var localDateTime = DateTime.SpecifyKind(span, DateTimeKind.Utc).ToLocalTime();
            return localDateTime;
        }
        public static void WriteLog(this Exception ex)
        {
            var startupPath =
                new DirectoryInfo(
                    Path.GetDirectoryName(
                        Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent
                    .FullName;
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(st.FrameCount - 1);
            File.AppendAllText(startupPath + "\\ErrorLog.txt",
                DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + ":: " + Path.GetFileName(frame.GetFileName())
                + ":: " + frame.GetMethod().Name + "::" + frame.GetFileLineNumber() + " :: " + ex.Message +
                Environment.NewLine);
        }
        public static string Md5Encrypt(this string toencrypt, string key = "Md.Ashiquzzaman(Rajib)", bool usehashing = true)
        {
            byte[] keyArray;
            if (usehashing)
            {
                using (var hashmd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }
            using (var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            using (var transform = tdes.CreateEncryptor())
            {
                try
                {
                    var toEncryptArray = Encoding.UTF8.GetBytes(toencrypt);
                    var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
        public static string Md5Decrypt(this string todecrypt, string key = "Md.Ashiquzzaman(Rajib)", bool usehashing = true)
        {
            byte[] toEncryptArray;

            try
            {
                toEncryptArray = Convert.FromBase64String(todecrypt.Replace(" ", "+"));
            }
            catch (Exception)
            {
                return string.Empty;
            }

            byte[] keyArray;

            if (usehashing)
            {
                using (var hashmd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }
            using (var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            using (var transform = tdes.CreateDecryptor())
            {
                try
                {
                    var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Encoding.UTF8.GetString(resultArray);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
      
    }
}
