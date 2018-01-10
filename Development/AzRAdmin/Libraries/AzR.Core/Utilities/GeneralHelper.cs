using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;

namespace AzR.Core.Utilities
{
    public static class GeneralHelper
    {
        /// <summary>
        /// Retrieves a setting from the AppSettings collection
        /// </summary>
        /// <param name="key">Key name</param>
        /// <param name="defaultValue">If the key is not found, this value is returned</param>
        public static T GetConfigValue<T>(string key, T defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }

        /// <summary>
        /// Retrieves a settings from the AppSettings collection. If the key is not found, an exception is thrown
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetConfigValue<T>(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                throw new ArgumentException("The key was not found in the configuration file");
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }

        /// <summary>
        /// Retrieves a section from the application's configuration file. If the section is not found, an exception is thrown.
        /// </summary>
        public static T GetSection<T>(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
            {
                throw new ArgumentException("The section was not found in the configuration file");
            }

            return (T)section;
        }
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string ObjectToString(object NewValue)
        {
            string strNew = String.Empty;

            PropertyInfo[] aPropertyInfo = null;
            if ((NewValue != null))
            {
                aPropertyInfo = NewValue.GetType().GetProperties();

                foreach (PropertyInfo aProperty in aPropertyInfo)
                {
                    if (String.IsNullOrEmpty(strNew))
                        strNew = aProperty.Name + " (" + aProperty.PropertyType.ToString() + ") " + ": " + aProperty.GetValue(NewValue, null);
                    else
                    {
                        strNew = strNew + "; " + aProperty.Name + " (" + aProperty.PropertyType.ToString() + ") " + ": " + aProperty.GetValue(NewValue, null);
                    }
                }
            }
            return strNew;
        }

        public static string ObjectToStringWithoutPropertyType(object NewValue)
        {
            string strNew = String.Empty;

            PropertyInfo[] aPropertyInfo = null;
            if ((NewValue != null))
            {
                aPropertyInfo = NewValue.GetType().GetProperties();

                foreach (PropertyInfo aProperty in aPropertyInfo)
                {
                    var pName = aProperty.Name;
                    var type = aProperty.PropertyType.ToString();
                    var dvalue = aProperty.GetValue(NewValue, null);
                    dynamic value = "";
                    if (dvalue != null)
                    {
                        value = dvalue;
                    }

                    if (pName == "FristName")
                    {
                        pName = "FirstName";
                    }

                    if (String.IsNullOrEmpty(strNew))
                        strNew = pName + ": " + value;
                    else
                    {

                        if (value.ToString() != "" && (pName != "CurrentPassword") && (pName != "UserPassward") && (pName != "ConfirmPassword") && (pName != "Password") && (pName != "Passward") &&
                                   (!type.Contains("VelocityFinCrime.Entity")) && (!type.Contains("System.Collections.Generic")) && (!type.Contains("System.String[]")))
                        {
                            strNew = strNew + "; " + pName + ": " + value;
                        }
                    }
                }
            }
            return strNew;
        }

