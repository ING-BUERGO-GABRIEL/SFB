using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.SOM;
using SFB.Shared.Backend.Controller;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.SOM.Backend.Repositories;
using SFB.SOM.Shared.Models;

namespace SFB.SOM.Backend.Controllers
{
    [Route("api/SOM/[controller]/[action]")]
    public class SalesTxnController(IServiceProvider services) : BaseController<SFBContext, SalesTxnRepositorie>(services)
    {
        [HttpGet]
        public async Task<IActionResult> GetPage(
        [FromQuery] int pageSize,
        [FromQuery] int pageNumber = 1,
        [FromQuery] string? filter = null)
        {
            try
            {
                var page = await Repository.GetPage(filter, pageSize, pageNumber);
                var result = page.Adapt<PagedListModel<MSalesTxn>>();
                return OkResult(result);
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

        [HttpGet("{txnId:int}")]
        public async Task<IActionResult> GetById(int txnId)
        {
            try
            {
                var txn = await Repository.GetById(txnId);
                return OkResult(txn.Adapt<MSalesTxn>());
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
        public async Task<IActionResult> Create([FromBody] MSalesTxn model)
        {
            try
            {
                var entity = model.Adapt<ESalesTxn>();
                var result = await Repository.Create(entity);
                return OkResult(result.Adapt<MSalesTxn>());
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


        [HttpPost("{txnId:int}")]
        public async Task<IActionResult> Anular(int txnId)
        {
            try
            {
                var result = await Repository.Anular(txnId);
                return OkResult(result.Adapt<MSalesTxn>());
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

        [HttpGet]
        public async Task<IActionResult> GetMetadata()
        {
            try
            {
                var result = await Repository.GetMetadata();
                return OkResult(result);
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
