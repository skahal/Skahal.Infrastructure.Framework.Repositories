using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skahal.Infrastructure.Framework.Domain;

namespace Skahal.Infrastructure.Framework.Repositories
{
    /// <summary>
    /// Repository extensions.
    /// </summary>
    public static class RepositoryExtensions
    {
        #region FindAll
        /// <summary>
        /// Finds all entities.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="filter">Filter.</param>
        public static async Task<IEnumerable<TEntity>> FindAll<TEntity>(this IRepository<TEntity> repository)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAsync(0, int.MaxValue, null);
        }

        /// <summary>
        /// Finds all entities that matches the filter.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="filter">Filter.</param>
        public static async Task<IEnumerable<TEntity>> FindAll<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAsync(0, int.MaxValue, filter);
        }

        /// <summary>
        /// Finds all entities.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        public static async Task<IEnumerable<TEntity>> FindAll<TEntity>(this IRepository<TEntity> repository, int offset, int limit)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAsync(offset, limit, null);
        }

        /// <summary>
        /// Finds all entities.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        public static async Task<IEnumerable<TEntity>> FindAll<TEntity>(this IRepository<TEntity> repository, int offset, long limit)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAsync(offset, Convert.ToInt32(limit), null);
        }
        #endregion

        #region FindAllAscending
        /// <summary>
        /// Finds all entities  in a ascending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">The repository.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllAscending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAscendingAsync(0, int.MaxValue, null, orderBy);
        }

        /// <summary>
        /// Finds all entities that matches the filter in a ascending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllAscending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAscendingAsync(0, int.MaxValue, filter, orderBy);
        }

        /// <summary>
        /// Finds all entities in a ascending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllAscending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, int offset, int limit, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAscendingAsync(offset, limit, null, orderBy);
        }

        /// <summary>
        /// Finds all entities in a ascending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAll<TEntity, TOrderByKey>(this IRepository<TEntity> repository, int offset, long limit, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllAscendingAsync(offset, Convert.ToInt32(limit), null, orderBy);
        }
        #endregion

        #region FindAllDescending
        /// <summary>
        /// Finds all entities  in a descending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">The repository.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllDescending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllDescendingAsync(0, int.MaxValue, (f) => true, orderBy);
        }

        /// <summary>
        /// Finds all entities that matches the filter in a descending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllDescending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllDescendingAsync(0, int.MaxValue, filter, orderBy);
        }

        /// <summary>
        /// Finds all entities in a descending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllDescending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, int offset, int limit, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllDescendingAsync(offset, limit, null, orderBy);
        }

        /// <summary>
        /// Finds all entities in a descending order
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="repository">Repository.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="orderBy">The order.</param>
        public static async Task<IEnumerable<TEntity>> FindAllDescending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, int offset, long limit, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            return await repository.FindAllDescendingAsync(offset, Convert.ToInt32(limit), null, orderBy);
        }
        #endregion

        #region CountAll
        /// <summary>
        /// Counts all entities.
        /// </summary>
        /// <param name="repository">Repository.</param>
        /// <returns>The number of the entities that matches the filter.</returns>
        public static async Task<long> CountAll<TEntity>(this IRepository<TEntity> repository)
            where TEntity : IAggregateRoot
        {
            return await repository.CountAllAsync(null);
        }
        #endregion

        #region FindFirst
        /// <summary>
        /// Finds the first entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <returns>The first entity.</returns>
        public static async Task<TEntity> FindFirst<TEntity>(this IRepository<TEntity> repository)
            where TEntity : IAggregateRoot
        {
            var result = await repository.FindAll(0, 1);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Finds the first entity that match the filter.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The first entity that match the filter or null if none match.</returns>
        public static async Task<TEntity> FindFirst<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter)
            where TEntity : IAggregateRoot
        {
            var result = await repository.FindAllAsync(0, 1, filter);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Finds the first entity that match the filter in an ascending order.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order.</param>
        /// <returns>The first entity that match the filter or null if none match.</returns>
        public static async Task<TEntity> FindFirstAscending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            var result = await repository.FindAllAscendingAsync(0, 1, filter, orderBy);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Finds the first entity that match the filter in an descending order.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order.</param>
        /// <returns>The first entity that match the filter or null if none match.</returns>
        public static async Task<TEntity> FindFirstDescending<TEntity, TOrderByKey>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : IAggregateRoot
        {
            var result = await repository.FindAllDescendingAsync(0, 1, filter, orderBy);
            return result.FirstOrDefault();
        }
        #endregion

        #region FindLast
        /// <summary>
        /// Finds the last entity.
        /// </summary>
        /// <returns>The last entity.</returns>
        /// <param name="repository">Repository.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        public static async Task<TEntity> FindLast<TEntity>(this IRepository<TEntity> repository)
            where TEntity : IAggregateRoot
        {
            var last = await repository.CountAll();
            var result = await repository.FindAll(Convert.ToInt32(last - 1), 1);
            return result.FirstOrDefault();
        }
        #endregion
    }
}