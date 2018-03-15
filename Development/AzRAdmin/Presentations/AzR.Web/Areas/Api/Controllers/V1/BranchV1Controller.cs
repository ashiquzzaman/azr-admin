using AzR.Core.Config;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities.Exentions;
using AzR.Web.Controllers;
using AzR.Web.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AzR.Web.Areas.Api.Controllers.V1
{
    [Authorize]
    [RoutePrefix("api/v1/branchs")]
    public class BranchV1Controller : BaseApiController
    {
        private IBranchService _branch;
        public BranchV1Controller(IBranchService branch)
        {
            _branch = branch;
        }

        /// <summary>
        /// Get All Branch with Paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        [Route("")]
        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = 30)
        {

            var model = await _branch.GetAllAsync().OrderBy(s => s.Id).ToPagedListAsync(page, pageSize);
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

            var model = await _branch.LoadBranchsAsync();
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
        public async Task<IHttpActionResult> Post([FromBody]BranchViewModel model)
        {
            model.ParentId = 1;
            model.Active = true;
            try
            {
                var branch = await _branch.CreateAsync(model);
                if (branch != null) model.Id = branch.Id;

                var result = new
                {
                    Error = branch == null,
                    Message = branch == null ? "Branch Creation failed." : "",
                    Data = branch == null ? null : model
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Branch Creation failed.");
            }

        }

        [Route("edit")]
        [HttpPut]
        [HttpModelValidator]
        public async Task<IHttpActionResult> Put([FromBody]BranchViewModel model)
        {
            model.ParentId = 1;
            try
            {
                var branch = await _branch.UpdateAsync(model);
                if (branch != null) model.Id = branch.Id;

                var result = new
                {
                    Error = branch == null,
                    Message = branch == null ? "Branch Update failed." : "",
                    Data = branch == null ? null : model
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Branch Update failed.");
            }

        }

        [Route("delete")]
        [HttpDelete]
        [HttpModelValidator]
        public async Task<IHttpActionResult> Delete([FromBody]int id)
        {
            try
            {
                var branch = await _branch.DeActiveAsync(id);

                var result = new
                {
                    Error = branch != 1,
                    Message = branch != 1 ? "Branch Deactivation failed." : "",
                    Data = branch
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                return BadRequest("Branch Deactivation failed.");
            }

        }





    }
}
