using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaRow collector.
    /// </summary>
    public class MochaRowCollection:MochaCollection<MochaRow>, IMochaCollection<MochaRow> {
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
        /// This happens after Changed event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> RowChanged;
        private void OnRowChanged(object sender,EventArgs e) {
            //Invoke.
            RowChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_Changed(object sender,EventArgs e) {
            OnRowChanged(sender,e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove all items.
        /// </summary>
        public void Clear() {
            for(int index = 0; index < Count; index++) {
                collection[index].Datas.Changed-=Item_Changed;
            }
            collection.Clear();
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(MochaRow item) {
            if(item== null)
                return;

            collection.Add(item);
            item.Datas.Changed+=Item_Changed;
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="datas">Datas of item.</param>
        public void Add(params object[] datas) =>
            Add(new MochaRow(datas));

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="datas">Datas of item.</param>
        public void Add(params MochaData[] datas) =>
            Add(new MochaRow(datas));

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="datas">Datas of item.</param>
        public void Add(IEnumerable<MochaData> datas) =>
            Add(new MochaRow(datas));

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
            collection[index].Datas.Changed-=Item_Changed;
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return first element in collection.
        /// </summary>
        public MochaRow GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaRow GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        #endregion
    }
}
