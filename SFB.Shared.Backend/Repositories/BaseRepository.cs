using Microsoft.EntityFrameworkCore;

namespace SFB.Shared.Backend.Repositories
{
    public abstract  class BaseRepository<TContext> 
        where TContext : DbContext
    {
        protected readonly TContext Context ;
        public BaseRepository(TContext repositoryContext)
        {
            this.Context = repositoryContext;
            //this.Contexts = new DbContext[] { repositoryContext };
        }
    }
}
