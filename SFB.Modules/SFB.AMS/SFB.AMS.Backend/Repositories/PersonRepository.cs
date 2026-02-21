
using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.AMS;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Repositories;

namespace SFB.AMS.Backend.Repositories
{
    public class PersonRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        internal async Task<List<EPerson>> GetPersons(string? type)
        {
            var query = Context.AMSPerson.AsNoTracking();

            query = query.ConditionalWhere(
                type,
                c => Context.AMSTypePerson
                    .Where(t => t.Type == type && t.Status)
                    .Select(t => t.NroPerson)
                    .Any(nro => nro == c.NroPerson)
            );

            return await query.ToListAsync();
        }
    }

}
