using AzR.Core.Config;
using AzR.Student.Core.Services.Interface;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AzR.WebFw.Controllers;

namespace AzR.Web.Areas.Example.Controllers
{
    [RoutePrefix("api/v1/students")]
    public class StudentV1Controller : BaseApiController
    {
        private IStudentService _student;
        public StudentV1Controller(IStudentService student)
        {
            _student = student;
        }

        /// <summary>
        /// Get All Task with Paging
        /// </summary>
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {

            var model = await _student.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync(1, 30);
            var result = new
            {
                Error = false,
                Message = "",
                Data = model
            };
            return Ok(result);

        }





    }
}
