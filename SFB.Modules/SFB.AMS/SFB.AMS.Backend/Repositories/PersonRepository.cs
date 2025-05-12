
using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.AMS;
using SFB.Shared.Backend.Repositories;

namespace SFB.AMS.Backend.Repositories
{
    public class PersonRepository(SGDContext context) : BaseRepository<SGDContext>(context)
    {
        internal async Task<List<EPerson>> GetPersons()
        {
            var result = await Context.AMSPerson.ToListAsync();

            return result;
        }
    }

}
