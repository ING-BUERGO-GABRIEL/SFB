using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.CMS;
using SFB.Shared.Backend.Repositories;


namespace SFB.CMS.Backend.Repositories
{
    public class OptionMenuRepository : BaseRepository<SFBContext>
    {
        public OptionMenuRepository(SFBContext context)
        : base(context)
        {
        }

        internal async Task<List<EOptionMenu>> GetOptionMenuByUser(string nameUser)
        {
            List<EOptionMenu> optionMenus = await Context.CMSOptionMenus
                        .Where(o => o.GroupOptions
                            .Any(go => go.Group.UserGroups
                                .Any(ug => ug.NameUser == nameUser)))
                        .ToListAsync();

            return optionMenus;
        }
    }
}
