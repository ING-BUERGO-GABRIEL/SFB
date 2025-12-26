using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.PCM;
using SFB.PCM.Backend.Repositories;
using SFB.PCM.Shared.Models;
using SFB.Shared.Backend.Controller;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;

namespace SFB.PCM.Backend.Controllers
{
    [Route("api/PCM/[controller]/[action]")]
    public class SupplierController(IServiceProvider services) : BaseController<SFBContext, SupplierRepository>(services)
    {
        [HttpGet]
        public async Task<IActionResult> GetPage(
            [FromQuery] int pageSize,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filter = null)
        {
            try
            {
                var result = await Repository.GetPage(filter, pageSize, pageNumber);

                return OkResult(result.Adapt<PagedListModel<MSupplier>>());
            }
            catch (ControllerException ex)
            {
                return ControlledException(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MSupplier supplier)
        {
            try
            {
                var entity = supplier.Adapt<ESupplier>();

                var created = await Repository.Create(entity);

                var result = created.Adapt<MSupplier>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MSupplier supplier)
        {
            try
            {
                var entity = supplier.Adapt<ESupplier>();

                var updated = await Repository.Update(entity);

                var result = updated.Adapt<MSupplier>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{supplierId:int}")]
        public async Task<IActionResult> Delete(int supplierId)
        {
            try
            {
                await Repository.Delete(supplierId);

                return DeletedResult();
            }
            catch (ControllerException ex)
            {
                return ControlledException(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
