using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Repositories;

namespace SFB.PCM.Backend.Repositories
{
    public class PurchaseTxnRepository(IServiceProvider services) : BaseRepository<SFBContext>(services)
    {

    }
}
