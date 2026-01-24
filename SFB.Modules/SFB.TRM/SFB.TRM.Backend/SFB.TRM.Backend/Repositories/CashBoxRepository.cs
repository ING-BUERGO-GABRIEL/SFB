using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.TRM;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.TRM.Backend.Repositories
{
    public class CashBoxRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Name", "Type", "CurrencyCode" };
        }

        internal async Task<PagedListModel<ECashBox>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.TRMCashBoxs.AsQueryable();

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "CashBoxId" });

            return result;
        }

        internal async Task<ECashBox> Create(ECashBox cashBox)
        {
            cashBox.Active = true;

            Context.TRMCashBoxs.Add(cashBox);

            await Context.SaveChangesAsync();

            return cashBox;
        }

        internal async Task<ECashBox> Update(ECashBox cashBox)
        {
            Context.TRMCashBoxs.Update(cashBox);
            await Context.SaveChangesAsync();
            return cashBox;
        }

        public async Task<bool> Delete(int cashBoxId)
        {
            var entity = await Context.TRMCashBoxs.FindAsync(cashBoxId);

            if (entity is null) throw new ControllerException("Caja no encontrada");

            try
            {
                Context.TRMCashBoxs.Remove(entity);
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
