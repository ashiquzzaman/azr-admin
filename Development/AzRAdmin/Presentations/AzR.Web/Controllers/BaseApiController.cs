using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AzR.Core.HelperModels;
using AzR.Core.Services.Interface;

namespace AzR.Web.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IBaseService BaseHepler;
        protected BaseApiController(IBaseService general)
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
            get { return HttpContext.Current.Items["APPUSER"] as CmsUserViewModel; }
        }


    }
}