using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.PCM;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.PCM.Backend.Repositories
{
    public class SupplierRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Name", "Address" };
        }

        internal async Task<PagedListModel<ESupplier>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.PCMSupplier.AsQueryable();

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "SupplierId" });

            return result;
        }

        internal async Task<ESupplier> Create(ESupplier supplier)
        {
            Context.PCMSupplier.Add(supplier);

            await Context.SaveChangesAsync();

            return supplier;
        }

        internal async Task<ESupplier> Update(ESupplier supplier)
        {
            Context.PCMSupplier.Update(supplier);
            await Context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> Delete(int supplierId)
        {
            var entity = await Context.PCMSupplier.FindAsync(supplierId);

            if (entity is null) throw new ControllerException("Proveedor no encontrado");

            try
            {
                Context.PCMSupplier.Remove(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                    throw new ControllerException("No es posible eliminar, item en uso");
                throw;
            }
        }
    }
}
