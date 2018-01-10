using System.Configuration;

//Md. Ashiquzzaman
namespace AzR.Core.Utilities
{
    public class LdapUtility
    {
        private readonly LdapConfig _config;

        public LdapUtility()
        {
            _config = new LdapConfig();
        }
     }

    public class LdapConfig
    {
        private readonly string _ldapServer = ConfigurationManager.AppSettings["LdapServer"];
        public readonly string AdminUser = ConfigurationManager.AppSettings["AdminUser"];
        public readonly string AdminUserPassword = ConfigurationManager.AppSettings["AdminUserPassword"];
        private readonly string _orgUnit = ConfigurationManager.AppSettings["OrganizationUnit"];
        public readonly string OrgSuffix = ConfigurationManager.AppSettings["OrganizationSuffix"];
        public string LdapUrlWithAdmin
        {
            get { return string.Format("LDAP://{0}/CN={1},{2}", _ldapServer, AdminUser, _orgUnit); }
        }

        public string LdapUrl
        {
            get { return string.Format("LDAP://{0}/{1}", _ldapServer, _orgUnit); }
        }
    }

}
