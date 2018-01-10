using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AzR.Core.Business;
using AzR.Core.ModelConfig;

namespace VelocityWorkFlow.Web.Controllers.Mvc
{
    public abstract class BaseController : Controller
    {
        protected IBaseService BaseHepler;
        protected BaseController(IBaseService general)
        {
            BaseHepler = general;
        }

        protected IEnumerable<SelectListItem> Months
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Select((monthName, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = monthName
                       });
            }
        }
        protected CmsUserViewModel CmsUser
        {
            get { return HttpContext.Items["APPUSER"] as CmsUserViewModel; }
        }

    }
}