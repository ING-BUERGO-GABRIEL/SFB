﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
