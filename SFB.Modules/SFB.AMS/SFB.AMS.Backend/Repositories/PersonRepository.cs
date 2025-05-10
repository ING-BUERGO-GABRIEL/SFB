
using Microsoft.EntityFrameworkCore;
using SGD.Infrastructure.Contexts;
using SGD.Infrastructure.Entities.AMS;
using SGD.Shared.Backend.Repositories;

namespace SGD.AMS.Backend.Repositories
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
