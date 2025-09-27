using System.Runtime;
using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Helpers;
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

        public async Task<bool> Delete(int nroProd)
        {
            var entity = await Context.IAWProducts.FindAsync(nroProd);

            if (entity is null) throw new ControllerException("Producto no encontrado");

            try
            {
                Context.IAWProducts.Remove(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // si el error viene por foreign keys
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    // revertimos la eliminación y hacemos un "soft delete"
                    Context.Entry(entity).State = EntityState.Unchanged;
                    entity.Status = false;
                    await Context.SaveChangesAsync();
                    return true; // se procesó el "borrado lógico"
                }

                throw; // si no es por FK, relanzamos
            }
        }

    }
}
