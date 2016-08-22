using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Krosbook.Models.Base
{
    /// <summary>
    ///  Generic repository for CRUD operations on the T model.
    /// </summary>
    /// <typeparam name="T">
    /// Type of model. <seealso cref="IntraWeb.Models.Base.IModel"/>
    /// </typeparam>
    /// <seealso cref="IntraWeb.Models.Base.IRepository{T}" />
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IModel, new()
    {
        #region Protected Fields

        protected ApplicationDbContext _dbContext;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item for add.</param>
        public virtual void Add(T item)
        {
            _dbContext.Set<T>().Add(item);
        }

        /// <summary>
        /// Deletes the specified item by Id.
        /// </summary>
        /// <param name="itemId">The item identifier for deleting.</param>
        public virtual void Delete(int itemId)
        {
            this.Delete(this.GetItem(itemId));
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">THe item for delete.</param>
        public virtual void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        /// <summary>
        /// Deletes the specified items by predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            _dbContext.RemoveRange(this.Get(predicate));
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="roomitem">The item for edit.</param>
        public virtual void Edit(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Gets the item by Id.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns>
        /// Return item with specific id; otherwise null.
        /// </returns>
        public virtual T GetItem(int itemId)
        {
            return this.GetItem(p => p.Id == itemId);
        }

        /// <summary>
        /// Gets the item by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Return item, which match the predicate; otherwise null.
        /// </returns>
        public virtual T GetItem(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets the item by Id.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Return item with specific id; otherwise null.
        /// </returns>
        public virtual T GetItem(int itemId, params Expression<Func<T, object>>[] includeProperties)
        {
            return this.GetItem(p => p.Id == itemId, includeProperties);
        }

        /// <summary>
        /// Gets the item by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Return item, which match the predicate; otherwise null.
        /// </returns>
        public virtual T GetItem(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.AsNoTracking().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>
        /// Queryalble for obtain all items.
        /// </returns>
        public virtual IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// Gets the items by predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// Items that satisfy the condition specified by predicate.
        /// </returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsNoTracking().Where(predicate);
        }

        /// <summary>
        /// Gets the single item by predicate.
        /// </summary>
        /// <param name="predicate"></param>
        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets unique item by predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public virtual void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}