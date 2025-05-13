using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.CMS;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<EModule> CMSModules { get; set; }
        public DbSet<EOptionMenu> CMSOptionMenus { get; set; }
        public DbSet<EGroup> CMSGroups { get; set; }
        public DbSet<EGroupOption> CMSGroupOptions { get; set; }

        private static void CMSModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EModule>(entity =>
            {
                entity.ToTable("CMS_Module");
                entity.HasKey(e => e.CodModule);
            });

            modelBuilder.Entity<EOptionMenu>(entity =>
            {
                entity.ToTable("CMS_OptionMenu");
                entity.HasKey(e => e.CodOption);

                entity.HasOne(e => e.OptionMenu)
                      .WithMany(e => e.OptionMenus)
                      .HasForeignKey(e => e.CodGruOption)
                      .HasPrincipalKey(e => e.CodOption)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<EGroup>(entity =>
            {
                entity.ToTable("CMS_Group");
                entity.HasKey(e => e.NroGroup);

                entity.Property(e => e.NroGroup)
                        .ValueGeneratedOnAdd(); 

                entity.HasOne(d => d.Module)
                        .WithMany(p => p.Groups)
                        .HasForeignKey(d => d.CodModule)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EGroupOption>(entity =>
            {
                entity.ToTable("CMS_GroupOption");
                entity.HasKey(e => new { e.NroGroup, e.CodOption });
                entity.HasOne(d => d.Group)
                        .WithMany(p => p.GroupOptions)
                        .HasForeignKey(d => d.NroGroup)
                        .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.OptionMenu)
                        .WithMany(p => p.GroupOptions)
                        .HasForeignKey(d => d.CodOption)
                        .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
