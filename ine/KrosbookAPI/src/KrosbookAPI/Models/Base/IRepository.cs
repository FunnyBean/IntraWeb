using System;
using System.Linq;
using System.Linq.Expressions;

namespace Krosbook.Models.Base
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the T model.
    /// </summary>
    /// <typeparam name="T">
    /// Type of model. <seealso cref="Krosbook.Models.Base.IModel"/>
    /// </typeparam>
    public interface IRepository<T> where T : class, IModel, new()
    {
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>Queryalble for obtain all items.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets the items by predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// Items that satisfy the condition specified by predicate.
        /// </returns>
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the item by Id.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns>
        /// Return item with specific id; otherwise null.
        /// </returns>
        T GetItem(int itemId);

        /// <summary>
        /// Gets the item by Id.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Return item with specific id; otherwise null.
        /// </returns>
        T GetItem(int itemId, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the item by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Return item, which match the predicate; otherwise null.
        /// </returns>
        T GetItem(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the item by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Return item, which match the predicate; otherwise null.
        /// </returns>
        T GetItem(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The new item.</param>
        void Add(T item);

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Edit(T item);

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item for deleting.</param>
        void Delete(T item);

        /// <summary>
        /// Deletes the specified item by id.
        /// </summary>
        /// <param name="itemId">The item identifier for deleting.</param>
        void Delete(int itemId);

        /// <summary>
        /// Deletes the specified items by predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        void Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Save changes.
        /// </summary>
        void Save();
    }
}