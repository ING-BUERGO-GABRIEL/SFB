using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.IAW;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<EProduct> IAWProducts { get; set; }
      
        private static void IAWModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EProduct>(entity =>
            {
                entity.ToTable("IAW_Product");
                entity.HasKey(e => e.NroProduct);
            });

        }
    }
}
