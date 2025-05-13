using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.AMS.Backend.Repositories;
using SFB.AMS.Shared.Models;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;


namespace SFB.AMS.Backend.Controllers
{
    [Route("api/AMS/[controller]/[action]")]
    public class PersonController(SFBContext context) : BaseController<SFBContext, PersonRepository>(context)
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
                return InternalServerError(ex);
            }
        }
    }
}
