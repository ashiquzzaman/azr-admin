using AzR.Core.HelperModels;
using System.Web.Mvc;

namespace AzR.Web.Root.ViewPage
{
    public abstract class BaseViewPage : WebViewPage
    {
        protected CmsUserViewModel CmsUser
        {
            get { return Context.Items["AzRADMINUSER"] as CmsUserViewModel; }

        }
        protected string CurrentAction
        {
            get { return ViewContext.RouteData.Values["action"].ToString(); }

        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public CmsUserViewModel CmsUser
        {
            get { return Context.Items["AzRADMINUSER"] as CmsUserViewModel; }

        }
        protected string CurrentAction
        {
            get { return ViewContext.RouteData.Values["action"].ToString(); }

        }
    }
}