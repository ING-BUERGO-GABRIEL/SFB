using Microsoft.AspNetCore.Mvc;
using SGD.CMS.Backend.Repositories;
using SGD.Infrastructure.Contexts;
using SGD.Shared.Backend.Controller;

namespace SGD.CMS.Backend.Controllers
{
    [Route("api/CMS/[controller]/[action]")]
    public class OptionMenuController : BaseController<SGDContext, OptionMenuRepository>
    {
        public OptionMenuController(SGDContext context)
        {
            Repository = new OptionMenuRepository(context);
        }

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
                return InternalServerErrorResult(ex);
            }
        }
    }
}
