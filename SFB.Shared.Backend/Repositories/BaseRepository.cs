using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SFB.Shared.Backend.Repositories
{
    public abstract  class BaseRepository<TContext> 
        where TContext : DbContext
    {
        protected readonly TContext Context ;
        public BaseRepository(TContext context)
        {
            this.Context = context;
        }
        //public BaseRepository(IServiceProvider serviceProvider)
        //{
        //    this.Context = serviceProvider.GetRequiredService<TContext>();
        //}
    }
}
