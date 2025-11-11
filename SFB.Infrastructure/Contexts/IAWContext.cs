using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.IAW;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<EProduct> IAWProducts { get; set; }
        public DbSet<EWarehouse> IAWWarehouse { get; set; }
        public DbSet<EStock> IAWStocks { get; set; }
        public DbSet<EInventoryTxn> IAWInventoryTxn { get; set; }
        public DbSet<EInvDetail> IAWInvDetail { get; set; }
        public DbSet<EPresentation> IAWUnit { get; set; }

        private static void IAWModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EProduct>(entity =>
            {
                entity.ToTable("IAW_Product");
                entity.HasKey(e => e.NroProduct);
            });

            modelBuilder.Entity<EWarehouse>(entity =>
            {
                entity.ToTable("IAW_Warehouse");
                entity.HasKey(e => e.WarehouseId);
            });

            modelBuilder.Entity<EInventoryTxn>(entity =>
            {
                entity.ToTable("IAW_InventoryTxn");
                entity.HasKey(e => e.TxnId);

                entity.HasMany(e => e.InvDetails)
                        .WithOne(d => d.InventoryTxn)
                        .HasForeignKey(d => d.TxnId)
                        .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<EInvDetail>(entity =>
            {
                entity.ToTable("IAW_InvDetail");
                entity.HasKey(e => e.DetailId);

                entity.HasOne(d => d.Product)
                      .WithMany(p => p.InvDetails)
                      .HasForeignKey(d => d.NroProduct)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<EStock>(entity =>
            {
                entity.ToTable("IAW_Stock");
                entity.HasKey(e =>  new { e.NroProduct, e.WarehouseId });                
            });

            modelBuilder.Entity<EPresentation>(entity =>
            {
                entity.ToTable("IAW_Presentation");
                entity.HasKey(e => new { e.Code });
            });

            //indices
            modelBuilder.Entity<EInventoryTxn>().HasIndex(e => e.WarehouseOriginId);
            modelBuilder.Entity<EInventoryTxn>().HasIndex(e => e.WarehouseDestId);
            modelBuilder.Entity<EInvDetail>().HasIndex(d => d.TxnId);
            modelBuilder.Entity<EInvDetail>().HasIndex(d => d.NroProduct);

        }
    }
}
