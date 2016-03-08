using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IntraWeb.Models.Base
{
    /// <summary>
    /// Generic dummy repository for testing.
    /// </summary>
    /// <typeparam name="T">
    /// Type of model. <seealso cref="IntraWeb.Models.Base.IModel"/>
    /// </typeparam>
    /// <seealso cref="IntraWeb.Models.Base.IRepository{T}" />
    public class BaseDummyRepository<T> : IRepository<T> where T : class, IModel, new()
    {
        #region Protected Fields

        protected List<T> _data = new List<T>();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDummyRepository{T}"/> class.
        /// </summary>
        public BaseDummyRepository()
        {
            InitDummyData(_data);
        }

        /// <summary>
        /// Throw exception when saving data? For unit tests.
        /// </summary>
        public bool ThrowExceptionWhenSaveData { get; set; } = false;

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item for add.</param>
        public virtual void Add(T item)
        {          
            item.Id = _data?.Count > 0 ? _data.Max(p => p.Id) + 1 : 0;

            _data.Add(item);
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
            if (item != null)
            {
                _data.Remove(item);
            }
        }

        /// <summary>
        /// Deletes the specified items by predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            //try
            //{
            //    foreach (T item in this.Get(predicate))
            //    {
            //        this.Delete(item);
            //    }
            //}
            //catch (Exception)
            //{
            //}
            foreach (T item in this.Get(predicate))
            {
                this.Delete(item);
            }
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="roomitem">The item for edit.</param>
        public virtual void Edit(T item)
        {
            var destItem = GetItem(item.Id);
            AutoMapper.Mapper.Map<T, T>(item, destItem);
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
            return _data.FirstOrDefault(p => p.Id == itemId);
        }

        /// <summary>
        /// Gets the item by Id with includings.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="asNoTracking">The returned entities will not be cached in the DbContext.</param>
        /// <param name="includeProperties">A function for all includes.</param>
        /// <returns>
        /// Return item with specific id; otherwise null.
        /// </returns>
        public virtual T GetItemIncluding(int itemId, bool asNoTracking = false, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the item by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Return item, which match the predicate; otherwise null.
        /// </returns>
        public T GetItem(Expression<Func<T, bool>> predicate)
        {
            return this.GetAll().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>
        /// Queryalble for obtain all items.
        /// </returns>
        public virtual IQueryable<T> GetAll()
        {
            return _data.AsQueryable();
        }

        /// <summary>
        /// Gets all items with includings.
        /// </summary>
        /// <returns>
        /// Queryalble for obtain all items.
        /// </returns>
        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the items by predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// Items that satisfy the condition specified by predicate.
        /// </returns>
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return this.GetAll().Where(predicate);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public virtual void Save()
        {
            if (this.ThrowExceptionWhenSaveData)
            {
                throw new Exception("Testing exception.");
            }
        }

        /// <summary>
        /// Delete all rooms.
        /// </summary>
        public void ClearAll()
        {
            _data.Clear();
        }

        /// <summary>
        /// Initializes the dummy data.
        /// </summary>
        /// <param name="dummyData">The dummy data.</param>
        protected virtual void InitDummyData(IList<T> dummyData)
        { }
    }
}