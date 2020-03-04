using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaColumn collector.
    /// </summary>
    public class MochaColumnCollection:IMochaCollection<MochaColumn> {
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
        /// This happens after Changed event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> ColumnChanged;
        private void OnColumnChanged(object sender,EventArgs e) {
            //Invoke.
            ColumnChanged?.Invoke(sender,e);
        }

        /// <summary>
        /// This happens after NameChanged event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> ColumnNameChanged;
        private void OnColumnNameChanged(object sender,EventArgs e) {
            //Invoke.
            ColumnNameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_Changed(object sender,EventArgs e) {
            OnColumnChanged(sender,e);
        }

        private void Item_NameChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as IMochaColumn).Name);
            if(result.Count() >1)
                throw new Exception("There is already a column with this name!");

            OnColumnNameChanged(sender,e);
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
            if(Contains(item.Name))
                throw new Exception("There is already a column with this name!");

            item.NameChanged+=Item_NameChanged;
            item.Datas.Changed+=Item_Changed;
            collection.Add(item);
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
        /// Remove item by name.
        /// </summary>
        /// <param name="name">Name of item to remove.</param>
        public void Remove(string name) {
            for(int index = 0; index < Count; index++)
                if(collection[index].Name == name) {
                    collection[index].NameChanged-=Item_NameChanged;
                    collection[index].Datas.Changed-=Item_Changed;
                    collection.RemoveAt(index);
                    OnChanged(this,new EventArgs());
                    break;
                }
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            Remove(collection[index].Name);
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaColumn item) {
            return IndexOf(item.Name);
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="name">Name of item to find index.</param>
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
        /// <param name="name">Name of item to exists check.</param>
        public bool Contains(string name) {
            return IndexOf(name) != -1;
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
        public MochaColumn GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaColumn GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public MochaColumn ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public MochaColumn[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<MochaColumn> ToList() =>
            collection.ToList();

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaColumn this[int index] =>
            ElementAt(index);

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaColumn this[string name] {
            get {
                int dex = IndexOf(name);
                if(dex!=-1)
                    return ElementAt(dex);
                return null;
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
