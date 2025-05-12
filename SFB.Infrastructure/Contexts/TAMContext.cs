using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.TAM;

namespace SFB.Infrastructure.Contexts
{
    public partial class SGDContext
    {
        public DbSet<ETeacher> TAMTeacher { get; set; }
        public DbSet<EFormTeacher> TAMFormTeachers { get; set; }
        public DbSet<ETeacherTask> TAMTeacherTasks { get; set; }

        private static void TAMModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ETeacher>(entity =>
            {
                entity.ToTable("TAM_Teacher");
                entity.HasKey(e => e.NroTeacher);
            });

            modelBuilder.Entity<EFormTeacher>(entity =>
            {
                entity.ToTable("TAM_FormTeacher");
                entity.HasKey(e => e.NroForm);
            });

            modelBuilder.Entity<ETeacherTask>(entity =>
            {
                entity.ToTable("TAM_TeacherTask");
                entity.HasKey(e => e.NroTask);

                entity.HasOne(d => d.PersonAssigned)
                        .WithMany(p => p.TeacherTasksAssigned)
                        .HasForeignKey(d => d.NroPersonAssigned)
                        .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.PersonReturn)
                        .WithMany(p => p.TeacherTasksReturned)
                        .HasForeignKey(d => d.NroPersonReturn)
                        .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.FormTeacher)
                        .WithMany(p => p.TeacherTask)
                        .HasForeignKey(d => d.NroFromTeacher)
                        .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
