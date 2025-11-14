using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.IAW;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.IAW.Backend.Repositories
{
    public class PresentationRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Code", "Name" };
        }

        internal async Task<PagedListModel<EPresentation>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.IAWPresentation.AsQueryable();

            return await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "Code" });
        }

        internal async Task<EPresentation> Create(EPresentation presentation)
        {
            Context.IAWPresentation.Add(presentation);
            await Context.SaveChangesAsync();
            return presentation;
        }

        internal async Task<EPresentation> Update(EPresentation presentation)
        {
            Context.IAWPresentation.Update(presentation);
            await Context.SaveChangesAsync();
            return presentation;
        }

        public async Task<bool> Delete(string code)
        {
            var entity = await Context.IAWPresentation.FindAsync(code);

            if (entity is null)
            {
                throw new ControllerException("Presentación no encontrada");
            }

            try
            {
                Context.IAWPresentation.Remove(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                    throw new ControllerException("No es posible eliminar, item en uso");

                throw;
            }
        }
    }
}
