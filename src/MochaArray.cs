using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// Static array for MochaDB.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    public class MochaArray<T>:IEnumerable,IEnumerable<T> {
        #region Fields

        internal T[] array;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaArray.
        /// </summary>
        /// <param name="capacity">Capacity of items.</param>
        public MochaArray(int capacity) {
            array = new T[capacity];
        }

        /// <summary>
        /// Create a new MochaArray.
        /// </summary>
        /// <param name="item">Items of array.</param>
        public MochaArray(params T[] items) {
            array = items.ToArray();
        }

        /// <summary>
        /// Create a new MochaArray.
        /// </summary>
        /// <param name="item">Items of array.</param>
        public MochaArray(IEnumerable<T> items) {
            array = items.ToArray();
        }

        #endregion

        #region Operators

        public static implicit operator T[](MochaArray<T> value) =>
            value.array.ToArray();

        public static implicit operator MochaArray<T>(T[] value) =>
            new MochaArray<T>(value.Length) {
                array = value.ToArray()
            };

        #endregion

        #region Queryable

        /// <summary>
        /// Select items by query.
        /// </summary>
        /// <param name="query">Query to use in filtering.</param>
        public IEnumerable<T> Select(Func<T,T> query) =>
            array.Select(query);

        /// <summary>
        /// Select items by query.
        /// </summary>
        /// <param name="query">Query to use in filtering.</param>
        public IEnumerable<T> Select(Func<T,int,T> query) =>
            array.Select(query);

        /// <summary>
        /// Select items by condition.
        /// </summary>
        /// <param name="query">Query to use in conditioning.</param>
        public IEnumerable<T> Where(Func<T,bool> query) =>
            array.Where(query);

        /// <summary>
        /// Select items by condition.
        /// </summary>
        /// <param name="query">Query to use in conditioning.</param>
        public IEnumerable<T> Where(Func<T,int,bool> query) =>
            array.Where(query);

        /// <summary>
        /// Order items descending by query.
        /// </summary>
        /// <param name="query">Query to use in ordering.</param>
        public IEnumerable<T> OrderByDescending(Func<T,T> query) =>
            array.OrderByDescending(query);

        /// <summary>
        /// Order items ascending by query.
        /// </summary>
        /// <param name="query">Query to use in ordering.</param>
        public IEnumerable<T> OrderBy(Func<T,T> query) =>
            array.OrderBy(query);

        /// <summary>
        /// Group items by query.
        /// </summary>
        /// <param name="query">Query to use in grouping.</param>
        public IEnumerable<IGrouping<T,T>> GroupBy(Func<T,T> query) =>
            array.GroupBy(query);

        #endregion

        #region Methods

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(T item) {
            for(int index = 0; index < Length; index++)
                if(this[index].Equals(item))
                    return index;

            return -1;
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(T item) {
            return array.Contains(item);
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            array.Length-1;

        /// <summary>
        /// Return true if is empty collection but return false if not.
        /// </summary>
        public bool IsEmptyCollection() =>
            array.Length == 0 ? true : false;

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public T ElementAt(int index) =>
            array.ElementAt(index);

        /// <summary>
        /// Set element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        /// <param name="item">Item to set.</param>
        public T SetElement(int index,T item) =>
            array[index] = item;

        /// <summary>
        /// Create and return copy this collection.
        /// </summary>
        public MochaArray<T> Clone() =>
            array.ToArray();

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public T[] ArrayClone() =>
            array.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<T> ToList() =>
            array.ToList();

        /// <summary>
        /// Returns values in MochaReader.
        /// </summary>
        public MochaReader<T> ToReader() =>
            new MochaReader<T>(array);

        /// <summary>
        /// Return first element in collection.
        /// </summary>
        public T GetFirst() =>
            IsEmptyCollection() ? throw new MochaException("Collection is empty!") : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public T GetLast() =>
            IsEmptyCollection() ? throw new MochaException("Collection is empty!") : this[MaxIndex()];

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() =>
            array.GetEnumerator();

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        public IEnumerator<T> GetEnumerator() {
            return array.ToList().GetEnumerator();
        }

        /// <summary>
        /// Copy items to array by start index.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Index to start copying.</param>
        public void CopyTo(T[] array,int arrayIndex) {
            array.CopyTo(array,arrayIndex);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Count of item.
        /// </summary>
        public int Length =>
            array.Length;

        /// <summary>
        /// Returns or set item by index.
        /// </summary>
        /// <param name="index">Index.</param>
        public T this[int index] {
            get => ElementAt(index);
            set => SetElement(index,value);
        }

        #endregion
    }
}
