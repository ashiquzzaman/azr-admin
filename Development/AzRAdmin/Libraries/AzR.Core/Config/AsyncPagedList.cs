using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PagedList;

namespace AzR.Core.Config
{
    public class AsyncPagedList<T> : BasePagedList<T>
    {
        private AsyncPagedList() { }

        public static async Task<IPagedList<T>> Create(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var list = new AsyncPagedList<T>();
            await list.Init(superset, pageNumber, pageSize);
            return list;
        }
        private async Task Init(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");
            TotalItemCount = superset == null ? 0 : await superset.CountAsync();
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
            var num = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = num > TotalItemCount ? TotalItemCount : num;
            if (superset == null || TotalItemCount <= 0)
                return;
            var skipCount = pageNumber == 1 ? 0 : (pageNumber - 1) * pageSize;
            Subset.AddRange(await superset.Skip(skipCount).Take(pageSize).ToListAsync());
        }
    }
    public static class PagedListExtensions
    {
        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="AsyncPagedList{T}"/>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return await AsyncPagedList<T>.Create(superset, pageNumber, pageSize);
        }
    }
}