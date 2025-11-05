using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class StockRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string>();
        }

        internal async Task<PagedListModel<EStock>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.IAWStocks.AsQueryable();

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "WarehouseId" });

            return result;
        }


        internal async Task RevertFromTxn(EInventoryTxn txn)
        {

            if (txn.InvDetails is null || txn.InvDetails.Count == 0)
                return; // Nada que revertir

            // Sumar cantidades por producto (maneja líneas duplicadas)
            var detailSums = txn.InvDetails
                .GroupBy(d => d.NroProduct)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.QtyProduct));

            var productIds = detailSums.Keys.ToList();

            var originId = txn.WarehouseOriginId ?? 0;
            var destId = txn.WarehouseDestId ?? 0;

            // Nombres de productos para mensajes
            var productNames = await Context.IAWProducts.AsNoTracking()
                .Where(p => productIds.Contains(p.NroProduct))
                .ToDictionaryAsync(p => p.NroProduct, p => p.Name);

            switch (txn.Type)
            {
                case "INI":
                    {
                        // Reverso de INI: restar en DESTINO lo que se cargó inicialmente
                        if (!txn.WarehouseDestId.HasValue)
                            throw new ControllerException("Almacén destino requerido para revertir INI.");

                        var stocksDest = await Context.IAWStocks
                            .Where(s => s.WarehouseId == destId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var missing = productIds.Except(stocksDest.Select(s => s.NroProduct)).ToList();
                        if (missing.Any())
                        {
                            var names = string.Join(", ", missing.Select(id => productNames.TryGetValue(id, out var n) ? n : id.ToString()));
                            throw new ControllerException($"No existe stock en almacen destino para revertir INI de: {names}");
                        }

                        foreach (var st in stocksDest)
                        {
                            var qty = detailSums[st.NroProduct];
                            if (st.QtyOnHand < qty)
                            {
                                var name = productNames.TryGetValue(st.NroProduct, out var n) ? n : st.NroProduct.ToString();
                                throw new ControllerException($"Stock insuficiente en destino para revertir INI de: {name}");
                            }

                            st.QtyOnHand -= qty;
                            st.UserUpd = "SYSTEM";
                            st.DateUpd = DateTime.UtcNow;
                            Context.IAWStocks.Update(st);
                        }
                    }
                    break;

                case "ING":
                    {
                        // Reverso de ING: restar en DESTINO lo que se ingresó
                        if (!txn.WarehouseDestId.HasValue)
                            throw new ControllerException("Almacén destino requerido para revertir ING.");

                        var stocksDest = await Context.IAWStocks
                            .Where(s => s.WarehouseId == destId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var missing = productIds.Except(stocksDest.Select(s => s.NroProduct)).ToList();
                        if (missing.Any())
                        {
                            var names = string.Join(", ", missing.Select(id => productNames.TryGetValue(id, out var n) ? n : id.ToString()));
                            throw new ControllerException($"No existe stock en destino para revertir ingreso de: {names}");
                        }

                        foreach (var st in stocksDest)
                        {
                            var qty = detailSums[st.NroProduct];
                            if (st.QtyOnHand < qty)
                            {
                                var name = productNames.TryGetValue(st.NroProduct, out var n) ? n : st.NroProduct.ToString();
                                throw new ControllerException($"Stock insuficiente en destino para revertir ingreso de: {name}");
                            }

                            st.QtyOnHand -= qty;
                            st.UserUpd = "SYSTEM";
                            st.DateUpd = DateTime.UtcNow;
                            Context.IAWStocks.Update(st);
                        }
                    }
                    break;

                case "SAL":
                    {
                        // Reverso de SAL: sumar en ORIGEN lo que se descontó (crear si no existe)
                        if (!txn.WarehouseOriginId.HasValue)
                            throw new ControllerException("Almacén origen requerido para revertir SAL.");

                        var stocksOrigin = await Context.IAWStocks
                            .Where(s => s.WarehouseId == originId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var originMap = stocksOrigin.ToDictionary(s => s.NroProduct);

                        foreach (var kv in detailSums)
                        {
                            var prodId = kv.Key;
                            var qty = kv.Value;

                            if (originMap.TryGetValue(prodId, out var st))
                            {
                                st.QtyOnHand += qty;
                                st.UserUpd = "SYSTEM";
                                st.DateUpd = DateTime.UtcNow;
                                Context.IAWStocks.Update(st);
                            }
                            else
                            {
                                var created = new EStock
                                {
                                    NroProduct = prodId,
                                    WarehouseId = originId,
                                    QtyOnHand = qty,
                                    QtyReserved = 0m,
                                    UserUpd = "SYSTEM",
                                    DateUpd = DateTime.UtcNow
                                };
                                Context.IAWStocks.Add(created);
                            }
                        }
                    }
                    break;

                case "TRA":
                    {
                        // Reverso de TRA: sumar en ORIGEN y restar en DESTINO
                        if (!txn.WarehouseOriginId.HasValue)
                            throw new ControllerException("Almacén origen requerido para revertir TRA.");
                        if (!txn.WarehouseDestId.HasValue)
                            throw new ControllerException("Almacén destino requerido para revertir TRA.");
                        if (originId == destId)
                            throw new ControllerException("Almacén origen y destino no pueden ser el mismo.");

                        var stocksOrigin = await Context.IAWStocks
                            .Where(s => s.WarehouseId == originId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var stocksDest = await Context.IAWStocks
                            .Where(s => s.WarehouseId == destId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        // Para restar en destino, deben existir
                        var missingDest = productIds.Except(stocksDest.Select(s => s.NroProduct)).ToList();
                        if (missingDest.Any())
                        {
                            var names = string.Join(", ", missingDest.Select(id => productNames.TryGetValue(id, out var n) ? n : id.ToString()));
                            throw new ControllerException($"No existe stock en destino para revertir traspaso de: {names}");
                        }

                        // Sumar en origen (crear si no existe)
                        var originMap = stocksOrigin.ToDictionary(s => s.NroProduct);
                        foreach (var kv in detailSums)
                        {
                            var prodId = kv.Key;
                            var qty = kv.Value;

                            if (originMap.TryGetValue(prodId, out var st))
                            {
                                st.QtyOnHand += qty;
                                st.UserUpd = "SYSTEM";
                                st.DateUpd = DateTime.UtcNow;
                                Context.IAWStocks.Update(st);
                            }
                            else
                            {
                                var created = new EStock
                                {
                                    NroProduct = prodId,
                                    WarehouseId = originId,
                                    QtyOnHand = qty,
                                    QtyReserved = 0m,
                                    UserUpd = "SYSTEM",
                                    DateUpd = DateTime.UtcNow
                                };
                                Context.IAWStocks.Add(created);
                            }
                        }

                        // Restar en destino (validando suficiencia)
                        foreach (var st in stocksDest)
                        {
                            var qty = detailSums[st.NroProduct];
                            if (st.QtyOnHand < qty)
                            {
                                var name = productNames.TryGetValue(st.NroProduct, out var n) ? n : st.NroProduct.ToString();
                                throw new ControllerException($"Stock insuficiente en destino para revertir traspaso de: {name}");
                            }

                            st.QtyOnHand -= qty;
                            st.UserUpd = "SYSTEM";
                            st.DateUpd = DateTime.UtcNow;
                            Context.IAWStocks.Update(st);
                        }
                    }
                    break;

                default:
                    throw new ControllerException("Tipo de transacción no manejado para reversión.");
            }

            // Sin SaveChanges / transacciones: lo maneja la capa superior.
        }


        internal async Task UpdateFromTxn(EInventoryTxn invTxn)
        {
            if (invTxn.InvDetails is null || invTxn.InvDetails.Count == 0)
                return;

            // Agrupar cantidades por producto (evita problemas si hay líneas duplicadas)
            var detailSums = invTxn.InvDetails
                .GroupBy(d => d.NroProduct)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.QtyProduct));

            var productIds = detailSums.Keys.ToList();

            var originId = invTxn.WarehouseOriginId ?? 0;
            var destId = invTxn.WarehouseDestId ?? 0;

            // Obtener nombres para mensajes
            var productNames = await Context.IAWProducts.AsNoTracking()
                .Where(p => productIds.Contains(p.NroProduct))
                .ToDictionaryAsync(p => p.NroProduct, p => p.Name);

            switch (invTxn.Type)
            {
                case "INI":
                    {
                        if (!invTxn.WarehouseDestId.HasValue)
                            throw new ControllerException("Almacén destino requerido para transacción inicial.");

                        // Crear registros de stock
                        var newStocks = detailSums.Select(kv => new EStock
                        {
                            NroProduct = kv.Key,
                            WarehouseId = destId,
                            QtyOnHand = kv.Value,
                            QtyReserved = 0m,
                        }).ToList();

                        Context.IAWStocks.AddRange(newStocks);
                    }
                    break;

                case "ING":
                    {
                        if (!invTxn.WarehouseDestId.HasValue)
                            throw new ControllerException("Almacén destino requerido para ingreso.");

                        var stocks = await Context.IAWStocks
                            .Where(s => s.WarehouseId == destId
                            && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        foreach (var st in stocks)
                        {
                            if (detailSums.TryGetValue(st.NroProduct, out var qty))
                            {
                                st.QtyOnHand += qty;
                                Context.IAWStocks.Update(st);
                            }
                        }
                    }
                    break;
                case "SAL":
                    {
                        if (!invTxn.WarehouseOriginId.HasValue)
                            throw new ControllerException("Almacén origen requerido para salida.");

                        var stocks = await Context.IAWStocks
                            .Where(s => s.WarehouseId == originId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var missing = productIds.Except(stocks.Select(s => s.NroProduct)).ToList();

                        if (missing.Any())
                        {
                            var names = missing.Select(id => productNames.TryGetValue(id, out var n) ? n : id.ToString());
                            throw new ControllerException($"No existe stock en origen para los productos: {string.Join(", ", names)}");
                        }

                        // Aplicar la resta
                        foreach (var st in stocks)
                        {
                            var qty = detailSums[st.NroProduct];

                            if (st.QtyOnHand < qty)
                                throw new ControllerException($"Stock insuficiente para: {productNames[st.NroProduct]}");

                            st.QtyOnHand -= qty;
                            st.UserUpd = "SYSTEM";
                            st.DateUpd = DateTime.UtcNow;
                            Context.IAWStocks.Update(st);
                        }
                    }
                    break;

                case "TRA":
                    {
                        if (!invTxn.WarehouseOriginId.HasValue)
                            throw new ControllerException("Almacén origen requerido para traspaso.");
                        if (!invTxn.WarehouseDestId.HasValue)
                            throw new ControllerException("Almacén destino requerido para traspaso.");


                        if (originId == destId)
                            throw new ControllerException("Almacén origen y destino no pueden ser el mismo.");

                        // Obtener stocks en origen y destino
                        var stocksOrigin = await Context.IAWStocks
                            .Where(s => s.WarehouseId == originId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var stocksDest = await Context.IAWStocks
                            .Where(s => s.WarehouseId == destId && productIds.Contains(s.NroProduct))
                            .ToListAsync();

                        var missing = productIds.Except(stocksOrigin.Select(s => s.NroProduct)).ToList();
                        if (missing.Any())
                        {
                            var names = missing.Select(id => productNames.TryGetValue(id, out var n) ? n : id.ToString());
                            throw new ControllerException($"No existe stock en origen para los productos: {string.Join(", ", names)}");
                        }

                        // Restar en origen
                        foreach (var st in stocksOrigin)
                        {
                            var qty = detailSums[st.NroProduct];

                            if (st.QtyOnHand < qty)
                                throw new ControllerException($"Stock insuficiente en origen para: {productNames[st.NroProduct]}");

                            st.QtyOnHand -= qty;
                            Context.IAWStocks.Update(st);
                        }

                        // Sumar en destino (crear si hace falta)
                        var destMap = stocksDest.ToDictionary(s => s.NroProduct);

                        foreach (var kv in detailSums)
                        {
                            var prodId = kv.Key;
                            var qty = kv.Value;
                            if (destMap.TryGetValue(prodId, out var destStock))
                            {
                                destStock.QtyOnHand += qty;
                                Context.IAWStocks.Update(destStock);
                            }
                            else
                            {
                                var created = new EStock
                                {
                                    NroProduct = prodId,
                                    WarehouseId = destId,
                                    QtyOnHand = qty,
                                    QtyReserved = 0m,
                                };
                                Context.IAWStocks.Add(created);
                            }
                        }
                    }
                    break;

                default:
                    throw new ControllerException("Tipo de transacción no manejado.");
            }
        }
    }
}
