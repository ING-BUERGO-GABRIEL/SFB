using Microsoft.AspNetCore.Mvc;
using SFB.CMS.Backend.Repositories;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;

namespace SFB.CMS.Backend.Controllers
{
    [Route("api/CMS/[controller]/[action]")]
    public class ModuleController : BaseController<SGDContext, ModuleRepository>
    {
        public ModuleController(SGDContext context)
        {
            Repository = new ModuleRepository(context);
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeModules()
        {
            try
            {
                var user = User.FindFirst("NameUser")?.Value;

                var modules = await Repository.GetModulesByUser(user);

                return OkResult(modules);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult(ex);
            }
        }
    }
}
