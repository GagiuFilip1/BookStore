using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BookStore.Core.Common;

namespace BookStore.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, OrderDirection direction)
        {
            var entity = typeof(TSource);
            var propertyInfo = entity.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            var arg = Expression.Parameter(entity, "x");
            var property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, arg);

            var actionName = direction.Equals(OrderDirection.Asc)
                ? "OrderBy"
                : "OrderByDescending";
            
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == actionName && m.IsGenericMethodDefinition)
                .Single(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    return parameters.Count == 2;
                });

            if (propertyInfo == null)
                return query;

            var genericMethod = method.MakeGenericMethod(entity, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>) genericMethod.Invoke(genericMethod, new object[] {query, selector});
            return newQuery;
        }
    }
}