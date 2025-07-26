using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SFB.Shared.Backend.Enums;

namespace SFB.Shared.Backend.Models
{
    public class DBConfiguration
    {
        public NameProvider NameProvider { get; set; }
        public Version Version { get; set; }
        public string Server { get; set; }
        public string Host { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public DBConfiguration() { }

        public DBConfiguration(IConfigurationSection configSection) 
        {
            configSection.Bind(this);
        }

        public string GetConnectionString()
        {
            switch (NameProvider)
            {
                case NameProvider.MYSQL:
                    return $"Server={Server};Database={Database};User={User};Password={Password};";
                case NameProvider.POSTGRESQL:
                    return $"Host={Host};Database={Database};Username={User};Password={Password};SSL Mode=Require;Trust Server Certificate=true";              
                default:
                    throw new Exception("NameProvider not Suport");
            }

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
                case NameProvider.POSTGRESQL:
                    options.UseNpgsql(conexion.GetConnectionString());
                    break;

            }
        }
    }
}
