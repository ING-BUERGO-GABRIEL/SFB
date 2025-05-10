using Microsoft.EntityFrameworkCore;
using SGD.Infrastructure.Contexts;
using SGD.Infrastructure.Entities.CMS;
using SGD.Shared.Backend.Repositories;


namespace SGD.CMS.Backend.Repositories
{
    public class OptionMenuRepository : BaseRepository<SGDContext>
    {
        public OptionMenuRepository(SGDContext context)
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
