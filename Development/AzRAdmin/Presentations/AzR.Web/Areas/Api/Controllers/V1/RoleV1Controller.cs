using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities.Exentions;
using AzR.Web.Controllers;
using AzR.Web.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AzR.Web.Areas.Api.Controllers.V1
{
    [Authorize]
    [RoutePrefix("api/v1/roles")]
    public class RoleV1Controller : BaseApiController
    {
        private IRoleService _role;
        public RoleV1Controller(IRoleService role)
        {
            _role = role;
        }

        /// <summary>
        /// Get All Task with Paging
        /// </summary>
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {

            var model = await _role.GetAllAsync();
            var result = new
            {
                Error = false,
                Message = "",
                Data = model
            };
            return Ok(result);

        }
        [Route("dropdown")]
        public async Task<IHttpActionResult> GetDropdown()
        {

            var model = await _role.LoadParentAsync();
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
        public async Task<IHttpActionResult> Post([FromBody]RoleViewModel model)
        {
            model.IsActive = true;
            try
            {
                var branch = await _role.CreateAsync(model);
                if (branch != null) model.Id = branch.Id;

                var result = new
                {
                    Error = branch == null,
                    Message = branch == null ? "Role Creation failed." : "",
                    Data = branch == null ? null : model
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Role Creation failed.");
            }

        }

        [Route("edit")]
        [HttpPut]
        [HttpModelValidator]
        public async Task<IHttpActionResult> Put([FromBody]RoleViewModel model)
        {
            try
            {
                var branch = await _role.UpdateAsync(model);
                if (branch != null) model.Id = branch.Id;

                var result = new
                {
                    Error = branch == null,
                    Message = branch == null ? "Role Update failed." : "",
                    Data = branch == null ? null : model
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Role Update failed.");
            }

        }

        [Route("delete")]
        [HttpDelete]
        [HttpModelValidator]
        public async Task<IHttpActionResult> Delete([FromBody]int id)
        {
            try
            {
                var branch = await _role.DeActiveAsync(id);

                var result = new
                {
                    Error = branch != 1,
                    Message = branch != 1 ? "Role Deactivation failed." : "",
                    Data = branch
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Role Deactivation failed.");
            }

        }





    }
}
