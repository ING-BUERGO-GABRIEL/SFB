using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.PCM.Backend.Repositories;
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
                //var page = await Repository.GetPage(filter, pageSize, pageNumber);

                //var result = page.Adapt<PagedListModel<MInventoryTxn>>();

                return OkResult("result");
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
