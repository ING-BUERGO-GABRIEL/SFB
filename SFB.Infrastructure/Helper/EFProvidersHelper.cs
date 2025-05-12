using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SFB.Infrastructure.Models;

namespace SFB.Infrastructure.Helper
{
    public static class EFProvidersHelper
    {
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

    }


}
