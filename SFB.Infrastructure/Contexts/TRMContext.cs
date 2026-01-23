using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.SOM;
using SFB.Infrastructure.Entities.TRM;


namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<ECashBox> TRMCashBoxs { get; set; }
        public DbSet<ETreasuryTxn> TRMTreasuryTxn { get; set; }
        public DbSet<ETreasuryDetail> TRMTreasuryDetail { get; set; }

        private static void TRMModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ECashBox>(entity =>
            {
                entity.ToTable("TRM_CashBox");
                entity.HasKey(e => new { e.CashBoxId });
            });

            modelBuilder.Entity<ETreasuryTxn>(entity =>
            {
                entity.ToTable("TRM_TreasuryTxn");
                entity.HasKey(e => new { e.TxnId });
            });
            modelBuilder.Entity<ETreasuryDetail>(entity =>
            {
                entity.ToTable("TRM_TreasuryDetail");
                entity.HasKey(e => new { e.DetailId });
            });

        }
    }
}
