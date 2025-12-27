using Microsoft.EntityFrameworkCore;
using SFB.IAW.Shared.Sealed;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class InventoryTxnRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        private readonly StockRepository _stockRepository = new StockRepository(context);

        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Name", "Location" };
        }

        internal async Task<PagedListModel<EInventoryTxn>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.IAWInventoryTxn
                        .Where(inv=> !inv.Delete)
                        .Include(i=> i.WarehouseOrigin)
                        .Include(i => i.WarehouseDest);

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

        public  async Task<EInventoryTxn> CreateTxn(EInventoryTxn invTxn)
        {

            await ValidateTxnInit(invTxn.InvDetails, invTxn.Type);

            await _stockRepository.UpdateFromTxn(invTxn);

            Context.IAWInventoryTxn.Add(invTxn);

            return invTxn;
        }


        internal async Task ValidateTxnInit(ICollection<EInvDetail> invDetail, string type)
        {
            if (invDetail is null || invDetail.Count == 0)
                return;

            var productIds = invDetail.Select(d => d.NroProduct).Distinct().ToList();

            // Productos que ya tienen una transacción inicial activa (si la hay)
            var initialProductIds = await Context.IAWInvDetail.AsNoTracking()
                .Where(d => productIds.Contains(d.NroProduct)
                            && d.InventoryTxn != null
                            && d.InventoryTxn.Type == InvType.TxnInicial.Code
                            && d.InventoryTxn.StatusCode == InvStatus.Activo.Code
                            && !d.InventoryTxn.Delete)
                .Select(d => d.NroProduct)
                .Distinct()
                .ToListAsync();

            // Si estamos creando una Txn Inicial, impedir duplicados: no permitir productos que ya tienen inicial
            if (type == InvType.TxnInicial.Code)
            {
                if (initialProductIds.Any())
                {
                    var productNames = await Context.IAWProducts
                        .AsNoTracking()
                        .Where(p => initialProductIds.Contains(p.NroProduct))
                        .Select(p => p.Name)
                        .ToListAsync();

                    throw new ControllerException($"No se puede crear transacción inicial: los productos ya tienen transacción inicial: {string.Join(", ", productNames)}");
                }

                return;
            }

            // Para transacciones distintas de inicial, asegurar que exista una inicial previa para cada producto
            var missing = productIds.Except(initialProductIds).ToList();

            if (missing.Any())
            {
                var productNames = await Context.IAWProducts.AsNoTracking()
                    .Where(p => missing.Contains(p.NroProduct))
                    .Select(p => p.Name)
                    .ToListAsync();

                throw new ControllerException($"Falta transacción inicial para los productos: {string.Join(", ", productNames)}");
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

        internal async Task<EInventoryTxn> GetById(int txnId)
        {
            return await Context.IAWInventoryTxn.AsNoTracking()
                .Include(t=>t.InvDetails)
                .ThenInclude(p=>p.Product)
                .FirstAsync(t=>t.TxnId == txnId);
        }

        internal async Task<EInventoryTxn> Anular(int txnId)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var result = await AnularTxn(txnId);
                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<EInventoryTxn> AnularTxn(int txnId)
        {
            var txn = Context.IAWInventoryTxn
                .Include(t => t.InvDetails)
                .First(t => t.TxnId == txnId);
            await _stockRepository.RevertFromTxn(txn);

            txn.StatusCode = InvStatus.Anulado.Code;            
            Context.IAWInventoryTxn.Update(txn);
            return txn;
        }
    }
}
