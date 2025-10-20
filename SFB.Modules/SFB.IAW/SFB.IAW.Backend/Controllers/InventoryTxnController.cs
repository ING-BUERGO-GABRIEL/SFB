using SFB.IAW.Backend.Repositories;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;
using SFB.IAW.Shared.Models;
using SFB.Infrastructure.Entities.IAW;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace SFB.IAW.Backend.Controllers
{
    [Route("api/IAW/[controller]/[action]")]
    public class InventoryTxnController : BaseController<SFBContext, InventoryTxnRepository>
    {
        public InventoryTxnController(IServiceProvider services) : base(services) { }

        [HttpGet]
        public async Task<IActionResult> GetPage(
                [FromQuery] int pageSize,
                [FromQuery] int pageNumber = 1,
                [FromQuery] string? filter = null)
        {
            try
            {
                var result = await Repository.GetPage(filter, pageSize, pageNumber);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetPagePrueva()
        {
            try
            {

                return OkResult("Hola");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MWarehouse product)
        {
            try
            {
                var eWarehouse = product.Adapt<EWarehouse>();

                var resultcreate = await Repository.Create(eWarehouse);

                var result = resultcreate.Adapt<MWarehouse>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MWarehouse product)
        {
            try
            {
                var eProduct = product.Adapt<EWarehouse>();

                var resultcreate = await Repository.Update(eProduct);

                var result = resultcreate.Adapt<MWarehouse>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{warehouseId:int}")]
        public async Task<IActionResult> Delete(int warehouseId)
        {
            try
            {
                await Repository.Delete(warehouseId);

                return DeletedResult();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
