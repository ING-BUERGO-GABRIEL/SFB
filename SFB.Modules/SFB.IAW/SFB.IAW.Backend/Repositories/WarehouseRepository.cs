using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class WarehouseRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Name", "Location" };
        }

        internal async Task<PagedListModel<EWarehouse>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.IAWWarehouse.AsQueryable();

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "WarehouseId" });

            return result;
        }
        internal async Task<EWarehouse> Create(EWarehouse warehouse)
        {
            warehouse.Status = true;

            Context.IAWWarehouse.Add(warehouse);

            await Context.SaveChangesAsync();

            return warehouse;
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

    }
}
