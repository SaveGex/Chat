using Domain.Models.Contracts;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure
{
    public static class KeysetPaginationBuilderService<TEntity>
        where TEntity : IIdentifiable
    {
        public static Action<KeysetPaginationBuilder<TEntity>> CreateActionKeysetPaginationBuilder<TKey, TBreaker>(
            Expression<Func<TEntity, TKey>> orderBy,
            Expression<Func<TEntity, TBreaker>> tieBreaker,
            bool isDescending)
        {
            return builder =>
            {
                if (isDescending)
                {
                    builder.Descending(orderBy);
                    builder.Descending(tieBreaker);
                }
                else
                {
                    builder.Ascending(orderBy);
                    builder.Ascending(tieBreaker);
                }
            };
        }

        /// <summary>
        /// Creates an action that configures a keyset pagination builder with the specified ordering and tie-breaker
        /// properties.
        /// </summary>
        /// <remarks>The returned action can be used to configure a KeysetPaginationBuilder<TEntity>
        /// instance for keyset pagination scenarios. Both the primary and tie-breaker properties must exist on the
        /// entity type and be suitable for ordering. This method is typically used to facilitate efficient,
        /// cursor-based pagination in data access layers.</remarks>
        /// <param name="orderByPropName">The name of the property to use for the primary sort order. This property determines the main ordering of
        /// the paginated results.</param>
        /// <param name="tieBreakerPropName">The name of the property to use as a tie-breaker when the primary sort property has duplicate values. This
        /// ensures a consistent and deterministic order.</param>
        /// <param name="isDescending">true to sort the results in descending order; otherwise, false to sort in ascending order.</param>
        /// <returns>An action that applies the specified ordering and tie-breaker configuration to a KeysetPaginationBuilder for
        /// the entity type.</returns>
        public static Action<KeysetPaginationBuilder<TEntity>> CreateActionKeysetPaginationBuilder(
            string orderByPropName,
            string tieBreakerPropName,
            bool isDescending)
        {
            var orderByExpression = CreateLambdaExpression(orderByPropName);
            var tieBreakerExpression = CreateLambdaExpression(tieBreakerPropName);

            var keyType = orderByExpression.ReturnType;
            var tieBreakerType = tieBreakerExpression.ReturnType;

            MethodInfo ExtractKeysetPaginationBuilderMethod(string methodName, Type genericMethodType) =>
                typeof(KeysetPaginationBuilder<TEntity>)
                    .GetMethods()
                    .First(m => m.Name == methodName && m.IsGenericMethod)
                    .MakeGenericMethod(genericMethodType);


            if (isDescending)
            {
                var descendingMethod = ExtractKeysetPaginationBuilderMethod("Descending", keyType);

                var descendingMethodTieBreaker = ExtractKeysetPaginationBuilderMethod("Descending", tieBreakerType);

                return builder =>
                {
                    descendingMethod.Invoke(builder, new object[] { orderByExpression });
                    descendingMethodTieBreaker.Invoke(builder, new object[] { tieBreakerExpression });
                };
            }
            else
            {
                var ascendingMethod = ExtractKeysetPaginationBuilderMethod("Ascending", keyType);

                var ascendingMethodTieBreaker = ExtractKeysetPaginationBuilderMethod("Ascending", tieBreakerType);

                return builder =>
                {
                    ascendingMethod.Invoke(builder, new object[] { orderByExpression });
                    ascendingMethodTieBreaker.Invoke(builder, new object[] { tieBreakerExpression });
                };
            }
        }

        public static LambdaExpression CreateLambdaExpression(string propName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "m");
            var property = Expression.PropertyOrField(parameter, propName);
            return Expression.Lambda(property, parameter);
        }
    }
}
