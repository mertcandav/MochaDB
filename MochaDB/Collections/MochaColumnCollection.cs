using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Collections {
    /// <summary>
    /// MochaColumn collector.
    /// </summary>
    public sealed class MochaColumnCollection:IMochaCollection<MochaColumn> {
        #region Fields

        internal List<MochaColumn> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaColumnCollection.
        /// </summary>
        public MochaColumnCollection() {
            collection =new List<MochaColumn>();
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
        /// This happens after Changed event of any column in collection.
        /// </summary>
        public event EventHandler<EventArgs> ColumnChanged;
        private void OnColumnChanged(object sender,EventArgs e) {
            //Invoke.
            ColumnChanged?.Invoke(sender,e);
        }

        #endregion

        #region Column Events

        private void Column_Changed(object sender,EventArgs e) {
            OnColumnChanged(sender,e);
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
        public void Add(MochaColumn item) {
            if(item == null)
                return;

            if(!Contains(item.Name)) {
                item.Datas.Changed+=Column_Changed;
                collection.Add(item);
            } else
                throw new Exception("There is no such table or there is already a table with this name!");
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaColumn> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaColumn item) {
            Remove(item.Name);
        }

        /// <summary>
        /// Remove column by name.
        /// </summary>
        /// <param name="name">Name of column to remove.</param>
        public void Remove(string name) {
            for(int index = 0; index < Count; index++)
                if(collection[index].Name == name) {
                    collection[index].Datas.Changed-=Column_Changed;
                    collection.RemoveAt(index);
                    break;
                }
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            Remove(collection[index].Name);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaColumn item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="name">Name of column to find index.</param>
        public int IndexOf(string name) {
            for(int index = 0; index < Count; index++)
                if(this[index].Name==name)
                    return index;
            return -1;
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaColumn item) {
            return Contains(item.Name);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="name">Name of columnt to exists check.</param>
        public bool Contains(string name) {
            return IndexOf(name) != -1;
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
        public MochaColumn this[int index] =>
            collection[index];

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of column.</param>
        public MochaColumn this[string name] {
            get {
                int dex = IndexOf(name);
                if(dex!=-1)
                    return collection[dex];
                return null;
            }
        }

        /// <summary>
        /// Count of data.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
