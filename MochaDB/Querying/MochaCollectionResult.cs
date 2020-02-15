using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Querying {
    /// <summary>
    /// Base for MochaDB collection results.
    /// </summary>
    /// <typeparam name="T">Type of results.</typeparam>
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

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public T ElementAt(int index) =>
            this[index];

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
        public T this[int index] =>
            collection.ElementAt(index);

        /// <summary>
        /// Count of items.
        /// </summary>
        public int Count =>
            collection.Count();

        #endregion
    }
}
