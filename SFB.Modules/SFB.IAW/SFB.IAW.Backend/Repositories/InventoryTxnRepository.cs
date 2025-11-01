using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Infrastructure.Entities.IAW.Sealed;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class InventoryTxnRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Name", "Location" };
        }

        internal async Task<PagedListModel<EInventoryTxn>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.IAWInventoryTxn.AsQueryable();

            var result = await base.GetPage(query, filter, pageSize, pageNumber);

            return result;
        }
        internal async Task<EInventoryTxn> Create(EInventoryTxn invTxn)
        {

            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                await CreateTxn(invTxn);
                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return invTxn;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        internal  async Task<EInventoryTxn> CreateTxn(EInventoryTxn invTxn)
        {

            Context.IAWInventoryTxn.Add(invTxn);

            return invTxn;
        }


        internal async Task<EWarehouse> Update(EWarehouse product)
        {
            Context.IAWWarehouse.Update(product);
            await Context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> Delete(int warehouseId)
        {
            var entity = await Context.IAWWarehouse.FindAsync(warehouseId);

            if (entity is null) throw new ControllerException("Almacen no encontrado");

            try
            {
                Context.IAWWarehouse.Remove(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                    throw new ControllerException("No es posible eliminar, item en uso"); // si no es por FK, relanzamos
                throw;
            }
        }

        internal async Task<Dictionary<string, object>> GetMetadata()
        {
            var cmbWerehouses = await Context.IAWWarehouse
                .Where(w => w.Status)
                .ToListAsync();

            return new Dictionary<string, object>
            {
                {"CmbType",InvType.List() },
                {"CmbWerehouses",cmbWerehouses },
                {"CmbStatus",InvStatus.List() }
            };
        }
    }
}
