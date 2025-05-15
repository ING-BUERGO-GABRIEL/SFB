using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SFB.Shared.Backend.Enums;
using SFB.Shared.Backend.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace SFB.Shared.Backend.Helpers
{
    public static class ProgramHelper
    {
        public static void ConfigureJsonSerialize(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
        }

        public static void ConfigureJwtAuthenticate(WebApplicationBuilder builder)
        {
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }

        public static void ConfigureDBContext<TContext>(WebApplicationBuilder builder,string nameConfig) where TContext : DbContext
        {
            var SGD = builder.Configuration.GetSection($"ConnectionString:{nameConfig}").Get<DBConfiguration>();
            builder.Services.AddDbContext<TContext>(options =>
            {
                UseConfiguredProviderName(options, SGD);
            });
        }

        public static void UseConfiguredProviderName(DbContextOptionsBuilder options, DBConfiguration conexion)
        {
            switch (conexion.NameProvider)
            {
                case NameProvider.MYSQL:
                    var serverVersion = new MySqlServerVersion(conexion.Version);
                    options.UseMySql(conexion.GetConnectionString(), serverVersion, mySqlOptions =>
                    {
                        mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    });
                    break;

            }
        }

        public static void ConfigureAddCors(WebApplicationBuilder builder,string name)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name, policy =>
                {
                    policy
                      //.WithOrigins("http://localhost:5173")
                      .AllowAnyOrigin()    // acepta cualquier dominio
                      .AllowAnyMethod()    // GET, POST, PUT, DELETE, OPTIONS…
                      .AllowAnyHeader();   // Content-Type, Authorization…n
                });
            });
        }

    }
}
