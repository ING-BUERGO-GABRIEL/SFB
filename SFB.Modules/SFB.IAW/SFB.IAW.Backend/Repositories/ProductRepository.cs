using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class ProductRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "NroProduct", "Name", "SerialNumber" }; 
        }
        internal async Task<PagedListModel<EProduct>> GetPage(string? filter, int pageSize,int pageNumber)
        {
            var query = Context.IAWProducts.Where(p=>p.Status);

            var result = await base.GetPage(query, filter,pageSize, pageNumber,new List<string> { "NroProduct" });

            return result;
        }
        internal async Task<EProduct> Create(EProduct product)
        {
            product.Status = true;

            Context.IAWProducts.Add(product);
            await Context.SaveChangesAsync();

            return product;
        }

        internal async Task<EProduct> Update(EProduct product)
        {

            Context.IAWProducts.Update(product);
            await Context.SaveChangesAsync();

            return product;
        }

    }
}
