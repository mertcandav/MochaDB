using System;
using System.Collections.Generic;

namespace MochaDB.Collections {
    /// <summary>
    /// Collection interface for MochaDB.
    /// </summary>
    /// <typeparam name="T">Type of collector.</typeparam>
    public interface IMochaCollection<T> {
        #region Events

        event EventHandler<EventArgs> Changed;

        #endregion

        #region Methods

        void Clear();
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Remove(T item);
        void RemoveAt(int index);
        int IndexOf(T item);
        bool Contains(T item);
        int MaxIndex();

        #endregion

        #region Properties

        int Count { get; }
        T this[int index] { get; }

        #endregion
    }
}
