using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.AMS;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext
    {
        public DbSet<EUser> AMSUsers { get; set; }
        public DbSet<EUserGroup> AMSUserGroup { get; set; }
        public DbSet<EPerson> AMSPerson { get; set; }
        public DbSet<ETypePerson> AMSTypePerson { get; set; }

        private static void AMSModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EUser>(entity =>
            {
                entity.ToTable("AMS_User");
                entity.HasKey(e => e.NameUser);

                entity.HasOne(d => d.Person)
                        .WithMany(p => p.Users)
                        .HasForeignKey(d => d.NroPerson)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EUserGroup>(entity =>
            {
                entity.ToTable("AMS_UserGroup");
                entity.HasKey(e => new { e.NameUser, e.NroGroup });

                entity.HasOne(d => d.User)
                        .WithMany(p => p.UserGroups)
                        .HasForeignKey(d => d.NameUser)
                        .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Group)
                        .WithMany(p => p.UserGroups)
                        .HasForeignKey(d => d.NroGroup)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EPerson>(entity =>
            {
                entity.ToTable("AMS_Person");
                entity.HasKey(e => e.NroPerson);

            });

            modelBuilder.Entity<ETypePerson>(entity =>
            {
                entity.ToTable("AMS_TypePerson");
                entity.HasKey(e => new { e.NroPerson, e.Type });
            });

        }
    }
}
