using Mapster;
using Microsoft.AspNetCore.Mvc;
using SGD.AMS.Backend.Repositories;
using SGD.AMS.Shared.Models;
using SGD.Infrastructure.Contexts;
using SGD.Shared.Backend.Controller;


namespace SGD.AMS.Backend.Controllers
{
    [Route("api/AMS/[controller]/[action]")]
    public class PersonController(SGDContext context) : BaseController<SGDContext, PersonRepository>(context)
    {
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            try
            {
                var personList = await Repository.GetPersons();
                var result = personList.Adapt<List<MPerson>>();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult(ex);
            }
        }
    }
}
