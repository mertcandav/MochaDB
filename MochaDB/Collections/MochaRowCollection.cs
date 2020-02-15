using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Collections {
    /// <summary>
    /// MochaRow collector.
    /// </summary>
    public sealed class MochaRowCollection:IMochaCollection<MochaRow> {
        #region Fields

        internal List<MochaRow> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaRowCollection.
        /// </summary>
        public MochaRowCollection() {
            collection=new List<MochaRow>();
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

        /// <summary>
        /// This happens after Changed event of any row in collection.
        /// </summary>
        public event EventHandler<EventArgs> RowChanged;
        private void OnRowChanged(object sender,EventArgs e) {
            //Invoke.
            RowChanged?.Invoke(sender,e);
        }

        #endregion

        #region Rows Events

        private void Row_Changed(object sender,EventArgs e) {
            OnRowChanged(sender,e);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Remove all items.
        /// </summary>
        public void Clear() {
            for(int index = 0; index < Count; index++)
                RemoveAt(index);
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(MochaRow item) {
            if(item== null)
                return;

            collection.Add(item);
            item.Datas.Changed+=Row_Changed;
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaRow> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaRow item) {
            int dex = IndexOf(item);
            if(dex!=-1)
                RemoveAt(dex);
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            collection[index].Datas.Changed-=Row_Changed;
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaRow item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaRow item) {
            return collection.Contains(item);
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            collection.Count-1;

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaRow this[int index] =>
            collection[index];

        /// <summary>
        /// Count of data.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
