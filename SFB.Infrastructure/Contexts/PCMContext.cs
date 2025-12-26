using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.PCM;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {

        public DbSet<EPurchaseTxn> PCMPurchaseTxn { get; set; }
        public DbSet<EPurDetail> PCMPurDetail { get; set; }
        public DbSet<ESupplier> PCMSupplier { get; set; }


        private static void PCMModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EPurchaseTxn>(entity =>
            {
                entity.ToTable("PCM_PurchaseTxn");
                entity.HasKey(e => e.TxnId);

                //entity.HasMany(e => e.InvDetails)
                //        .WithOne(d => d.InventoryTxn)
                //        .HasForeignKey(d => d.TxnId)
                //        .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<EPurDetail>(entity =>
            {
                entity.ToTable("PCM_PurDetail");
                entity.HasKey(e => e.DetailId);

                //entity.HasOne(d => d.Product)
                //      .WithMany(p => p.InvDetails)
                //      .HasForeignKey(d => d.NroProduct)
                //      .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<ESupplier>(entity =>
            {
                entity.ToTable("PCM_Supplier");
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(70);
                entity.Property(e => e.Address).HasMaxLength(300);
            });

          
            //indices
            //modelBuilder.Entity<EInventoryTxn>().HasIndex(e => e.WarehouseOriginId);
            //modelBuilder.Entity<EInventoryTxn>().HasIndex(e => e.WarehouseDestId);
            //modelBuilder.Entity<EInvDetail>().HasIndex(d => d.TxnId);
            //modelBuilder.Entity<EInvDetail>().HasIndex(d => d.NroProduct);

        }
    }
}
