using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.FileSystem {
    /// <summary>
    /// MochaDb file system file collector.
    /// </summary>
    public class MochaFileCollection:IMochaCollection<MochaFile> {
        #region Fields

        internal List<MochaFile> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaFileCollection.
        /// </summary>
        public MochaFileCollection() {
            collection =new List<MochaFile>();
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
        /// This happens after NameChanged event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> FileNameChanged;
        private void OnFileNameChanged(object sender,EventArgs e) {
            //Invoke.
            FileNameChanged?.Invoke(sender,e);
        }

        /// <summary>
        /// This happens after NameChanged event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> FileExtensionChanged;
        private void OnFileExtensionChanged(object sender,EventArgs e) {
            //Invoke.
            FileExtensionChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_NameChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as IMochaFile).Name);
            if(result.Count()>1)
                throw new Exception("There is already a file with this name and extension!");

            OnFileNameChanged(sender,e);
        }

        private void Item_ExtensionChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as IMochaFile).Name);
            if(result.Count()>1)
                throw new Exception("There is already a file with this name and extension!");

            OnFileExtensionChanged(sender,e);
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
        public void Add(MochaFile item) {
            if(item == null)
                return;
            if(Contains(item.FullName))
                throw new Exception("There is already a file with this name and extension!");

            item.NameChanged+=Item_NameChanged;
            item.ExtensionChanged+=Item_ExtensionChanged;
            collection.Add(item);
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaFile> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaFile item) {
            Remove(item.Name);
        }

        /// <summary>
        /// Remove item by name.
        /// </summary>
        /// <param name="fullName">FullName of item to remove.</param>
        public void Remove(string fullName) {
            for(int index = 0; index < Count; index++)
                if(collection[index].Name == fullName) {
                    collection[index].NameChanged-=Item_NameChanged;
                    collection[index].ExtensionChanged-=Item_ExtensionChanged;
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
        public int IndexOf(MochaFile item) {
            return IndexOf(item.Name);
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="fullName">FullName of item to find index.</param>
        public int IndexOf(string fullName) {
            for(int index = 0; index < Count; index++)
                if(this[index].FullName==fullName)
                    return index;
            return -1;
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaFile item) {
            return Contains(item.Name);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="fullName">FullName of item to exists check.</param>
        public bool Contains(string fullName) {
            return IndexOf(fullName) != -1;
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
        public MochaFile GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaFile GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public MochaFile ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public MochaFile[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<MochaFile> ToList() =>
            collection.ToList();

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaFile this[int index] =>
            ElementAt(index);

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaFile this[string name] {
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