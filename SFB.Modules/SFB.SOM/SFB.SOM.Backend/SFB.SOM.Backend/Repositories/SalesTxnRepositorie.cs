using SFB.IAW.Backend.Repositories;
using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Repositories;


namespace SFB.SOM.Backend.Repositories
{
    public class SalesTxnRepositorie(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        private readonly InventoryTxnRepository _invTxnRepository = new InventoryTxnRepository(context);

    }
}
