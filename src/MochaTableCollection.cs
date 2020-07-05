using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaTable collector.
    /// </summary>
    public class MochaTableCollection:MochaCollection<MochaTable> {
        #region Constructors

        /// <summary>
        /// Create a new MochaTableCollection.
        /// </summary>
        public MochaTableCollection() {
            collection=new List<MochaTable>();
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens after NameChanged event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> TableNameChanged;
        private void OnTableNameChanged(object sender,EventArgs e) {
            //Invoke.
            TableNameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_NameChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as MochaStackItem).Name);
            if(result.Count()>1)
                throw new MochaException("There is already a table with this name!");

            OnTableNameChanged(sender,e);
        }

        #endregion

        #region Methods

        public override void Clear() {
            for(int index = 0; index < Count; index++) {
                collection[index].NameChanged-=Item_NameChanged;
            }
            collection.Clear();
        }

        public override void Add(MochaTable item) {
            if(Contains(item.Name))
                throw new MochaException("There is already a table with this name!");

            item.NameChanged+=Item_NameChanged;
            collection.Add(item);
            OnChanged(this,new EventArgs());
        }

        public override void AddRange(IEnumerable<MochaTable> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        public override void Remove(MochaTable item) {
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

        public override void RemoveAt(int index) {
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

        public override MochaTable GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        public override MochaTable GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public new MochaTable this[int index] {
            get =>
                ElementAt(index);
            set {
                if(Contains(value.Name))
                    throw new MochaException("There is already a table with this name!");

                collection[index]=value;
                OnChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaTable this[string name] {
            get {
                int dex = IndexOf(name);
                return dex!=-1 ? this[dex] : throw new MochaException("There is no item by this name!");
            }
            set {
                int dex = IndexOf(name);
                this[dex] = dex!=-1 ? value : throw new MochaException("There is no item by this name!");
            }
        }

        #endregion
    }
}
