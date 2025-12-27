using Microsoft.EntityFrameworkCore;
using SFB.IAW.Backend.Repositories;
using SFB.IAW.Shared.Sealed;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Infrastructure.Entities.PCM;
using SFB.PCM.Shared.Sealed;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.PCM.Backend.Repositories
{
    public class PurchaseTxnRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        private readonly InventoryTxnRepository _invTxnRepository = new InventoryTxnRepository(context);

        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Reference", "Supplier.Name" };
        }

        internal async Task<PagedListModel<EPurchaseTxn>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.PCMPurchaseTxn
                .Where(p => !p.Delete)
                .Include(p => p.Supplier)
                .Include(p => p.Warehouse);

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "TxnId" });

            return result;
        }

        internal async Task<EPurchaseTxn> Create(EPurchaseTxn purchase)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                purchase.GrandTotal = purchase.Details?.Sum(d => d.TotalCost) ?? 0m;

                Context.PCMPurchaseTxn.Add(purchase);
                await Context.SaveChangesAsync();

                var invTxn = BuildInventoryTxnFromPurchase(purchase);

                await _invTxnRepository.CreateTxn(invTxn);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return purchase;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        internal async Task<EPurchaseTxn?> GetById(int txnId)
        {
            return await Context.PCMPurchaseTxn.AsNoTracking()
                .Include(p => p.Supplier)
                .Include(p => p.Warehouse)
                .Include(p => p.Details)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(p => p.TxnId == txnId);
        }

        internal async Task<Dictionary<string, object>> GetMetadata()
        {
            var cmbSuppliers = await Context.PCMSupplier
                .AsNoTracking()
                .ToListAsync();

            var cmbWarehouses = await Context.IAWWarehouse
                .Where(w => w.Status)
                .AsNoTracking()
                .ToListAsync();

            return new Dictionary<string, object>
            {
                {"CmbSuppliers", cmbSuppliers},
                {"CmbWarehouses", cmbWarehouses},
                {"CmbStatus", PurchaseStatus.List()},
                {"CmbType", PurchaseType.List()},
            };
        }

      

        internal async Task<EPurchaseTxn> Anular(int txnId)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var purchase = await Context.PCMPurchaseTxn
                    .Include(p => p.Details)
                    .FirstAsync(p => p.TxnId == txnId);

                purchase.StatusCode = PurchaseStatus.Anulado.Code;

                var invTxn = await Context.IAWInventoryTxn
                    .FirstAsync(t => t.ModOrigin == "PCM" && t.TxnOrigin == txnId);

                await _invTxnRepository.AnularTxn(invTxn.TxnId);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return purchase;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static string MapPurchaseTypeToInv(string purchaseType)
        {
            return purchaseType switch
            {
                var t when t == PurchaseType.Ingreso.Code => InvType.Ingreso.Code,
                var t when t == PurchaseType.TxnInicial.Code => InvType.TxnInicial.Code,
                _ => throw new ControllerException("Tipo de compra no soportado para inventario")
            };
        }

        private EInventoryTxn BuildInventoryTxnFromPurchase(EPurchaseTxn purchase)
        {
            var invTxn = new EInventoryTxn
            {
                ModOrigin = "PCM",
                TxnOrigin = purchase.TxnId,
                Type = MapPurchaseTypeToInv(purchase.Type),
                WarehouseDestId = purchase.WarehouseId,
                StatusCode = purchase.StatusCode,
                Delete = false,
                InvDetails = purchase.Details.Select(d => new EInvDetail
                {
                    NroProduct = d.NroProduct,
                    PresentCode = d.PresentCode,
                    QtyPresent = d.QtyPresent,
                    QtyProduct = d.QtyProduct,
                }).ToList()
            };

            return invTxn;
        }


       

     
    }
}
