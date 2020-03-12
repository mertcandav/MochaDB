using System;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// MochaRow collector.
    /// </summary>
    public class MochaRowCollection:IMochaCollection<MochaRow> {
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
            Changed?.Invoke(this,e);
        }

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

        /// <summary>
        /// Return true if is empty collection but return false if not.
        /// </summary>
        public bool IsEmptyCollection() =>
            collection.Count == 0 ? true : false;

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

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public MochaRow ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public MochaRow[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<MochaRow> ToList() =>
            collection.ToList();

        /// <summary>
        /// Returns values in MochaReader.
        /// </summary>
        public MochaReader<MochaRow> ToReader() =>
            new MochaReader<MochaRow>(collection);

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaRow this[int index] =>
            ElementAt(index);

        /// <summary>
        /// Count of items.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
