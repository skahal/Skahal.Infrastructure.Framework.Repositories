using System;
using System.Threading.Tasks;
using Skahal.Infrastructure.Framework.Domain;

namespace Skahal.Infrastructure.Framework.Repositories
{
    /// <summary>
    /// Defines an unit of work repository.
    /// </summary>
    public interface IUnitOfWorkRepository
    {
        /// <summary>
        /// Persists the new item.
        /// </summary>
        /// <param name="item">Item.</param>
        Task PersistNewItemAsync(IAggregateRoot item);

        /// <summary>
        /// Persists the updated item.
        /// </summary>
        /// <param name="item">Item.</param>
        Task PersistUpdatedItemAsync(IAggregateRoot item);

        /// <summary>
        /// Persists the deleted item.
        /// </summary>
        /// <param name="item">Item.</param>
        Task PersistDeletedItemAsync(IAggregateRoot item);
    }
}

