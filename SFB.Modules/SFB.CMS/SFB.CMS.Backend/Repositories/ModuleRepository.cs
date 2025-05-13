using Mapster;
using Microsoft.EntityFrameworkCore;
using SFB.CMS.Shared.Models;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.CMS;
using SFB.Shared.Backend.Repositories;

namespace SFB.CMS.Backend.Repositories
{
    public class ModuleRepository : BaseRepository<SFBContext>
    {
        public ModuleRepository(SFBContext context)
        : base(context)
        {
        }

        internal async Task<List<MModule>> GetModulesByUser(string? nameUser)
        {
            List<EModule> modules = await Context.CMSModules
                .Include(m => m.Groups)
                    .ThenInclude(g => g.GroupOptions)
                        .ThenInclude(go => go.OptionMenu)
                            .ThenInclude(god=> god.OptionMenus)// se rompe al agregar este include
                .Where(o => o.Groups
                    .Any(go => go.UserGroups
                        .Any(ug => ug.NameUser == nameUser)))
                .ToListAsync();


            List<MModule> modulesList = new List<MModule>();

            foreach (var module in modules)
            {
                MModule mModule = module.Adapt<MModule>();
                foreach (var group in module.Groups)
                {
                    foreach (var groupOption in group.GroupOptions)
                    {
                        if (groupOption.OptionMenu != null)
                        {
                            var mOptionMenu = groupOption.OptionMenu.Adapt<MOptionMenu>();
                            if (!mModule.OptionMenus.Any(o => o.CodOption == mOptionMenu.CodOption))
                            {
                                mModule.OptionMenus.Add(mOptionMenu);
                            }
                        }
                    }
                }
                modulesList.Add(mModule);
            }

            return modulesList;
        }
    }
}
