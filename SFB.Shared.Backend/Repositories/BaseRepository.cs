using Microsoft.EntityFrameworkCore;
using SGD.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Shared.Backend.Repositories
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
