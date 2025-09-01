using SFB.IAW.Shared.Models;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class ProductRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        internal async Task<EProduct> Create(EProduct product)
        {
            product.Status = true;

            Context.IAWProducts.Add(product);
            await Context.SaveChangesAsync();

            return product;
        }
    }
}
