using System.Web.Mvc;

namespace AzR.Web.Root.Views
{
    public class CssViewResult : PartialViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/css";
            base.ExecuteResult(context);
        }
    }
}
