using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Querying {
    /// <summary>
    /// Base for MochaDB collection results.
    /// </summary>
    /// <typeparam name="T">Type of result value.</typeparam>
    public class MochaCollectionResult<T>:IMochaCollectionResult<T> {
        #region Fields

        internal IEnumerable<T> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaCollectionResult.
        /// </summary>
        public MochaCollectionResult(IEnumerable<T> collection) {
            this.collection=collection;
        }

        #endregion

        #region Methods

        #region Queryable

        /// <summary>
        /// Select items by query.
        /// </summary>
        /// <param name="query">Query to use in filtering.</param>
        public IEnumerable<T> Select(Func<T,T> query) =>
            collection.Select(query);

        /// <summary>
        /// Select items by query.
        /// </summary>
        /// <param name="query">Query to use in filtering.</param>
        public IEnumerable<T> Select(Func<T,int,T> query) =>
            collection.Select(query);

        /// <summary>
        /// Select items by condition.
        /// </summary>
        /// <param name="query">Query to use in conditioning.</param>
        public IEnumerable<T> Where(Func<T,bool> query) =>
            collection.Where(query);

        /// <summary>
        /// Select items by condition.
        /// </summary>
        /// <param name="query">Query to use in conditioning.</param>
        public IEnumerable<T> Where(Func<T,int,bool> query) =>
            collection.Where(query);

        /// <summary>
        /// Order items descending by query.
        /// </summary>
        /// <param name="query">Query to use in ordering.</param>
        public IEnumerable<T> OrderByDescending(Func<T,T> query) =>
            collection.OrderByDescending(query);

        /// <summary>
        /// Order items ascending by query.
        /// </summary>
        /// <param name="query">Query to use in ordering.</param>
        public IEnumerable<T> OrderBy(Func<T,T> query) =>
            collection.OrderBy(query);

        /// <summary>
        /// Group items by query.
        /// </summary>
        /// <param name="query">Query to use in grouping.</param>
        public IEnumerable<IGrouping<T,T>> GroupBy(Func<T,T> query) =>
            collection.GroupBy(query);

        #endregion

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaResult<T> ElementAt(int index) =>
            this[index];

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public T[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<T> ToList() =>
            collection.ToList();

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            Count-1;

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaResult<T> this[int index] =>
            collection.ElementAt(index);

        /// <summary>
        /// Count of items.
        /// </summary>
        public int Count =>
            collection.Count();

        /// <summary>
        /// This is collection result.
        /// </summary>
        public bool IsCollectionResult =>
            false;

        #endregion
    }
}
