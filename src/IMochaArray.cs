using System.Collections;
using System.Collections.Generic;
using MochaDB.Mochaq;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB static arrays.
    /// </summary>
    /// <typeparam name="T">Type of array items.</typeparam>
    public interface IMochaArray<T>:IEnumerable<T>, IEnumerable, IMochaQueryableCollection<T> {
        #region Methods

        int IndexOf(T item);
        bool Contains(T item);
        int MaxIndex();
        bool IsEmptyCollection();
        T ElementAt(int index);
        T SetElement(int index,T item);
        MochaArray<T> Clone();
        T[] ArrayClone();
        List<T> ToList();
        MochaReader<T> ToReader();
        MochaWriter<T> ToWriter();
        void CopyTo(T[] array,int arrayIndex);

        #endregion

        #region Properties

        T this[int index] { get; set; }
        int Length { get; }

        #endregion
    }
}
