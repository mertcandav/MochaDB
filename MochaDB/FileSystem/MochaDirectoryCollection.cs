using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.FileSystem {
    /// <summary>
    /// MochaDB file system directory collector.
    /// </summary>
    public class MochaDirectoryCollection:MochaCollection<MochaDirectory>, IMochaCollection<MochaDirectory> {
        #region Constructors

        /// <summary>
        /// Create new MochaDirectoryCollection.
        /// </summary>
        public MochaDirectoryCollection() {
            collection =new List<MochaDirectory>();
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens after NameChanged event of any directory in collection.
        /// </summary>
        public event EventHandler<EventArgs> DirectoryNameChanged;
        private void OnDirectoryNameChanged(object sender,EventArgs e) {
            //Invoke.
            DirectoryNameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_NameChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as IMochaDirectory).Name);
            if(result.Count()>1)
                throw new Exception("There is already a directory with this name!");

            OnDirectoryNameChanged(sender,e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove all items.
        /// </summary>
        public void Clear() {
            for(int index = 0; index < Count; index++) {
                collection[index].NameChanged-=Item_NameChanged;
            }
            collection.Clear();
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(MochaDirectory item) {
            if(item == null)
                return;
            if(Contains(item.Name))
                throw new Exception("There is already a directory with this name!");

            item.NameChanged+=Item_NameChanged;
            collection.Add(item);
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaDirectory> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaDirectory item) {
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
        /// <param name="name">Name of item to exists check.</param>
        public bool Contains(string name) {
            return IndexOf(name) != -1;
        }

        /// <summary>
        /// Return first element in collection.
        /// </summary>
        public MochaDirectory GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaDirectory GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        #endregion

        #region Properties

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaDirectory this[string name] {
            get {
                int dex = IndexOf(name);
                if(dex!=-1)
                    return ElementAt(dex);
                return null;
            }
        }

        #endregion
    }
}
