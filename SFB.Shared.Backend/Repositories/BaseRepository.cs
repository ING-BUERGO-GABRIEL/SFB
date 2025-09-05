using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;

namespace SFB.Shared.Backend.Repositories
{
    public abstract  class BaseRepository<TContext> 
        where TContext : DbContext
    {
        protected readonly TContext Context ;

        protected  int PageSize = 10;
        protected  int PageNumber = 1;
        public BaseRepository(TContext context)
        {
            this.Context = context;
        }

        //public BaseRepository(IServiceProvider serviceProvider)
        //{
        //    this.Context = serviceProvider.GetRequiredService<TContext>();
        //}

        protected async Task<PagedListModel<TEntity>> GetPage<TEntity>(IQueryable<TEntity> query, string filter, int pageSize, int pageNumber, List<string>? orderBy) where TEntity : class
        {
            query = Filter(query, filter);

            if (orderBy != null && orderBy.Any())
                query = OrderBy(query, orderBy);

            var pagedList = await PagedList<TEntity>.CreateAsync(query, pageNumber, pageSize);

            return pagedList.ToPagedListModel();
        }

        protected Task<PagedListModel<TEntity>> GetPage<TEntity>(IQueryable<TEntity> query, string filter, int pageSize, int pageNumber) where TEntity : class
        {
            return GetPage(query, filter, pageSize, pageNumber, new List<string>());
        }

        protected Task<PagedListModel<TEntity>> GetPage<TEntity>(IQueryable<TEntity> query, string filter, List<string>? orderBy) where TEntity : class
        {
            return GetPage(query, filter, PageSize, PageNumber, orderBy);            
        }

        protected  Task<PagedListModel<TEntity>> GetPage<TEntity>(IQueryable<TEntity> query, string filter) where TEntity : class
        {
            return GetPage(query, filter, PageSize, PageNumber, new List<string>());
        }

        protected virtual IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, string value)
        {
            try
            {
                var filterProps = GetFilterableProperties();
                var filteredQuery = ExpressionHelper<TEntity>.GenerateSearchQuery(filterProps, query, value);
                return filteredQuery;
            }
            catch (Exception ex)
            {
                return query;
            }
        }

        protected virtual IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> query, List<string> orderProperties)
        {
            if (orderProperties.Count() == 0)
                return query;

            var orderedQuery = ExpressionHelper<TEntity>.GenerateOrderByQuery(query, orderProperties);

            return orderedQuery;
        }

        protected virtual List<string> GetFilterableProperties()
        {
            return new List<string>();
        }
    }
}
