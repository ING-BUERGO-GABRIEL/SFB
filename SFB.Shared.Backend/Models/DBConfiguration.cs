using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SFB.Shared.Backend.Enums;
using System.Runtime.CompilerServices;

namespace SFB.Shared.Backend.Models
{
    public class DBConfiguration
    {
        public NameProvider NameProvider { get; set; }
        public Version Version { get; set; }
        public string DefaultConnection { get; set; }

        public DBConfiguration() { }

        public DBConfiguration(IConfigurationSection configSection) 
        {
            configSection.Bind(this);
        }


        public static void UseConfiguredProviderName(DbContextOptionsBuilder options, DBConfiguration conexion)
        {
            switch (conexion.NameProvider)
            {
                case NameProvider.MYSQL:
                    var serverVersion = new MySqlServerVersion(conexion.Version);
                    options.UseMySql(conexion.DefaultConnection, serverVersion, mySqlOptions =>
                    {
                        mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    });
                    break;
                case NameProvider.POSTGRESQL:
                    options.UseNpgsql(conexion.DefaultConnection);
                    break;

            }
        }
    }
}
