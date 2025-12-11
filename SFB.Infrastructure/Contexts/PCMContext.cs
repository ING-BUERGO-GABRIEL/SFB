using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.PCM;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {

        public DbSet<EPurchaseTxn> PCMPurchaseTxn { get; set; }
        public DbSet<EPurDetail> PCMPurDetail { get; set; }


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

          
            //indices
            //modelBuilder.Entity<EInventoryTxn>().HasIndex(e => e.WarehouseOriginId);
            //modelBuilder.Entity<EInventoryTxn>().HasIndex(e => e.WarehouseDestId);
            //modelBuilder.Entity<EInvDetail>().HasIndex(d => d.TxnId);
            //modelBuilder.Entity<EInvDetail>().HasIndex(d => d.NroProduct);

        }
    }
}