        public static string ObjectToStringWithoutEmptyProperty(object NewValue, string listype)
        {
            string strNew = String.Empty;
            PropertyInfo[] aPropertyInfo = null;
            if ((NewValue != null))
            {
                aPropertyInfo = NewValue.GetType().GetProperties();
                foreach (PropertyInfo aProperty in aPropertyInfo)
                {
                    var aData = aProperty.GetValue(NewValue, null).ToString().ToLower();
                    var pName = aProperty.Name.ToUpper();
                    var Type = aProperty.PropertyType.ToString();
                    if (String.IsNullOrEmpty(strNew))
                    {
                        if (aData.Trim() != "" && (pName != "INDIVIDUAL_DOCUMENT") && (!pName.Contains("ALIAS_QUALITY")) &&
                                  (pName != "FIRST_NAME") && (pName != "SECOND_NAME") && (pName != "WHOLENAME") &&
                                  (pName != "CUSTOMERNAME") && (pName != "FIRSTNAME") && (pName != "MIDDLENAME") && (pName != "LASTNAME") && (!pName.Contains("NAME")) &&
                                  (pName != "ID") && (pName != "UID") && (pName != "DATAID") && (pName != "LIST_TYPE") &&
                                  (pName != "AKALIST") && (pName != "VERSIONNUM") && (pName != "REFERENCE_NUMBER") &&
                                  (pName != "INDIVIDUAL_DATE_OF_BIRTH") && (pName != "DESIGNATION") && (pName != "UN_LIST_TYPE") &&
                                  (pName != "TITLE") && (pName != "DATEOFBIRTH") && (pName != "ADDRESSLIST_ADDRESS_POSTALCODE") &&
                                  (!pName.Contains("IDLIST")) && (!pName.Contains("PROGRAMLIST")) &&
                                  (pName != "SORT_KEY") && (pName != "SORT_KEY_LAST_MOD") && (pName != "LAST_DAY_UPDATED") &&
                                  (!Type.Contains("System.DateTime")) && (!Type.Contains("System.Boolean")))
                        {
                            strNew = SetValue(pName) + ": " + aData;
                        }
                    }
                    else
                    {
                        if (aData.Trim() != "" && (pName != "INDIVIDUAL_DOCUMENT") && (!pName.Contains("ALIAS_QUALITY")) &&
             (pName != "FIRST_NAME") && (pName != "SECOND_NAME") && (pName != "WHOLENAME") &&
             (pName != "CUSTOMERNAME") && (pName != "FIRSTNAME") && (pName != "MIDDLENAME") && (pName != "LASTNAME") && (!pName.Contains("NAME")) &&
             (pName != "ID") && (pName != "UID") && (pName != "DATAID") && (pName != "LIST_TYPE") &&
             (pName != "AKALIST") && (pName != "VERSIONNUM") && (pName != "REFERENCE_NUMBER") &&
             (pName != "INDIVIDUAL_DATE_OF_BIRTH") && (pName != "DESIGNATION") && (pName != "UN_LIST_TYPE") &&
             (pName != "TITLE") && (pName != "DATEOFBIRTH") && (pName != "ADDRESSLIST_ADDRESS_POSTALCODE") &&
             (!pName.Contains("IDLIST")) && (!pName.Contains("PROGRAMLIST")) &&
             (pName != "SORT_KEY") && (pName != "SORT_KEY_LAST_MOD") && (pName != "LAST_DAY_UPDATED") &&
             (!Type.Contains("System.DateTime")) && (!Type.Contains("System.Boolean")))
                        {
                            strNew = strNew + Environment.NewLine + SetValue(pName) + ":: " + aData;
                        }
                    }
                }

            }
            return strNew;
        }


        private static string SetValue(string aProperty)
        {

            aProperty = aProperty.Replace("SDNTYPE", "CUSTOMER TYPE");
            aProperty = aProperty.Replace("SECOND_NAME", "LAST_NAME");
            aProperty = aProperty.Replace("COMMENTS1", "COMMENTS");
            aProperty = aProperty.Replace("INDIVIDUAL_ALIAS_", "");
            aProperty = aProperty.Replace("INDIVIDUAL_", "");
            aProperty = aProperty.Replace("ENTITY_", "");
            aProperty = aProperty.Replace("IDLIST_", "");
            aProperty = aProperty.Replace("ADDRESSLIST_ADDRESS", "");
            aProperty = aProperty.Replace("VESSELINFO_", "");
            aProperty = aProperty.Replace("_", " ");
            return aProperty.ToUpper();
        }

        public static List<T> SetDefaultValue<T>(List<T> aList)
        {

            foreach (var akycinfo in aList)
            {
                foreach (var prop in akycinfo.GetType().GetProperties())
                {
                    var PropertyType = prop.PropertyType.FullName.ToString();
                    var PropertyName = prop.Name;
                    var PropertyValue = prop.GetValue(akycinfo, null);


                    if (PropertyValue == null)
                    {
                        if (PropertyType.Contains("System.String") && PropertyType != "System.String[]")
                            prop.SetValue(akycinfo, "");
                        else if (PropertyType.Contains("System.Boolean") && PropertyType != "System.Boolean[]")
                            prop.SetValue(akycinfo, false);
                        else if (PropertyType.Contains("System.Int16") && PropertyType != "System.Int16[]")
                            prop.SetValue(akycinfo, 0);
                        else if (PropertyType.Contains("System.Int32") && PropertyType != "System.Int32[]")
                            prop.SetValue(akycinfo, 0);
                        else if (PropertyType.Contains("System.Int64") && PropertyType != "System.Int64[]")
                            prop.SetValue(akycinfo, 0L);
                        else if (PropertyType.Contains("System.DateTime") && PropertyType != "System.DateTime[]")
                        {
                            DateTime dt = Convert.ToDateTime("1900/01/01");
                            prop.SetValue(akycinfo, dt);
                        }
                        else if (PropertyType.Contains("System.Double") && PropertyType != "System.Double[]")
                            prop.SetValue(akycinfo, 0.0d);
                        else if (PropertyType.Contains("System.Decimal") && PropertyType != "System.Decimal[]")
                            prop.SetValue(akycinfo, 0.0M);

                    }
                }
            }
            return aList;
        }

