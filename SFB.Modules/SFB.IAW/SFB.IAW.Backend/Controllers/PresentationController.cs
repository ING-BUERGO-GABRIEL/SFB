using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.IAW.Backend.Repositories;
using SFB.IAW.Shared.Models;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Controller;

namespace SFB.IAW.Backend.Controllers
{
    [Route("api/IAW/[controller]/[action]")]
    public class PresentationController(IServiceProvider services) : BaseController<SFBContext, PresentationRepository>(services)
    {
        [HttpGet]
        public async Task<IActionResult> GetPage(
            [FromQuery] int pageSize,
            [FromQuery] int pageNumber,
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MPresentation model)
        {
            try
            {
                var entity = model.Adapt<EPresentation>();
                var created = await Repository.Create(entity);
                var result = created.Adapt<MPresentation>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MPresentation model)
        {
            try
            {
                var entity = model.Adapt<EPresentation>();
                var updated = await Repository.Update(entity);
                var result = updated.Adapt<MPresentation>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                await Repository.Delete(code);

                return DeletedResult();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
