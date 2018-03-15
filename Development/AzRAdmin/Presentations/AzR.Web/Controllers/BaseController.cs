using AzR.Utilities.Securities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace AzR.Web.Controllers
{
    public abstract class BaseController : Controller
    {

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
        protected AppUserPrincipal CmsUser
        {
            get { return HttpContext.Items["AzRADMINUSER"] as AppUserPrincipal; }
        }

    }
}