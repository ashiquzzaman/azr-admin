using AzR.Utilities.Securities;
using System.Web.Mvc;

namespace AzR.Web.Root.ViewPage
{
    public abstract class BaseViewPage : WebViewPage
    {
        protected AppUserPrincipal CmsUser
        {
            get { return Context.Items["AzRADMINUSER"] as AppUserPrincipal; }

        }
        protected string CurrentAction
        {
            get { return ViewContext.RouteData.Values["action"].ToString(); }

        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public AppUserPrincipal CmsUser
        {
            get { return Context.Items["AzRADMINUSER"] as AppUserPrincipal; }

        }
        protected string CurrentAction
        {
            get { return ViewContext.RouteData.Values["action"].ToString(); }

        }
    }
}