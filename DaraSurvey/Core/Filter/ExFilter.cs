using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DaraSurvey.Core
{
    public static class ExFilter
    {
        public static IQueryable<T> GetOrderedQuery<T>(this IQueryable<T> query, IOrderedFilterable orderedFilter)
        {
            var entityProps =
                typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            if (!string.IsNullOrEmpty(orderedFilter.Sort))
                orderedFilter.Sort = entityProps.Any(o => o.Name.ToLower() == orderedFilter.Sort.ToLower())
                ? orderedFilter.Sort
                : entityProps.First().Name;
            else
                orderedFilter.Sort = entityProps.First().Name;

            orderedFilter.Skip = orderedFilter.Skip ?? 0;

            var param = Expression.Parameter(typeof(T), "x"); // x
            var body = Expression.PropertyOrField(param, orderedFilter.Sort); // x.SortBy
            var lambda = (dynamic)Expression.Lambda(body, param);

            query = orderedFilter.Asc == true
                ? Queryable.OrderBy(query, lambda)
                : Queryable.OrderByDescending(query, lambda);

            if (orderedFilter.Skip.HasValue && orderedFilter.Take.HasValue)
            {
                query = query.Skip(orderedFilter.Skip.Value);
                query = query.Take(orderedFilter.Take.Value);
            }

            if (orderedFilter.RndArgmnt)
                query = query.OrderBy(o => Guid.NewGuid());

            return query;
        }
    }
}
