using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AzR.Core.HelperModels;
using AzR.Core.Services.Interface;

namespace AzR.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IBaseManager BaseHepler;
        protected BaseController(IBaseManager general)
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