using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using HelperSharp;
using Skahal.Infrastructure.Framework.Domain;

namespace Skahal.Infrastructure.Framework.Repositories
{
    /// <summary>
    /// A basic repository on memory.
    /// <remarks>
    /// In most of cases will be used for tests purposes.
    /// </remarks>
    /// </summary>
    public class MemoryRepository<TEntity> : RepositoryBase<TEntity> where TEntity : IAggregateRoot
    {
        #region Fields
        private Func<TEntity, object> m_createNewKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Skahal.Infrastructure.Framework.Repositories.MemoryRepository&lt;TEntity, TKey&gt;"/> class.
        /// </summary>
        public MemoryRepository(Func<TEntity, object> createNewKey)
        {
            m_createNewKey = createNewKey;
            Entities = new ObservableCollection<TEntity>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skahal.Infrastructure.Framework.Repositories.MemoryRepository&lt;TEntity, TKey&gt;"/> class.
        /// </summary>
        /// <param name="createNewKey">Create new key.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public MemoryRepository(IUnitOfWork unitOfWork, Func<TEntity, object> createNewKey)
            : base(unitOfWork)
        {
            m_createNewKey = createNewKey;
            Entities = new ObservableCollection<TEntity>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        protected ObservableCollection<TEntity> Entities { get; private set; }
        #endregion

        #region implemented abstract members of RepositoryBase
        /// <summary>
        /// Finds the entity by the key.
        /// </summary>
        /// <returns>The found entity.</returns>
        /// <param name="key">Key.</param>
        /// <param name="syncBeforeFind">Sync before find.</param>
        public override async Task<TEntity> FindByAsync(object key, bool syncBeforeFind = true)
        {
            var result = await FindAllAsync(0, 1, e => e.Key.Equals(key));
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Finds all entities that matches the filter.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="filter">Filter.</param>
        public override async Task<IEnumerable<TEntity>> FindAllAsync(int offset, int limit, Expression<Func<TEntity, bool>> filter)
        {
            return (await InitializeQuery(filter))
                                    .OrderBy(e => e.Key)
                                    .Skip(offset)
                                    .Take(limit);
        }

        /// <summary>
        /// Finds all entities that matches the filter in a ascending order.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="offset">The offset to start the result.</param>
        /// <param name="limit">The result count limit.</param>
        /// <param name="filter">The entities filter.</param>
        /// <param name="orderBy">The order.</param>
        public override async Task<IEnumerable<TEntity>> FindAllAscendingAsync<TOrderByKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
        {
            ExceptionHelper.ThrowIfNull("orderBy", orderBy);

            return (await InitializeQuery(filter))
                .OrderBy(e => orderBy.Compile()(e))
                .Skip(offset)
                .Take(limit);
        }

        /// <summary>
        /// Finds all entities that matches the filter in a descending order.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="offset">The offset to start the result.</param>
        /// <param name="limit">The result count limit.</param>
        /// <param name="filter">The entities filter.</param>
        /// <param name="orderBy">The order.</param>
        public override async Task<IEnumerable<TEntity>> FindAllDescendingAsync<TOrderByKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
        {
            ExceptionHelper.ThrowIfNull("orderBy", orderBy);

            return (await InitializeQuery(filter))
                .OrderByDescending(e => orderBy.Compile()(e))
                .Skip(offset)
                .Take(limit);
        }

        /// <summary>
        /// Counts all entities that matches the filter.
        /// </summary>
        /// <returns>The number of the entities that matches the filter.</returns>
        /// <param name="filter">Filter.</param>
        public override async Task<long> CountAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            long result = 0;

            if (filter == null)
            {
                result = Entities.Count();
            }
            else
            {
                result = Entities.Count(e => filter.Compile()(e));
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Persists the new item.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override async Task PersistNewItemAsync(TEntity item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            if (item.Key is string && string.IsNullOrEmpty(item.Key as string))
                item.Key = null;

            if (Entities.FirstOrDefault(e => e.Key.Equals(item.Key)) != null)
            {
                throw new InvalidOperationException("There is another entity with id '{0}'.".With(item.Key));
            }

            if (item.Key == null || (item.Key.GetType().GetTypeInfo().IsValueType && Activator.CreateInstance(item.Key.GetType()).Equals(item.Key)))
            {
                item.Key = m_createNewKey(item);
            }

            await Task.Run(() => Entities.Add(item));
        }

        /// <summary>
        /// Persists the updated item.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override async Task PersistUpdatedItemAsync(TEntity item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            await PersistDeletedItemAsync(item);
            await PersistNewItemAsync(item);
        }

        /// <summary>
        /// Persists the deleted item.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override async Task PersistDeletedItemAsync(TEntity item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            var old = Entities.FirstOrDefault(e => e.Key.Equals(item.Key));

            if (old == null)
            {
                throw new InvalidOperationException("There is no entity with id '{0}'.".With(item.Key));
            }

            await Task.Run(() => Entities.Remove(old));
        }

        private Task<ObservableCollection<TEntity>> InitializeQuery(Expression<Func<TEntity, bool>> filter)
        {
            ObservableCollection<TEntity> results = new ObservableCollection<TEntity>();

            if (filter == null)
            {
                results = Entities;
            }
            else
            {
                results = new ObservableCollection<TEntity>(Entities.Where(e => filter.Compile()(e)));
            }

            return Task.FromResult(results);
        }
        #endregion       
    }
}

