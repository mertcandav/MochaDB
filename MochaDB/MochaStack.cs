using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is stack object for MochaDB.
    /// </summary>
    [Serializable]
    public sealed class MochaStack {
        #region Constructors

        /// <summary>
        /// Create new MochaStack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public MochaStack(string name) {
            Items= new MochaStackItemCollection();
            Name=name;
            Description=string.Empty;
        }

        /// <summary>
        /// Create new MochaStack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="description">Description of stack.</param>
        public MochaStack(string name,string description) :
            this(name) {
            Description=description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of stack.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of stack.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Items of stack.
        /// </summary>
        public MochaStackItemCollection Items { get; }

        #endregion
    }

    /// <summary>
    /// Item for stacks.
    /// </summary>
    public sealed class MochaStackItem {
        #region Fields

        private string name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaStackItem.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaStackItem(string name) {
            Items= new MochaStackItemCollection();
            Name=name;
            Description=string.Empty;
            Value=string.Empty;
        }

        /// <summary>
        /// Create new MochaStackItem.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="description">Description of item.</param>
        public MochaStackItem(string name,string description) :
            this(name) {
            Description =description;
        }

        /// <summary>
        /// Create new MochaStackItem.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="description">Description of item.</param>
        /// <param name="value">Value of item.</param>
        public MochaStackItem(string name,string description,string value) :
            this(name,description) {
            Value=value;
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens before name changed.
        /// </summary>
        public event EventHandler<EventArgs> NameChanged;
        private void OnNameChanged(object sender,EventArgs e) {
            //Invoke.
            NameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of item.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                if(value==name)
                    return;

                name=value;
                OnNameChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Value of item.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Description of item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Items of item.
        /// </summary>
        public MochaStackItemCollection Items { get; }

        #endregion
    }

    /// <summary>
    /// MochaStackItem collector.
    /// </summary>
    public sealed class MochaStackItemCollection {
        #region Fields

        private List<MochaStackItem> items;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaStackItemCollection.
        /// </summary>
        public MochaStackItemCollection() {
            items= new List<MochaStackItem>();
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
        /// Add item.
        /// </summary>
        /// <param name="item">MochaStackItem object to add.</param>
        public void Add(MochaStackItem item) {
            if(Contains(item.Name))
                throw new Exception("An item with this name already exists!");

            item.NameChanged+=StackItem_NameChanged;
            items.Add(item);
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
            items[index].NameChanged-=StackItem_NameChanged;
            items.RemoveAt(index);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="name">Name of item to exists check.</param>
        public bool Contains(string name) =>
            IndexOf(name)!=-1 ? true : false;

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
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            items.Count-1;

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaStackItem this[int index] =>
            items[index];

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaStackItem this[string name] {
            get {
                int dex = IndexOf(name);
                return dex!=-1 ? this[dex] : throw new Exception("There is no item by this name!");
            }
        }

        /// <summary>
        /// Count of item.
        /// </summary>
        public int Count =>
            items.Count;

        #endregion
    }
}