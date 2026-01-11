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
        internal async Task<PagedListModel<EProduct>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.IAWProducts
                               .Where(p => p.Status)
                               .Select(p => new EProduct
                               {
                                   NroProduct = p.NroProduct,
                                   Name = p.Name,
                                   IsPurchases = p.IsPurchases,
                                   IsSales = p.IsSales,
                                   PresentCode = p.PresentCode,
                                   Price = p.Price,
                                   SerialNumber = p.SerialNumber,
                                   Presentation = p.Presentation,
                                   Stock = p.Stocks.Sum(s=>s.QtyOnHand),
                                   ProductPresent = p.ProductPresent
                                       .Select(pr => new EProductPresent
                                       {
                                           PresentCode = pr.PresentCode,
                                           ProductId = pr.ProductId,
                                           Price = pr.Price,
                                           QtyProduct = pr.QtyProduct,
                                           SerialNumber = pr.SerialNumber,
                                           Presentation = pr.Presentation
                                       }).ToList()
                               });

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "NroProduct" });
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
            await UpdProductPresent(product.NroProduct, product.ProductPresent);
            product.ProductPresent.Clear();
            Context.IAWProducts.Update(product);
            await Context.SaveChangesAsync();
            return product;
        }

        private async Task UpdProductPresent(int productId, ICollection<EProductPresent> newProductPresents)
        {
            var dbProductPresents = await Context.IAWProductPresent
                                                 .Where(p => p.ProductId == productId)
                                                 .ToListAsync();

            newProductPresents ??= new List<EProductPresent>();

            foreach (var dbItem in dbProductPresents)
            {
                bool stillExists = newProductPresents
                    .Any(p => p.PresentCode == dbItem.PresentCode && p.ProductId == productId);

                if (!stillExists)
                {
                    Context.IAWProductPresent.Remove(dbItem);
                }
            }

            foreach (var newItem in newProductPresents)
            {
                newItem.ProductId = productId;
                var existing = dbProductPresents
                    .FirstOrDefault(p => p.ProductId == productId && p.PresentCode == newItem.PresentCode);

                if (existing is null)
                {
                    Context.IAWProductPresent.Add(newItem);
                }
                else
                {
                    existing.Price = newItem.Price;
                    existing.QtyProduct = newItem.QtyProduct;
                    existing.SerialNumber = newItem.SerialNumber;
                }
            }
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
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    Context.Entry(entity).State = EntityState.Unchanged;
                    entity.Status = false;
                    await Context.SaveChangesAsync();
                    return true;
                }
                throw;
            }
        }

        internal async Task<Dictionary<string, object>> GetMetadata()
        {
            var cmbPresent = await Context.IAWPresentation
                .AsNoTracking()
                .ToListAsync();

            return new Dictionary<string, object>
            {
                {"CmbPresent",cmbPresent },
            };
        }

    }
}
