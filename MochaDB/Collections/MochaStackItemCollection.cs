using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Collections {
    /// <summary>
    /// MochaStackItem collector.
    /// </summary>
    public sealed class MochaStackItemCollection:IMochaCollection<MochaStackItem> {
        #region Fields

        private List<MochaStackItem> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaStackItemCollection.
        /// </summary>
        public MochaStackItemCollection() {
            collection= new List<MochaStackItem>();
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

        #region Item Events

        private void StackItem_NameChanged(object sender,EventArgs e) {
            if(Contains((sender as MochaStackItem).Name))
                throw new Exception("An item with this name already exists!");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove all items.
        /// </summary>
        public void Clear() {
            collection.Clear();
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(MochaStackItem item) {
            if(Contains(item.Name))
                throw new Exception("An item with this name already exists!");

            item.NameChanged+=StackItem_NameChanged;
            collection.Add(item);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaStackItem> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaStackItem item) {
            if(collection.Remove(item))
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Remove item by name.
        /// </summary>
        /// <param name="name">Name of item to remove.</param>
        public void Remove(string name) {
            int dex = IndexOf(name);
            if(dex!=-1)
                RemoveAt(dex);
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            collection[index].NameChanged-=StackItem_NameChanged;
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaStackItem item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaStackItem item) {
            return collection.Contains(item);
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="name">Name of item to find index.</param>
        public int IndexOf(string name) {
            for(int index = 0; index < Count; index++) {
                if(this[index].Name== name)
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="name">Name of item to exists check.</param>
        public bool Contains(string name) =>
            IndexOf(name)!=-1 ? true : false;

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
        public MochaStackItem GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaStackItem GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public MochaStackItem ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public MochaStackItem[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<MochaStackItem> ToList() =>
            collection.ToList();

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaStackItem this[int index] {
            get =>
                ElementAt(index);
            set {
                collection[index]=value;
                OnChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaStackItem this[string name] {
            get {
                int dex = IndexOf(name);
                return dex!=-1 ? this[dex] : throw new Exception("There is no item by this name!");
            }
            set {
                int dex = IndexOf(name);
                this[dex] = dex!=-1 ? value : throw new Exception("There is no item by this name!");
            }
        }

        /// <summary>
        /// Count of item.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
