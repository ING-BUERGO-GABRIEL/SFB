using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.SOM;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<ESalesTxn> SMOSalesTxn { get; set; }
        public DbSet<ESalesDetail> SMOSalesDetail { get; set; }
        public DbSet<ESalesSettings> SMOSalesSettings { get; set; }
        public DbSet<EPaymentMethod> SMOPaymentMethods { get; set; }

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

            modelBuilder.Entity<ESalesSettings>(entity =>
            {
                entity.ToTable("SOM_SalesSettings");
                entity.HasKey(x => new { x.SettingId });
                entity.HasOne(x => x.Group)
                    .WithMany(x => x.Settings)
                    .HasForeignKey(x => new { x.SettingsGroup })
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EPaymentMethod>(entity =>
            {
                entity.ToTable("SOM_PaymentMethod");
                entity.HasKey(e => new { e.PaymentId });
            });
        }
    }
}
