using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Entities.CMS;

namespace SFB.Shared.Backend.Repositories
{
    public abstract class BaseSettingsRepository<TContext, Entity> : BaseRepository<TContext>
        where TContext : DbContext
        where Entity : SettingsBase<Entity>
    {

        public BaseSettingsRepository(TContext context) : base(context)
        {

        }

        public BaseSettingsRepository(IServiceProvider services) : base(services)
        {

        }

        public async Task<Entity?> GetSetting(string codeGroup, string code)
        {
            var tabpar = Context.Set<Entity>();

            var parameter = await tabpar
                .Where(r => tabpar.Any(s => s.ConfigCode == codeGroup && s.SettingId == r.SettingsGroup)
                            && r.ConfigCode == code)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return parameter; 
        }

    }
}
