using System.Web.Mvc;
using AzR.Core.ModelConfig;

namespace VelocityWorkFlow.Web.ViewPag
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