using Microsoft.EntityFrameworkCore;
using SFB.IAW.Backend.Repositories;
using SFB.IAW.Shared.Sealed;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Infrastructure.Entities.SOM;
using SFB.Infrastructure.Entities.TRM;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;
using SFB.SOM.Shared.Sealed;
using SFB.TRM.Backend.Repositories;
using SFB.TRM.Shared.Models;
using SFB.TRM.Shared.Sealed;

namespace SFB.SOM.Backend.Repositories
{
    public class SalesTxnRepositorie(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        private readonly InventoryTxnRepository _invTxnRepository = new InventoryTxnRepository(context);
        private readonly SalesSettingsRepositorie _salSettsRepository = new SalesSettingsRepositorie(context);
        private readonly TreasuryTxnRepository _treasuryTxnRepository = new TreasuryTxnRepository(context);
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Reference", "Customer.Person.Name" };
        }

        internal async Task<PagedListModel<ESalesTxn>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.SMOSalesTxn
                .Where(s => !s.Delete)
                .Include(s => s.Customer)
                    .ThenInclude(c => c.Person)
                .Include(s => s.Warehouse);

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "TxnId" });

            return result;
        }

        internal async Task<ESalesTxn> Create(ESalesTxn sales)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var salesTxn = await CreateTxn(sales);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return salesTxn;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        internal async Task<ESalesTxn> CreateTxn(ESalesTxn sales)
        {
            sales.GrandTotal = sales.Details?.Sum(d => d.TotalPrice) ?? 0m;

            // Evitar que EF intente insertar entidades relacionadas que vienen desde el cliente.
            if (sales.Details != null)
            {
                foreach (var d in sales.Details)
                {
                    // El cliente envía el Product completo; aseguramos que EF no intente insertarlo.
                    d.Product = null;
                }
            }

            if (sales.PaymentMethods != null)
            {
                foreach (var pm in sales.PaymentMethods)
                {
                    // Evitar inserción accidental de la navegación inversa
                    pm.SalesTxn = null;
                }
            }

            Context.SMOSalesTxn.Add(sales);
            await Context.SaveChangesAsync();

            var invTxn = BuildInventoryTxnFromSales(sales);
            await _invTxnRepository.CreateTxn(invTxn);

            var cashBoxId = await _salSettsRepository.GetDefaultCashBoxId();
            var treasuryTxn = BuildTreasuryTxnFromSales(sales, cashBoxId.Value);
            await _treasuryTxnRepository.CreateTxn(treasuryTxn);

            return sales;
        }



        internal async Task<ESalesTxn?> GetById(int txnId)
        {
            return await Context.SMOSalesTxn.AsNoTracking()
                .Include(s => s.Customer)
                    .ThenInclude(c => c.Person)
                .Include(s => s.Warehouse)
                .Include(s => s.Details)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(s => s.TxnId == txnId);
        }

        internal async Task<Dictionary<string, object>> GetMetadata()
        {
            var warehouseId = await _salSettsRepository.GetDefaultWarehouseId();
            var customerId = await _salSettsRepository.GetDefaultCustomerId();

            var cmbCustomers = await Context.AMSCustomer
                .Where(c=>c.CustomerId == customerId)
                .Include(c => c.Person)
                .AsNoTracking()
                .ToListAsync();

            var cmbWarehouses = await Context.IAWWarehouse
                .Where(w => w.Status)
                .AsNoTracking()
                .ToListAsync();

            return new Dictionary<string, object>
            {
                {"DefaultWarehouseId", warehouseId},
                {"DefaultCustomerId", customerId},
                {"CmbCustomers", cmbCustomers},
                {"CmbWarehouses", cmbWarehouses},
                {"CmbStatus", SalesStatus.List()},
                {"CmbType", new [] { SalesType.Venta }},
            };
        }
       

        internal async Task<ESalesTxn> Anular(int txnId)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var sales = await Context.SMOSalesTxn
                    .Include(s => s.Details)
                    .FirstAsync(s => s.TxnId == txnId);

                sales.StatusCode = SalesStatus.Anulado.Code;

                var invTxn = await Context.IAWInventoryTxn
                    .FirstAsync(t => t.ModOrigin == "SOM" && t.TxnOrigin == txnId);

                await _invTxnRepository.AnularTxn(invTxn.TxnId);

                await _treasuryTxnRepository.AnularByOrigin("SOM", txnId);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return sales;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static string MapSalesTypeToInv(string salesType)
        {
            return salesType switch
            {
                var t when t == SalesType.Venta.Code => InvType.Salida.Code,
                _ => throw new ControllerException("Tipo de venta no soportado para inventario"),
            };
        }

        private EInventoryTxn BuildInventoryTxnFromSales(ESalesTxn sales)
        {
            var invTxn = new EInventoryTxn
            {
                ModOrigin = "SOM",
                TxnOrigin = sales.TxnId,
                Type = MapSalesTypeToInv(sales.Type),
                WarehouseOriginId = sales.WarehouseId,
                StatusCode = sales.StatusCode,
                Delete = false,
                InvDetails = sales.Details.Select(d => new EInvDetail
                {
                    NroProduct = d.NroProduct,
                    PresentCode = d.PresentCode,
                    QtyPresent = d.QtyPresent,
                    QtyProduct = d.QtyProduct,
                }).ToList()
            };

            return invTxn;
        }

        private static ETreasuryTxn BuildTreasuryTxnFromSales(ESalesTxn sales,int cashBoxId)
        {

            var paymentMethods = sales.PaymentMethods;

            if (paymentMethods is null || paymentMethods.Count == 0)            
                throw new ControllerException("Debe registrar al menos un método de pago para la venta.");           


            var details = paymentMethods.Select(detail => new ETreasuryDetail
            {
                PaymentMethodCode = detail.PaymentMethodCode,
                Amount = detail.Amount,
                PaymentRef = detail.PaymentRef
            }).ToList();

            var totalPayments = details.Sum(d => d.Amount);

            if (totalPayments != sales.GrandTotal)            
                throw new ControllerException("El total de pagos no coincide con el total de la venta.");       

            return new ETreasuryTxn
            {
                ModOrigin = "SOM",
                TxnOrigin = sales.TxnId,
                Type = TreasuryType.Ingreso.Code,
                TxnDate = DateTime.UtcNow,
                CashBoxDestId = cashBoxId,
                StatusCode = TreasuryStatus.Pendiente.Code,
                CurrencyCode = sales.CurrencyCode,
                GrandTotal = totalPayments,
                Reference = sales.Reference,
                Details = details
            };
        }
    }
}
