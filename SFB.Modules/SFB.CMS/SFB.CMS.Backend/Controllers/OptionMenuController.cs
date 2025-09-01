using Microsoft.AspNetCore.Mvc;
using SFB.CMS.Backend.Repositories;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;

namespace SFB.CMS.Backend.Controllers
{
    [Route("api/CMS/[controller]/[action]")]
    public class OptionMenuController(IServiceProvider services) : BaseController<SFBContext, OptionMenuRepository>(services)
    {

        [HttpGet]
        public async Task<IActionResult> GetOptionMenu()
        {
            try
            {
                var user = User.FindFirst("NameUser")?.Value;

                var optionMenus = await Repository.GetOptionMenuByUser(user);

                //var result = optionMenus.Adapt<>

                return OkResult(optionMenus);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
