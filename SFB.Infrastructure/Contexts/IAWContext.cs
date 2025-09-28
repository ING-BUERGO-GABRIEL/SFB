using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.IAW;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<EProduct> IAWProducts { get; set; }

        public DbSet<EWarehouse> IAWWarehouse { get; set; }

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

        }
    }
}
