using Microsoft.AspNetCore.Mvc;
using SGD.CMS.Backend.Repositories;
using SGD.Infrastructure.Contexts;
using SGD.Shared.Backend.Controller;

namespace SGD.CMS.Backend.Controllers
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
