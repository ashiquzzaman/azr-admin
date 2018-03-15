using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace AzR.Utilities.Helpers
{
    public class GeneralHelper
    {
        private const string ScriptTag = "<script type=\"text/javascript\" language=\"javascript\">{0}</script>";

        public static string StartupPath
        {
            get
            {
                var startupPath = new DirectoryInfo(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent.FullName;
                return startupPath;
            }
        }

        public static void WriteValue(string value, string fileName = "ErrorLog.txt")
        {
            var startupPath =
               new DirectoryInfo(
                   Path.GetDirectoryName(
                       Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent
                   .FullName;
            File.AppendAllText(startupPath + "\\" + fileName, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + "::" + value + Environment.NewLine);
        }
        public static void Alert(string message)
        {
            const string function = "alert('{0}');";
            var log = String.Format((string)GenerateCodeFromFunction(function), message);
            HttpContext.Current.Response.Write(log);
        }
        public static void Redirect(string url)
        {
            const string function = "window.location.href ='{0}';";
            var log = String.Format(GenerateCodeFromFunction(function), url);
            HttpContext.Current.Response.Write(log);
        }
        private static string GenerateCodeFromFunction(string function)
        {
            return String.Format(ScriptTag, function);
        }
        public static string FullyQualifiedApplicationPath(HttpRequestBase httpRequestBase)
        {
            string appPath = String.Empty;

            if (httpRequestBase != null)
            {
                //Formatting the fully qualified website url/name
                appPath = String.Format("{0}://{1}{2}{3}",
                            httpRequestBase.Url.Scheme,
                            httpRequestBase.Url.Host,
                            httpRequestBase.Url.Port == 80 ? String.Empty : ":" + httpRequestBase.Url.Port,
                            httpRequestBase.ApplicationPath);
            }

            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }

            return appPath;
        }
        public string UploadImage(HttpPostedFileBase file, string fileName, string uploadPath)
        {
            var result = "/Images/noimage.png";
            if (file == null || file.ContentLength <= 0) return result;
            var imageMime = Path.GetExtension(file.FileName);
            fileName = fileName + imageMime;
            result = string.Format("{0}/{1}", uploadPath, fileName);
            var path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~{0}", uploadPath)), fileName);
            file.SaveAs(path);
            return result;
        }
        public string GetContentType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
                return string.Empty;

            string contentType = string.Empty;
            switch (fileExtension)
            {
                case "htm":
                case "html":
                    contentType = "text/HTML";
                    break;

                case "txt":
                    contentType = "text/plain";
                    break;

                case "doc":
                case "rtf":
                case "docx":
                    contentType = "Application/msword";
                    break;

                case "xls":
                case "xlsx":
                    contentType = "Application/x-msexcel";
                    break;

                case "jpg":
                case "jpeg":
                    contentType = "image/jpeg";
                    break;

                case "gif":
                    contentType = "image/GIF";
                    break;

                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "png":
                    contentType = "image/png";
                    break;

            }

            return contentType;
        }

        public int GetDictonaryKeyIndex(Dictionary<string, dynamic> dictionary, string key)
        {
            foreach (var t in dictionary.Where(t => dictionary == null))
            {
            }
            return -1;
        }

        public string ConcanetArrayValue(IEnumerable<string> arrayStrings, string separateWith = ":")
        {
            var str = String.Empty;
            if (arrayStrings != null)
            {
                str = arrayStrings.Aggregate(str, (current, s) => current + (s + separateWith));
                str = str.Remove(str.LastIndexOf(separateWith, StringComparison.Ordinal));
            }
            return str;
        }
        public string ConcanetArrayValue(string[] arrayStrings, string separateWith = ":")
        {
            var str = String.Empty;
            if (arrayStrings != null)
            {
                str = arrayStrings.Aggregate(str, (current, s) => current + (s + separateWith));
                str = str.Remove(str.LastIndexOf(separateWith, StringComparison.Ordinal));
            }
            return str;
        }
        public string RandormString(int rang, bool lowerChar = false, bool specialChar = false)
        {
            var rsg = new StringGenerator { UseLowerCaseCharacters = lowerChar, UseSpecialCharacters = specialChar };
            var returnValue = rsg.Generate(rang);
            return returnValue;
        }
        public string DbWhereValue(Dictionary<string, dynamic> whereObjects, string startBracket, string endBracket, string condetion = " AND ")
        {
            var whereValue = String.Empty;

            if (whereObjects != null && whereObjects.Count > 1)
            {
                foreach (var o in whereObjects)
                {
                    string value;
                    if (o.Value.GetType().ToString() == "System.String" || o.Value.GetType().ToString() == "System.DateTime")
                    {
                        value = "'" + o.Value + "'";
                    }
                    else
                    {
                        value = o.Value.ToString();
                    }
                    whereValue = whereValue + (startBracket + o.Key + endBracket + "=" + value + condetion);
                }
                whereValue = whereValue.Remove(whereValue.LastIndexOf(condetion, StringComparison.Ordinal));
            }
            else
            {
                if (whereObjects != null)
                {
                    foreach (var o in whereObjects)
                    {
                        if (o.Value.GetType().ToString() == "System.String" || o.Value.GetType().ToString() == "System.DateTime")
                        {
                            whereValue = startBracket + o.Key + endBracket + "= '" + o.Value + "'";
                        }
                        else
                        {
                            whereValue = startBracket + o.Key + endBracket + "=" + o.Value;
                        }
                    }
                }
            }
            return whereValue;
        }
        public string AgeCalculation(string dateOfBitrh)
        {

            var dtToday = DateTime.Today;
            var toDay = string.Format("{0:dd-MMM-yyyy}", dtToday);
            var rrr = toDay.Substring(0, 4);


            var birthDay = Convert.ToDateTime(dateOfBitrh);
            var strAge = string.Format("{0:dd-MMM-yyyy}", birthDay);
            var dpt = strAge.Substring(0, 4);
            var returnValue = Convert.ToString(Convert.ToInt64(toDay.Substring(0, 4)) - Convert.ToInt64(strAge.Substring(0, 4)));
            return returnValue;
        }
        public bool IsPositive(int number)
        {
            return number > 0;
        }
        public bool IsNegative(int number)
        {
            return number < 0;
        }
        public bool IsZero(int number)
        {
            return number == 0;
        }
        public bool IsAwesome(int number)
        {
            return IsNegative(number) && IsPositive(number) && IsZero(number);
        }
        private static long lastTimeStamp = DateTime.UtcNow.Ticks;
        public static long UtcNowTicks
        {
            get
            {
                long original, newValue;
                do
                {
                    original = lastTimeStamp;
                    long now = DateTime.UtcNow.Ticks;
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange
                             (ref lastTimeStamp, newValue, original) != original);

                return newValue;
            }
        }

    }
}
