using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaStackItem collector.
    /// </summary>
    public class MochaStackItemCollection:MochaCollection<MochaStackItem> {
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
        /// This happens after NameChanged event of any item in collection.
        /// </summary>
        public event EventHandler<EventArgs> StackItemNameChanged;
        private void OnStackItemNameChanged(object sender,EventArgs e) {
            //Invoke.
            StackItemNameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Item Events

        private void Item_NameChanged(object sender,EventArgs e) {
            var result = collection.Where(x => x.Name==(sender as IMochaStackItem).Name);
            if(result.Count()>1)
                throw new MochaException("There is already a stack item with this name!");

            OnStackItemNameChanged(sender,e);
        }

        #endregion

        #region Methods

        public override void Clear() {
            for(int index = 0; index < Count; index++) {
                collection[index].NameChanged-=Item_NameChanged;
            }
            collection.Clear();
        }

        public override void Add(MochaStackItem item) {
            if(Contains(item.Name))
                throw new MochaException("There is already a stack item with this name!");

            item.NameChanged+=Item_NameChanged;
            collection.Add(item);
            OnChanged(this,new EventArgs());
        }

        public override void AddRange(IEnumerable<MochaStackItem> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        public override void Remove(MochaStackItem item) {
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

        public override MochaStackItem GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        public override MochaStackItem GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public new MochaStackItem this[int index] {
            get =>
                ElementAt(index);
            set {
                if(Contains(value.Name))
                    throw new MochaException("There is already a stack item with this name!");

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
