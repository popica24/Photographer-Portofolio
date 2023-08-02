using Microsoft.EntityFrameworkCore;

namespace MVCCore.Data
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public PaginatedList()
        {
            
        }

        private PaginatedList(List<T> items, int count, int pageIndex,int pageSize)
        {
            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);

            this.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex,int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items,count, pageIndex, pageSize);
        }
    }
}
