using System.Collections.Generic;
using System.Web.Http;
using AzR.Core.ViewModels.Admin;

namespace AzR.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/notifications")]
    public class NotificationController : ApiController
    {

        [Route("")]
        public IHttpActionResult Get()
        {
            var model = new List<NotificationViewModel>
            {

            };
            return Ok(model);
        }
    }
}
