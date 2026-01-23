using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.SOM;
using SFB.Shared.Backend.Repositories;

namespace SFB.SOM.Backend.Repositories
{
    public class SalesSettingsRepositorie(SFBContext context) : BaseSettingsRepository<SFBContext,ESalesSettings>(context)
    {

        public async Task<int?> GetDefaultWarehouseId()
        {
            try
            {
                var setting = await base.GetSetting("POSTSAL", "CODALM");

                if (setting != null && int.TryParse(setting.ValueCode,out int warehouseid))                
                    return warehouseid;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int?> GetDefaultCustomerId()
        {
            try
            {
                var setting = await base.GetSetting("POSTSAL", "CODCLI");

                if (setting != null && int.TryParse(setting.ValueCode, out int customerId))
                    return customerId;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
