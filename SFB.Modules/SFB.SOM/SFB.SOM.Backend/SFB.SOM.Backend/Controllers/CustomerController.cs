using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.AMS;
using SFB.Shared.Backend.Controller;
using SFB.SOM.Backend.Repositories;
using SFB.SOM.Shared.Models;

namespace SFB.SOM.Backend.Controllers
{
    [Route("api/SOM/[controller]/[action]")]
    public class CustomerController(IServiceProvider services) : BaseController<SFBContext, CustomerRepository>(services)
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
        public async Task<IActionResult> Create([FromBody] MCustomer customer)
        {
            try
            {
                var eCustomer = customer.Adapt<ECustomer>();

                var resultCreate = await Repository.Create(eCustomer);

                var result = resultCreate.Adapt<MCustomer>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MCustomer customer)
        {
            try
            {
                var eCustomer = customer.Adapt<ECustomer>();

                var resultUpdate = await Repository.Update(eCustomer);

                var result = resultUpdate.Adapt<MCustomer>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{customerId:int}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                await Repository.Delete(customerId);

                return DeletedResult();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
