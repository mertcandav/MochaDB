using System;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// MochaData collector.
    /// </summary>
    public class MochaDataCollection:IMochaCollection<MochaData> {
        #region Fields

        internal List<MochaData> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaDataCollection.
        /// </summary>
        public MochaDataCollection() {
            collection=new List<MochaData>();
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens after collection changed.
        /// </summary>
        public event EventHandler<EventArgs> Changed;
        private void OnChanged(object sender,EventArgs e) {
            //Invoke.
            Changed?.Invoke(this,new EventArgs());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove all items.
        /// </summary>
        public void Clear() {
            if(collection.Count==0)
                return;

            collection.Clear();
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(MochaData item) {
            collection.Add(item);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaData> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaData item) {
            if(collection.Remove(item))
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Removes all data equal to sample data.
        /// </summary>
        /// <param name="data">Sample data.</param>
        public void RemoveAllData(object data) {
            int count = collection.Count;
            collection = (
                from currentdata in collection
                where currentdata.Data != data
                select currentdata).ToList();

            if(collection.Count != count)
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaData item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaData item) {
            return collection.Contains(item);
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            collection.Count-1;

        /// <summary>
        /// Return true if is empty collection but return false if not.
        /// </summary>
        public bool IsEmptyCollection() =>
            collection.Count == 0 ? true : false;

        /// <summary>
        /// Return first element in collection.
        /// </summary>
        public MochaData GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaData GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public MochaData ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public MochaData[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<MochaData> ToList() =>
            collection.ToList();

        /// <summary>
        /// Returns values in MochaReader.
        /// </summary>
        public MochaReader<MochaData> ToReader() =>
            new MochaReader<MochaData>(collection);

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaData this[int index] {
            get =>
                ElementAt(index);
            set {
                collection[index] = value;
                OnChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Count of items.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
