using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaData collector.
    /// </summary>
    public class MochaDataCollection:MochaCollection<MochaData>, IMochaCollection<MochaData> {
        #region Constructors

        /// <summary>
        /// Create new MochaDataCollection.
        /// </summary>
        public MochaDataCollection() {
            collection=new List<MochaData>();
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
        /// Return first element in collection.
        /// </summary>
        public MochaData GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaData GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        #endregion
    }
}
