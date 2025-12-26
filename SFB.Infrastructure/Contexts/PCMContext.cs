using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.PCM;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {

        public DbSet<EPurchaseTxn> PCMPurchaseTxn { get; set; }
        public DbSet<EPurchaseDetail> PCMPurchaseDetail { get; set; }

        public DbSet<ESupplier> PCMSupplier { get; set; }


        private static void PCMModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EPurchaseTxn>(entity =>
            {
                entity.ToTable("PCM_PurchaseTxn");
                entity.HasKey(e => e.TxnId);

            });

            modelBuilder.Entity<EPurchaseDetail>(entity =>
            {
                entity.ToTable("PCM_PurchaseDetail");
                entity.HasKey(e => e.DetailId);

            });

            modelBuilder.Entity<ESupplier>(entity =>
            {
                entity.ToTable("PCM_Supplier");
                entity.HasKey(e => e.SupplierId);


            });


        }
    }
}
