using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Common
{
    //public static class IQueryableExtensions
    //{
    //    public static async Task<Paged<T>> ToPagedItemsAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
    //    {
    //        var count = await source.CountAsync().ConfigureAwait(false);
    //        var collection = await source
    //            .AsNoTracking()
    //            .Skip((pageIndex - 1) * pageSize)
    //            .Take(pageSize)
    //            .ToListAsync()
    //            .ConfigureAwait(false);

    //        return new Paged<T>(collection, count, pageIndex, pageSize);
    //    }

    //}

    //public class Paged<T> where T : class
    //{
    //    public Paged(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    //    {
    //        if (pageIndex < 1)
    //            throw new ArgumentOutOfRangeException(nameof(pageIndex));
    //        if (pageSize < 1)
    //            throw new ArgumentOutOfRangeException(nameof(pageSize));

    //        Items = items;
    //        TotalCount = totalCount;
    //        PageSize = pageSize;
    //        PageIndex = pageIndex;
    //    }

    //    public IEnumerable<T> Items { get; }
    //    public int PageIndex { get; }
    //    public int PageSize { get; }
    //    public int TotalCount { get; }
    //    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    //    //public bool HasPreviousPage => PageIndex > 1;
    //    //public bool HasNextPage => PageIndex < TotalPages;
    //    public int StartIndex => (PageIndex - 1) * PageSize;

    //    //public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    //    //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}

    //public class IdName
    //{
    //    public IdName(string id, string name)
    //    {
    //        Id = id;
    //        Name = name;
    //    }
    //    public string Id { get; }
    //    public string Name { get; }
    //}
}
