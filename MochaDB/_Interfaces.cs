using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// Collection interface for MochaDB.
    /// </summary>
    /// <typeparam name="T">Type of collector.</typeparam>
    public interface IMochaCollection<T> {
        #region Events

        public event EventHandler<EventArgs> Changed;

        #endregion

        #region Methods

        public void Clear();
        public void Add(T item);
        public void AddRange(IEnumerable<T> items);
        public void Remove(T item);
        public void RemoveAt(int index);
        public int IndexOf(T item);
        public bool Contains(T item);
        public int MaxIndex();

        #endregion

        #region Properties

        public int Count { get; }
        public T this[int index] { get; }

        #endregion
    }
}
