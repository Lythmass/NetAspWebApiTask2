using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories
{
    public class PagedList<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public PagedList(List<T> items, int pageNumber, int pageSize, int totalCount, bool hasNextPage, bool hasPreviousPage)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> items, int pageNumber, int pageSize)
        {
            var pagedItems = await items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await items.CountAsync();
            var hasNextPage = (pageNumber * pageSize) < totalCount;
            var hasPreviousPage = pageNumber > 1;

            return new PagedList<T>(pagedItems, pageNumber, pageSize, totalCount, hasNextPage, hasPreviousPage);
        }
    }

}