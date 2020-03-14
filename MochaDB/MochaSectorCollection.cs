﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaSector collector.
    /// </summary>
    public class MochaSectorCollection:MochaCollection<MochaSector>, IMochaCollection<MochaSector> {
        #region Constructors

        /// <summary>
        /// Create a new MochaSectorCollection.
        /// </summary>
        public MochaSectorCollection() {
            collection=new List<MochaSector>();
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens after NameChanged event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> SectorNameChanged;
        private void OnSectorNameChanged(object sender,EventArgs e) {
            //Invoke.
            SectorNameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_NameChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as IMochaStackItem).Name);
            if(result.Count()>1)
                throw new Exception("There is already a sector with this name!");

            OnSectorNameChanged(sender,e);
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
        public void Add(MochaSector item) {
            if(Contains(item.Name))
                throw new Exception("There is already a sector with this name!");

            item.NameChanged+=Item_NameChanged;
            collection.Add(item);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaSector> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaSector item) {
            Remove(item.Name);
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
            collection[index].NameChanged-=Item_NameChanged;
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
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
        /// Return first element in collection.
        /// </summary>
        public MochaSector GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaSector GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public new MochaSector this[int index] {
            get =>
                ElementAt(index);
            set {
                if(Contains(value.Name))
                    throw new Exception("");

                collection[index]=value;
                OnChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaSector this[string name] {
            get {
                int dex = IndexOf(name);
                return dex!=-1 ? this[dex] : throw new Exception("There is no item by this name!");
            }
            set {
                int dex = IndexOf(name);
                this[dex] = dex!=-1 ? value : throw new Exception("There is no item by this name!");
            }
        }

        #endregion
    }
}
