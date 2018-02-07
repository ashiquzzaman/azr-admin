using AzR.Core.ModelConfig;
using System.Web.Mvc;

namespace AzR.Admin.Web.ViewPage
{
    public abstract class BaseViewPage : WebViewPage
    {
        protected CmsUserViewModel CmsUser
        {
            get { return Context.Items["APPUSER"] as CmsUserViewModel; }

        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public CmsUserViewModel CmsUser
        {
            get { return Context.Items["APPUSER"] as CmsUserViewModel; }

        }
    }
}