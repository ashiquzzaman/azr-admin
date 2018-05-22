using AzR.Core.Config;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities.Exentions;
using AzR.Web.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AzR.WebFw.Controllers;
using AzR.WebFw.Filters;

namespace AzR.Web.Areas.Api.Controllers.V1
{
    [Authorize]
    [RoutePrefix("api/v1/menus")]
    public class MenuV1Controller : BaseApiController
    {
        private IMenuService _menu;
        public MenuV1Controller(IMenuService menu)
        {
            _menu = menu;
        }

        /// <summary>
        /// Get All Task with Paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        [Route("")]
        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = 30)
        {

            var model = await _menu.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync(page, pageSize);
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

            var model = await _menu.LoadParentAsync();
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
        public async Task<IHttpActionResult> Post([FromBody]MenuViewModel model)
        {
            try
            {
                model.IsActive = true;

                var creationResult = await _menu.CreateAsync(model);
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
        public async Task<IHttpActionResult> Put([FromBody]MenuViewModel model)
        {
            try
            {
                var updateResult = await _menu.UpdateAsync(model);
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
                var deactive = await _menu.DeActiveAsync(id);

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
