using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Models;
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

        internal async Task<PagedListModel<EProduct>> GetPage(string? filter, int pageSize,int pageNumber)
        {
            var query = Context.IAWProducts.AsQueryable();

            var result = await base.GetPage(query, filter,pageSize, pageNumber);

            return result;
        }

        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "NroProduct", "Name", "SerialNumber" }; 
        }
    }
}
