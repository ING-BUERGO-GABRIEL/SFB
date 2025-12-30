using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;
using SFB.SOM.Backend.Repositories;

namespace SFB.SOM.Backend.Controllers
{
    [Route("api/SOM/[controller]/[action]")]
    public class SalesTxnController(IServiceProvider services) : BaseController<SFBContext, SalesTxnRepositorie>(services)
    {

    }
}
