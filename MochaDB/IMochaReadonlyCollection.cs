using System.Collections;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// Readonly collection interface for MochaDB.
    /// </summary>
    /// <typeparam name="T">Item type of collector.</typeparam>
    public interface IMochaReadonlyCollection<T>:IEnumerable<T>, IEnumerable {
        #region Methods

        int IndexOf(T item);
        bool Contains(T item);
        int MaxIndex();
        bool IsEmptyCollection();
        void CopyTo(T[] array,int arrayIndex);
        T ElementAt(int index);
        T[] ToArray();
        List<T> ToList();
        T GetFirst();
        T GetLast();

        #endregion

        #region Properties

        bool IsReadOnly { get; }
        int Count { get; }
        T this[int index] { get; }

        #endregion
    }
}
