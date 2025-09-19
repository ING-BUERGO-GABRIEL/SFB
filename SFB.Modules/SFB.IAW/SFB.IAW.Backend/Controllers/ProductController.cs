using SFB.IAW.Backend.Repositories;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;
using Microsoft.AspNetCore.Mvc;
using SFB.IAW.Shared.Models;
using Mapster;
using SFB.Infrastructure.Entities.IAW;

namespace SFB.IAW.Backend.Controllers
{
    [Route("api/IAW/[controller]/[action]")]
    public class ProductController(IServiceProvider services) : BaseController<SFBContext, ProductRepository>(services)
    {
        [HttpGet]
        public async Task<IActionResult> GetPage(
            [FromQuery] int pageSize,
            [FromQuery] int PageNumber,
            [FromQuery] string? filter = null)
        {
            try
            {
                var result = await Repository.GetPage(filter,pageSize,PageNumber);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MProduct product)
        {
            try
            {
                var eProduct = product.Adapt<EProduct>();

                var resultcreate = await Repository.Create(eProduct);

                var result = resultcreate.Adapt<MProduct>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MProduct product)
        {
            try
            {
                var eProduct = product.Adapt<EProduct>();

                var resultcreate = await Repository.Update(eProduct);

                var result = resultcreate.Adapt<MProduct>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
