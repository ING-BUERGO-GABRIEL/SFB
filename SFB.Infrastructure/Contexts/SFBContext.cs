using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Interfaces;
using System.Security.Claims;

namespace SFB.Infrastructure.Contexts
{
    public partial class SFBContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SFBContext(DbContextOptions<SFBContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configuraciones de conexión adicionales (si es necesario)
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AMSModelCreating(modelBuilder);
            CMSModelCreating(modelBuilder);
            IAWModelCreating(modelBuilder);
            PCMModelCreating(modelBuilder);
            SOMModelCreating(modelBuilder);
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private string? _cachedUser;

        private string GetCurrentUser()
        {
            if (!string.IsNullOrWhiteSpace(_cachedUser))
                return _cachedUser;

            var user = _httpContextAccessor?.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                _cachedUser = user.FindFirst("NameUser")?.Value;
            }

            _cachedUser ??= "system"; 
            return _cachedUser;
        }

        private void ApplyAuditInfo()
        {
            var now = DateTime.UtcNow; 
            var user = GetCurrentUser();

            // Todas las entradas que implementen IAuditable
            var auditableEntries = ChangeTracker.Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .ToList();

            foreach (var entry in auditableEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    // Si el llamador ya asignó algo, no lo pisamos (pero normalmente queremos setearlo)
                    if (string.IsNullOrWhiteSpace(entry.Entity.UserReg))
                        entry.Entity.UserReg = user;

                    if (entry.Entity.DateReg == default)
                        entry.Entity.DateReg = now;

                    // Proteger campos de creación para que no se sobreescriban por accidente
                    entry.Property(x => x.UserReg).IsModified = false;
                    entry.Property(x => x.DateReg).IsModified = false;

                    // Asegura que no queden basuras en Upd
                    entry.Entity.UserUpd = null;
                    entry.Entity.DateUpd = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Preserva campos de creación
                    entry.Property(x => x.UserReg).IsModified = false;
                    entry.Property(x => x.DateReg).IsModified = false;

                    // ¿Realmente hubo cambios “de negocio”?
                    var hasRealChanges = entry.Properties.Any(p =>
                        p.IsModified &&
                        p.Metadata.Name != nameof(IAuditable.UserReg) &&
                        p.Metadata.Name != nameof(IAuditable.DateReg) &&
                        p.Metadata.Name != nameof(IAuditable.UserUpd) &&
                        p.Metadata.Name != nameof(IAuditable.DateUpd)
                    );

                    if (hasRealChanges)
                    {
                        entry.Entity.UserUpd = user;
                        entry.Entity.DateUpd = now;

                        // Evita loops marcando solo Upd como modificado si fuera lo único cambiado
                        entry.Property(x => x.UserUpd).IsModified = true;
                        entry.Property(x => x.DateUpd).IsModified = true;
                    }
                    else
                    {
                        entry.State = EntityState.Unchanged;
                    }
                }
            }
        }

    }
}