        public static T SetDefaultValue<T>(T obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                var PropertyType = prop.PropertyType.FullName;
                var PropertyName = prop.Name;
                var PropertyValue = prop.GetValue(obj, null);


                if (PropertyValue == null)
                {
                    if (PropertyType.Contains("System.String"))
                        prop.SetValue(obj, "");
                    else if (PropertyType.Contains("System.Boolean"))
                        prop.SetValue(obj, false);
                    else if (PropertyType.Contains("System.Int16"))
                        prop.SetValue(obj, 0);
                    else if (PropertyType.Contains("System.Int32"))
                        prop.SetValue(obj, 0);
                    else if (PropertyType.Contains("System.Int64"))
                        prop.SetValue(obj, 0L);
                    else if (PropertyType.Contains("System.DateTime"))
                    {
                        DateTime dt = Convert.ToDateTime("1900/01/01");
                        prop.SetValue(obj, dt);
                    }
                    else if (PropertyType.Contains("System.Double"))
                        prop.SetValue(obj, 0.0d);
                    else if (PropertyType.Contains("System.Decimal"))
                        prop.SetValue(obj, 0.0M);

                }
            }

            return obj;
        }

        public static void WriteToLog(Exception ex, string Module = "")
        {
            var startupPath =
               new DirectoryInfo(
                   Path.GetDirectoryName(
                       Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent
                   .FullName;
            File.AppendAllText(startupPath + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + "::" + Module + "::" + Environment.NewLine);
        }
        public static void WriteValue(string value, string fileName = "ErrorLog.txt")
        {
            var startupPath =
               new DirectoryInfo(
                   Path.GetDirectoryName(
                       Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent
                   .FullName;
            File.AppendAllText(startupPath + "\\" + fileName, DateTime.Now.ToString(CultureInfo.InvariantCulture) + "::" + value + Environment.NewLine);
        }
        public static string GetServerName
        {
            get
            {
                var strHostName = Dns.GetHostName();
                return strHostName;
            }
        }

        public static string ServerIpFour
        {
            get
            {
                var localIp = "?";
                var hostName = Dns.GetHostName();
                var host = Dns.GetHostEntry(hostName);
                foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily.ToString() == "InterNetwork"))
                {
                    localIp = ip.ToString();
                }
                return localIp;
            }
        }

        public static string GetUserIp()
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
                    strIp = ServerIpFour;
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
            catch (Exception e)
            {
                return "127.0.0.1";
            }
        }

        public static string GetUserBrowser()
        {
            var userBrowser = HttpContext.Current.Request.Browser;
            return " Browser: " + userBrowser.Browser + ", Version: " + userBrowser.Version;
        }

        public static string GetUserAgent()
        {
            var userAgent = HttpContext.Current.Request.UserAgent;
            return " UserAgent: " + userAgent;
        }

        public static string GetUserPcInfo()
        {
            return "IP: " + GetUserIp() + "," + GetUserBrowser();
        }

        public static string GetCurentUserRole()
        {
            try
            {
                var session = HttpContext.Current.Session;
                return session["Rolename"].ToString();
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        public static string GetCurentUser()
        {
            try
            {
                var session = HttpContext.Current.Session;
                return session["UserName"].ToString();
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }
        public static string GetCurentUserBranch()
        {
            try
            {
                var session = HttpContext.Current.Session;
                return session["UserBranchId"].ToString();
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }


        public static int GetCurentUserId()
        {
            try
            {
                var session = HttpContext.Current.Session;
                var userId = session["UserId"].ToString();
                return Convert.ToInt32(userId);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static string JoinDictonaryKeyValue(Dictionary<dynamic, dynamic> dictionary, string join = " = ", string separateWith = " AND ")
        {
            string str = dictionary.Select(x => x.Key + @join + x.Value).Aggregate((s1, s2) => s1 + separateWith + s2);
            return str;
        }

        public static string UniqueIdByLinq(string maxValue, string prefix)
        {
            const string uniq = "0";
            maxValue = String.IsNullOrEmpty(maxValue) ? prefix + uniq : maxValue;
            var intPart = maxValue.Substring(prefix.Length, maxValue.Length - prefix.Length);
            var id = (BigInteger.Parse(intPart, NumberStyles.Float, CultureInfo.InvariantCulture) + 1).ToString();
            id = prefix + id;
            return id;
        }

        public static string UniqueIdByQuery(string tableName, string columnName = "Id", string whereValue = null)
        {
            string query;
            if (whereValue != null)
            {
                query = "Select isnull(max(convert(numeric, substring(" + columnName + " , 5, LEN(" + columnName +
                        ")-4))),0)+1 as MaxNo from " + tableName + " WHERE " + whereValue;
            }
            else
            {
                query = "Select isnull(max(convert(numeric, substring(" + columnName + " , 5, LEN(" + columnName +
                        ")-4))),0)+1 as MaxNo from " + tableName + " ";

            }
            return query;

        }

        public static List<string> GolobalInfo(string functionName, string displayField = "")
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            List<string> valueField = new List<string>();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConnectionString))
                {
                    string query;

                    if (displayField == "")
                        query = String.Format("select * from GlobalConfig where FunctionName='{0}'", functionName);
                    else
                        query = String.Format("select * from GlobalConfig where FunctionName='{0}' AND DisplayField='{1}'", functionName, displayField);

                    var cmd = new SqlCommand(query, myConnection);
                    myConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            valueField.Add(reader["ValueField"].ToString());
                        }
                        myConnection.Close();
                    }
                }
            }
            catch (Exception)
            {
                valueField = null;
            }
            return valueField;
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

        private const string ScriptTag = "<script type=\"text/javascript\" language=\"javascript\">{0}</script>";

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
        public static void ExportToXml<T>(string path, T obj)
        {
            var writer = new XmlSerializer(typeof(T));
            var filePath = path + Guid.NewGuid() + ".xml";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var file = File.Create(filePath);
            writer.Serialize(file, obj);
            file.Close();
        }
        public static void StartEndDateConvert(string startDate, string endDate, out DateTime _startDate, out DateTime _endDate)
        {
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrWhiteSpace(startDate))
            {
                _startDate = DateTime.ParseExact(startDate, "MMM dd, yyyy", CultureInfo.InvariantCulture);
            }
            if (!String.IsNullOrEmpty(endDate) && !String.IsNullOrWhiteSpace(endDate))
            {
                _endDate = DateTime.ParseExact(endDate, "MMM dd, yyyy", CultureInfo.InvariantCulture);
            }
        }

        public static string GetContentType(string fileExtension)
        {
            if (String.IsNullOrEmpty(fileExtension))
                return String.Empty;

            string contentType = String.Empty;
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                    contentType = "text/HTML";
                    break;

                case ".txt":
                    contentType = "text/plain";
                    break;

                case ".doc":
                case ".rtf":
                case ".docx":
                    contentType = "Application/msword";
                    break;

                case ".xls":
                case ".xlsx":
                    contentType = "Application/x-msexcel";
                    break;

                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;

                case ".gif":
                    contentType = "image/GIF";
                    break;

                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;

            }

            return contentType;
        }

        public static void WriteVelue(string value, string fileName)
        {
            var startupPath = new DirectoryInfo(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent.FullName;
            File.AppendAllText(startupPath + "\\" + fileName + ".txt", "\n" + Environment.NewLine + DateTime.Now.ToString() + "::" + value);
        }


        public static IEnumerable<T> FindStringFromValue<T>(IEnumerable<T> aList, string searchItem)
        {
            List<T> bList = new List<T>();
            foreach (var akycinfo in aList)
            {
                var found = false;
                foreach (var prop in akycinfo.GetType().GetProperties())
                {
                    var propertyValue = prop.GetValue(akycinfo, null);
                    if (propertyValue != null && propertyValue.ToString().ToUpper().Contains(searchItem.ToUpper()))
                    {
                        found = true;
                        break;
                    }
                }
                if (found == true)
                    bList.Add(akycinfo);
            }
            return bList;
        }

    }
}
