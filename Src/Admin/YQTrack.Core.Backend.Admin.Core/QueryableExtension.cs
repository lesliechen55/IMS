using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, Func<bool> func, Expression<Func<TSource, bool>> predicate)
        {
            return func() ? source.Where(predicate) : source;
        }

        public static IQueryable<TSource> ToPage<TSource>(this IQueryable<TSource> source, int page, int size)
        {
            if (page <= 1) page = 1;
            if (size <= 10) size = 10;
            return source.Skip((page - 1) * size).Take(size);
        }
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<bool> func, Func<TSource, bool> predicate)
        {
            return func() ? source.Where(predicate) : source;
        }
    }
}