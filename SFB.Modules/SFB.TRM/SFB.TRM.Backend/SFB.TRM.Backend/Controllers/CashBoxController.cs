using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.TRM;
using SFB.Shared.Backend.Controller;
using SFB.TRM.Backend.Repositories;
using SFB.TRM.Shared.Models;

namespace SFB.TRM.Backend.Controllers
{
    [Route("api/TRM/[controller]/[action]")]
    public class CashBoxController(IServiceProvider services) : BaseController<SFBContext, CashBoxRepository>(services)
    {
        [HttpGet]
        public async Task<IActionResult> GetPage(
            [FromQuery] int pageSize,
            [FromQuery] int PageNumber,
            [FromQuery] string? filter = null)
        {
            try
            {
                var result = await Repository.GetPage(filter, pageSize, PageNumber);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MCashBox cashBox)
        {
            try
            {
                var entity = cashBox.Adapt<ECashBox>();

                var resultCreate = await Repository.Create(entity);

                var result = resultCreate.Adapt<MCashBox>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MCashBox cashBox)
        {
            try
            {
                var entity = cashBox.Adapt<ECashBox>();

                var resultUpdate = await Repository.Update(entity);

                var result = resultUpdate.Adapt<MCashBox>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{cashBoxId:int}")]
        public async Task<IActionResult> Delete(int cashBoxId)
        {
            try
            {
                await Repository.Delete(cashBoxId);

                return DeletedResult();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
