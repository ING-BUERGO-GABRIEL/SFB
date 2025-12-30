using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.SOM;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<ESalesTxn> AMSSalesTxn { get; set; }
        public DbSet<ESalesDetail> AMSSalesDetail { get; set; }
        private static void SOMModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ESalesTxn>(entity =>
            {
                entity.ToTable("SOM_SalesTxn");
                entity.HasKey(e => new { e.TxnId });
            });

            modelBuilder.Entity<ESalesDetail>(entity =>
            {
                entity.ToTable("SOM_SalesDetail");
                entity.HasKey(e => new { e.DetailId });
            });

        }
    }
}
