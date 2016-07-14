using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelperSharp;
using Skahal.Infrastructure.Framework.Domain;

namespace Skahal.Infrastructure.Framework.Repositories
{
    /// <summary>
    /// A base class for repositories.
    /// </summary>
    public abstract class RepositoryBase<TEntity>
        : IRepository<TEntity>, IUnitOfWorkRepository where TEntity : IAggregateRoot
    {
        #region Fields
        private IUnitOfWork m_unitOfWork;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Skahal.Infrastructure.Framework.Repositories.RepositoryBase&lt;TEntity, TKey&gt;"/> class.
        /// </summary>
        protected RepositoryBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skahal.Infrastructure.Framework.Repositories.RepositoryBase&lt;TEntity, TKey&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        protected RepositoryBase(IUnitOfWork unitOfWork)
        {
            m_unitOfWork = unitOfWork;
        }
        #endregion

        #region IRepository<T> Members
        /// <summary>
        /// Finds the entity by the key.
        /// </summary>
        /// <returns>The found entity.</returns>
        /// <param name="key">Key.</param>
        /// <param name="syncBeforeFind">Sync before find.</param>
        public abstract Task<TEntity> FindByAsync(object key, bool syncBeforeFind = true);

        /// <summary>
        /// Finds all entities that matches the filter.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="filter">Filter.</param>
        public abstract Task<IEnumerable<TEntity>> FindAllAsync(int offset, int limit, Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Finds all entities that matches the filter in a ascending order.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="offset">The offset to start the result.</param>
        /// <param name="limit">The result count limit.</param>
        /// <param name="filter">The entities filter.</param>
        /// <param name="orderBy">The order.</param>
        public abstract Task<IEnumerable<TEntity>> FindAllAscendingAsync<TKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderBy);

        /// <summary>
        /// Finds all entities that matches the filter in a descending order.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="offset">The offset to start the result.</param>
        /// <param name="limit">The result count limit.</param>
        /// <param name="filter">The entities filter.</param>
        /// <param name="orderBy">The order.</param>
        public abstract Task<IEnumerable<TEntity>> FindAllDescendingAsync<TKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderBy);

        /// <summary>
        /// Counts all entities that matches the filter.
        /// </summary>
        /// <returns>The found entities.</returns>
        /// <param name="filter">Filter.</param>
        public abstract Task<long> CountAllAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Sets the unit of work.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public virtual void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            m_unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Add the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void Add(TEntity item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            ValidateUnitOfWork();
            m_unitOfWork.RegisterAdded(item, this);
        }

        /// <summary>
        /// Remove the specified entity.
        /// </summary>
        /// <param name="item">The entity.</param>
        public void Remove(TEntity item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            ValidateUnitOfWork();
            m_unitOfWork.RegisterRemoved(item, this);
        }
         
        /// <summary>
        /// Attach the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public async Task<TEntity> Attach(TEntity entity)
        {
            var old = await FindByAsync(entity.Key, false);

            if (old == null)
                Add(entity);
            else
            {
                ValidateUnitOfWork();
                m_unitOfWork.RegisterChanged(entity, this);
            }

            return entity;
        }


        #endregion

        #region IUnitOfWorkRepository Members
        /// <summary>
        /// Persists the new item.
        /// </summary>
        /// <param name="item">Item.</param>
        public virtual async Task PersistNewItemAsync(IAggregateRoot item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            await PersistNewItemAsync((TEntity)item);
        }

        /// <summary>
        /// Persists the updated item.
        /// </summary>
        /// <param name="item">Item.</param>
        public virtual async Task PersistUpdatedItemAsync(IAggregateRoot item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            await PersistUpdatedItemAsync((TEntity)item);
        }

        /// <summary>
        /// Persists the deleted item.
        /// </summary>
        /// <param name="item">Item.</param>
        public virtual async Task PersistDeletedItemAsync(IAggregateRoot item)
        {
            ExceptionHelper.ThrowIfNull("item", item);

            await PersistDeletedItemAsync((TEntity)item);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        protected IUnitOfWork UnitOfWork
        {
            get { return m_unitOfWork; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Persists the new item.
        /// </summary>
        /// <param name="item">Item.</param>
        protected abstract Task PersistNewItemAsync(TEntity item);

        /// <summary>
        /// Persists the updated item.
        /// </summary>
        /// <param name="item">Item.</param>
        protected abstract Task PersistUpdatedItemAsync(TEntity item);

        /// <summary>
        /// Persists the deleted item.
        /// </summary>
        /// <param name="item">Item.</param>
        protected abstract Task PersistDeletedItemAsync(TEntity item);

        #endregion

        #region Helpers
        /// <summary>
        /// Validates the unit of work.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">There is no UnitOfWork configured for the repository '{0}'..With(GetType().Name)</exception>
        protected void ValidateUnitOfWork()
        {
            if (m_unitOfWork == null)
            {
                throw new InvalidOperationException("There is no UnitOfWork configured for the repository '{0}'.".With(GetType().Name));
            }
        }
        #endregion
    }
}