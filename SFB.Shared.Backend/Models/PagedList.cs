
using Microsoft.EntityFrameworkCore;

namespace SFB.Shared.Backend.Models
{
    public class PagedList<T> : List<T> where T : class
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            pageSize = (pageSize == -1) ? count : pageSize;
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            AddRange(items);
        }

        private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            pageSize = (pageSize == -1) ? count : pageSize;
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(
              IQueryable<T> source,
              int pageNumber,
              int pageSize,
              bool asNoTracking = true,
              CancellationToken cancellationToken = default)
        {
            if (asNoTracking)
                source = source.AsNoTracking();   // 👈 aquí metemos el "asnotrasqui"

            var count = await source.CountAsync(cancellationToken);   // 👈 1ª consulta (COUNT)

            if (pageSize == -1)
            {
                var allItems = await source.ToListAsync(cancellationToken);  // 👈 2ª consulta (SELECT todo)
                return new PagedList<T>(allItems, count, pageNumber, count);
            }

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);  // 👈 2ª consulta (SELECT page)

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public PagedListModel<T> ToPagedListModel()
        {
            PagedListModel<T> model = new PagedListModel<T>();
            model.TotalCount = this.TotalCount;
            model.PageSize = this.PageSize;
            model.CurrentPage = this.CurrentPage;
            model.HasNext = this.HasNext;
            model.HasPrevious = this.HasPrevious;
            model.TotalPages = this.TotalPages;
            model.Data = this.ToList();
            return model;
        }
    }
}
