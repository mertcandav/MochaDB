using System.Collections.Generic;

namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB collection results.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IMochaCollectionResult<T>:IMochaQueryableCollection<T>, IMochaResult, IEnumerable<T> {
        #region Methods

        T ElementAt(int index);
        T[] ToArray();
        List<T> ToList();
        int MaxIndex();
        bool IsEmptyCollection();

        #endregion

        #region Properties

        T this[int index] { get; }
        int Count { get; }

        #endregion
    }
}
