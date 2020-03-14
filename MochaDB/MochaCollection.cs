using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// Abstract base class for MochaDB collections.
    /// </summary>    
    /// <typeparam name="T">Item type.</typeparam>
    public abstract class MochaCollection<T>:IEnumerable<T> {
        #region Fields

        internal protected List<T> collection;

        #endregion

        #region Events

        /// <summary>
        /// This happens after collection changed.
        /// </summary>
        public event EventHandler<EventArgs> Changed;
        protected virtual void OnChanged(object sender,EventArgs e) {
            //Invoke.
            Changed?.Invoke(this,e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public virtual int IndexOf(T item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public virtual bool Contains(T item) {
            return collection.Contains(item);
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public virtual int MaxIndex() =>
            collection.Count-1;

        /// <summary>
        /// Return true if is empty collection but return false if not.
        /// </summary>
        public virtual bool IsEmptyCollection() =>
            collection.Count == 0 ? true : false;

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public virtual T ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public virtual T[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public virtual List<T> ToList() =>
            collection.ToList();

        /// <summary>
        /// Returns values in MochaReader.
        /// </summary>
        public virtual MochaReader<T> ToReader() =>
            new MochaReader<T>(collection);

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        public virtual IEnumerator<T> GetEnumerator() =>
            collection.GetEnumerator();

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() =>
            collection.GetEnumerator();

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public virtual T this[int index] =>
            ElementAt(index);

        /// <summary>
        /// Count of items.
        /// </summary>
        public virtual int Count =>
            collection.Count;

        #endregion
    }
}
