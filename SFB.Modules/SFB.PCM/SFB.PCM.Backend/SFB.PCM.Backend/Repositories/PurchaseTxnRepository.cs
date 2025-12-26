using Microsoft.EntityFrameworkCore;
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

                await ValidateTxnInit(invTxn.InvDetails, invTxn.Type);
                await UpdateStockFromTxn(invTxn);

                Context.IAWInventoryTxn.Add(invTxn);

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

        internal async Task<EPurchaseTxn> Update(EPurchaseTxn purchase)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var dbPurchase = await Context.PCMPurchaseTxn
                    .Include(p => p.Details)
                    .FirstAsync(p => p.TxnId == purchase.TxnId);

                if (dbPurchase.StatusCode == PurchaseStatus.Anulado.Code)
                {
                    throw new ControllerException("No se puede actualizar una compra anulada.");
                }

                var invTxn = await Context.IAWInventoryTxn
                    .Include(t => t.InvDetails)
                    .FirstOrDefaultAsync(t => t.ModOrigin == "PCM" && t.TxnOrigin == purchase.TxnId);

                if (invTxn is null)
                {
                    throw new ControllerException("No se encontró la transacción de inventario asociada.");
                }

                await RevertStockFromTxn(invTxn);

                dbPurchase.SupplierId = purchase.SupplierId;
                dbPurchase.WarehouseId = purchase.WarehouseId;
                dbPurchase.CurrencyCode = purchase.CurrencyCode;
                dbPurchase.Type = purchase.Type;
                dbPurchase.Reference = purchase.Reference;
                dbPurchase.StatusCode = purchase.StatusCode;
                dbPurchase.GrandTotal = purchase.Details?.Sum(d => d.TotalCost) ?? 0m;

                Context.PCMPurchaseDetail.RemoveRange(dbPurchase.Details);
                dbPurchase.Details = purchase.Details;

                invTxn.Type = MapPurchaseTypeToInv(purchase.Type);
                invTxn.WarehouseDestId = purchase.WarehouseId;
                invTxn.StatusCode = purchase.StatusCode;

                Context.IAWInvDetail.RemoveRange(invTxn.InvDetails);
                invTxn.InvDetails = purchase.Details.Select(d => new EInvDetail
                {
                    NroProduct = d.NroProduct,
                    PresentCode = d.PresentCode,
                    QtyPresent = d.QtyPresent,
                    QtyProduct = d.QtyProduct,
                }).ToList();

                await ValidateTxnInit(invTxn.InvDetails, invTxn.Type);
                await UpdateStockFromTxn(invTxn);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return dbPurchase;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
                    .Include(t => t.InvDetails)
                    .FirstOrDefaultAsync(t => t.ModOrigin == "PCM" && t.TxnOrigin == txnId);

                if (invTxn != null)
                {
                    await RevertStockFromTxn(invTxn);
                    invTxn.StatusCode = InvStatus.Anulado.Code;
                    Context.IAWInventoryTxn.Update(invTxn);
                }

                Context.PCMPurchaseTxn.Update(purchase);

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

        internal async Task ValidateTxnInit(ICollection<EInvDetail> invDetail, string type)
        {
            if (invDetail is null || invDetail.Count == 0)
                return;

            var productIds = invDetail.Select(d => d.NroProduct).Distinct().ToList();

            var initialProductIds = await Context.IAWInvDetail.AsNoTracking()
                .Where(d => productIds.Contains(d.NroProduct)
                            && d.InventoryTxn != null
                            && d.InventoryTxn.Type == InvType.TxnInicial.Code
                            && d.InventoryTxn.StatusCode == InvStatus.Activo.Code
                            && !d.InventoryTxn.Delete)
                .Select(d => d.NroProduct)
                .Distinct()
                .ToListAsync();

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

        private async Task UpdateStockFromTxn(EInventoryTxn invTxn)
        {
            if (invTxn.InvDetails is null || invTxn.InvDetails.Count == 0)
                return;

            var detailSums = invTxn.InvDetails
                .GroupBy(d => d.NroProduct)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.QtyProduct));

            var productIds = detailSums.Keys.ToList();
            var destId = invTxn.WarehouseDestId ?? 0;

            switch (invTxn.Type)
            {
                case "INI":
                    if (!invTxn.WarehouseDestId.HasValue)
                        throw new ControllerException("Almacén destino requerido para transacción inicial.");

                    var newStocks = detailSums.Select(kv => new EStock
                    {
                        NroProduct = kv.Key,
                        WarehouseId = destId,
                        QtyOnHand = kv.Value,
                        QtyReserved = 0m,
                    }).ToList();

                    Context.IAWStocks.AddRange(newStocks);
                    break;
                case "ING":
                    if (!invTxn.WarehouseDestId.HasValue)
                        throw new ControllerException("Almacén destino requerido para ingreso.");

                    var existing = await Context.IAWStocks
                        .Where(s => s.WarehouseId == destId && productIds.Contains(s.NroProduct))
                        .ToDictionaryAsync(s => s.NroProduct);

                    foreach (var kv in detailSums)
                    {
                        if (existing.TryGetValue(kv.Key, out var st))
                        {
                            st.QtyOnHand += kv.Value;
                            Context.IAWStocks.Update(st);
                        }
                        else
                        {
                            var created = new EStock
                            {
                                NroProduct = kv.Key,
                                WarehouseId = destId,
                                QtyOnHand = kv.Value,
                                QtyReserved = 0m,
                            };
                            Context.IAWStocks.Add(created);
                        }
                    }
                    break;
                default:
                    throw new ControllerException("Tipo de transacción de inventario no soportado para compras.");
            }
        }

        private async Task RevertStockFromTxn(EInventoryTxn txn)
        {
            if (txn.InvDetails is null || txn.InvDetails.Count == 0)
                return;

            var detailSums = txn.InvDetails
                .GroupBy(d => d.NroProduct)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.QtyProduct));

            var productIds = detailSums.Keys.ToList();
            var destId = txn.WarehouseDestId ?? 0;

            var stocksDest = await Context.IAWStocks
                .Where(s => s.WarehouseId == destId && productIds.Contains(s.NroProduct))
                .ToListAsync();

            var missing = productIds.Except(stocksDest.Select(s => s.NroProduct)).ToList();
            if (missing.Any())
            {
                throw new ControllerException("No existe stock en destino para revertir la compra.");
            }

            foreach (var st in stocksDest)
            {
                var qty = detailSums[st.NroProduct];
                if (st.QtyOnHand < qty)
                {
                    throw new ControllerException("Stock insuficiente para revertir la compra.");
                }

                st.QtyOnHand -= qty;
                Context.IAWStocks.Update(st);
            }
        }
    }
}
