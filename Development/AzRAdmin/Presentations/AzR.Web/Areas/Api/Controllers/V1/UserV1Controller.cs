using AzR.Utilities;
using AzR.Web.Controllers;
using AzR.Web.Filters;
using PagedList;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;

namespace AzR.Web.Areas.Api.Controllers.V1
{
    [Authorize]
    [RoutePrefix("api/v1/users")]
    public class UserV1Controller : BaseApiController
    {
        private IUserService _user;
        public UserV1Controller(IBaseService general, IUserService user) : base(general)
        {
            _user = user;
        }

        /// <summary>
        /// Get All Task with Paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        [Route("")]
        public IHttpActionResult Get(int page = 1, int pageSize = 30)
        {

            var model = _user.GetAll().OrderBy(s => s.Id).ToPagedList(page, pageSize);
            var result = new
            {
                Error = false,
                Message = "",
                Data = model
            };
            return Ok(result);

        }
        [Route("dropdown")]
        public IHttpActionResult GetDropdown()
        {

            var model = _user.LoadUsers();
            var result = new
            {
                Error = false,
                Message = "",
                Data = model
            };
            return Ok(result);

        }
        [Route("create")]
        [HttpPost]
        [HttpModelValidator]
        public IHttpActionResult Post([FromBody]UserViewModel model)
        {
            try
            {
                model.Active = true;
                if (model.OrgId == 0 || model.OrgId == null)
                {
                    model.OrgId = 1;
                }
                var creationResult = _user.Create(model);
                if (creationResult != null) model.Id = creationResult.Id;

                var result = new
                {
                    Error = creationResult == null,
                    Message = creationResult == null ? "Menu Creation failed." : "",
                    Data = creationResult == null ? null : model
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Menu Creation failed.");
            }

        }

        [Route("edit")]
        [HttpPut]
        [HttpModelValidator]
        public IHttpActionResult Put([FromBody]UserViewModel model)
        {
            try
            {
                if (model.OrgId == 0 || model.OrgId == null)
                {
                    model.OrgId = 1;
                }
                var updateResult = _user.Update(model);
                if (updateResult != null) model.Id = updateResult.Id;

                var result = new
                {
                    Error = updateResult == null,
                    Message = updateResult == null ? "Menu Update failed." : "",
                    Data = updateResult == null ? null : model
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Menu Update failed.");
            }

        }

        [Route("delete")]
        [HttpDelete]
        [HttpModelValidator]
        public async Task<IHttpActionResult> Delete([FromBody]int id)
        {
            try
            {
                var deactive = await _user.DeActiveAsync(id);

                var result = new
                {
                    Error = deactive != 1,
                    Message = deactive != 1 ? "Menu Deactivation failed." : "",
                    Data = deactive
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Menu Deactivation failed.");
            }

        }





    }
}
