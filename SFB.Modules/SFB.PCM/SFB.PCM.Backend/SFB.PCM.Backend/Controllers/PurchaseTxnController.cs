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
    public class PurchaseTxnController(IServiceProvider services) : BaseController<SFBContext, PurchaseTxnRepository>(services)
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
                var result = page.Adapt<PagedListModel<MPurchaseTxn>>();
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
                return OkResult(txn.Adapt<MPurchaseTxn>());
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
        public async Task<IActionResult> Create([FromBody] MPurchaseTxn model)
        {
            try
            {
                var entity = model.Adapt<EPurchaseTxn>();
                var result = await Repository.Create(entity);
                return OkResult(result.Adapt<MPurchaseTxn>());
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MPurchaseTxn model)
        {
            try
            {
                var entity = model.Adapt<EPurchaseTxn>();
                var result = await Repository.Update(entity);
                return OkResult(result.Adapt<MPurchaseTxn>());
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
                return OkResult(result.Adapt<MPurchaseTxn>());
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
