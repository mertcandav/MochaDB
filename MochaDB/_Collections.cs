using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB {
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

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaStackItem this[int index] {
            get =>
                collection[index];
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

    /// <summary>
    /// MochaData collector.
    /// </summary>
    public sealed class MochaDataCollection:IMochaCollection<MochaData> {
        #region Fields

        internal List<MochaData> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaDataCollection.
        /// </summary>
        public MochaDataCollection() {
            collection=new List<MochaData>();
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
        public void Add(MochaData item) {
            collection.Add(item);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaData> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaData item) {
            if(collection.Remove(item))
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Removes all data equal to sample data.
        /// </summary>
        /// <param name="data">Sample data.</param>
        public void RemoveAllData(object data) {
            int count = collection.Count;
            collection = (
                from currentdata in collection
                where currentdata.Data != data
                select currentdata).ToList();

            if(collection.Count != count)
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaData item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaData item) {
            return collection.Contains(item);
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            collection.Count-1;

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaData this[int index] {
            get =>
                collection[index];
            set {
                collection[index] = value;
                OnChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Count of data.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }

    /// <summary>
    /// MochaData collector for MochaColumns.
    /// </summary>
    public sealed class MochaColumnDataCollection:IMochaCollection<MochaData> {
        #region Fields

        internal List<MochaData> collection;
        private MochaDataType dataType;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaColumnDataCollection.
        /// </summary>
        /// <param name="dataType">DataType of column.</param>
        public MochaColumnDataCollection(MochaDataType dataType) {
            collection=new List<MochaData>();
            this.dataType=dataType;
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
        public void Add(MochaData item) {
            if(DataType==MochaDataType.AutoInt)
                throw new Exception("Data cannot be added directly to a column with AutoInt!");

            if(item.DataType == DataType) {
                collection.Add(item);
                Changed?.Invoke(this,new EventArgs());
            } else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">Data to add.</param>
        public void AddData(object data) {
            if(MochaData.IsType(DataType,data))
                AddData(new MochaData(DataType,data));
            else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        public void AddRange(IEnumerable<MochaData> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(MochaData item) {
            if(collection.Remove(item))
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Removes all data equal to sample data.
        /// </summary>
        /// <param name="data">Sample data.</param>
        public void RemoveAllData(object data) {
            int count = collection.Count;
            collection = (
                from currentdata in collection
                where currentdata.Data != data
                select currentdata).ToList();

            if(collection.Count != count)
                OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            collection.RemoveAt(index);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaData item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return true if item is exists but return false if item not exists.
        /// </summary>
        /// <param name="item">Item to exists check.</param>
        public bool Contains(MochaData item) {
            return collection.Contains(item);
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            collection.Count-1;

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaData this[int index] =>
            collection[index];

        /// <summary>
        /// Data type of column.
        /// </summary>
        public MochaDataType DataType {
            get =>
                dataType;
            internal set {
                if(value == dataType)
                    return;

                dataType = value;

                if(value == MochaDataType.AutoInt) {
                    return;
                }

                for(int index = 0; index < Count; index++)
                    collection[index].DataType = dataType;
            }
        }

        /// <summary>
        /// Count of data.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }

    /// <summary>
    /// MochaColumn collector.
    /// </summary>
    public sealed class MochaColumnCollection:IMochaCollection<MochaColumn> {
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
        /// This happens after Changed event of any column in collection.
        /// </summary>
        public event EventHandler<EventArgs> ColumnChanged;
        private void OnColumnChanged(object sender,EventArgs e) {
            //Invoke.
            ColumnChanged?.Invoke(sender,e);
        }

        #endregion

        #region Column Events

        private void Column_Changed(object sender,EventArgs e) {
            OnColumnChanged(sender,e);
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

            if(!Contains(item.Name)) {
                item.Datas.Changed+=Column_Changed;
                collection.Add(item);
            } else
                throw new Exception("There is no such table or there is already a table with this name!");
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
        /// Remove column by name.
        /// </summary>
        /// <param name="name">Name of column to remove.</param>
        public void Remove(string name) {
            for(int index = 0; index < Count; index++)
                if(collection[index].Name == name) {
                    collection[index].Datas.Changed-=Column_Changed;
                    collection.RemoveAt(index);
                    break;
                }
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index) {
            Remove(collection[index].Name);
            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="item">Item to find index.</param>
        public int IndexOf(MochaColumn item) {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Return index if index is find but return -1 if index is not find.
        /// </summary>
        /// <param name="name">Name of column to find index.</param>
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
        /// <param name="name">Name of columnt to exists check.</param>
        public bool Contains(string name) {
            return IndexOf(name) != -1;
        }

        /// <summary>
        /// Return max index of item count.
        /// </summary>
        public int MaxIndex() =>
            collection.Count-1;

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaColumn this[int index] =>
            collection[index];

        /// <summary>
        /// Return item by name.
        /// </summary>
        /// <param name="name">Name of column.</param>
        public MochaColumn this[string name] {
            get {
                int dex = IndexOf(name);
                if(dex!=-1)
                    return collection[dex];
                return null;
            }
        }

        /// <summary>
        /// Count of data.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }

    /// <summary>
    /// MochaRow collector.
    /// </summary>
    public sealed class MochaRowCollection:IMochaCollection<MochaRow> {
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
            Changed?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// This happens after Changed event of any row in collection.
        /// </summary>
        public event EventHandler<EventArgs> RowChanged;
        private void OnRowChanged(object sender,EventArgs e) {
            //Invoke.
            RowChanged?.Invoke(sender,e);
        }

        #endregion

        #region Rows Events

        private void Row_Changed(object sender,EventArgs e) {
            OnRowChanged(sender,e);
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
        public void Add(MochaRow item) {
            if(item== null)
                return;

            collection.Add(item);
            item.Datas.Changed+=Row_Changed;
            OnChanged(this,new EventArgs());
        }

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
            collection[index].Datas.Changed-=Row_Changed;
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

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaRow this[int index] =>
            collection[index];

        /// <summary>
        /// Count of data.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
