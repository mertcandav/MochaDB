using System;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// MochaData collector for MochaColumns.
    /// </summary>
    public class MochaColumnDataCollection:IMochaReadonlyCollection<MochaData> {
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

        #region Methods

        #region Internal

        /// <summary>
        /// Remove all items.
        /// </summary>
        internal void Clear() {
            if(collection.Count ==0)
                return;

            collection.Clear();
            //OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item">Item to add.</param>
        internal void Add(MochaData item) {
            if(DataType==MochaDataType.AutoInt)
                throw new Exception("Data cannot be added directly to a column with AutoInt!");
            if(item.DataType == MochaDataType.Unique && !string.IsNullOrEmpty(item.Data.ToString()))
                if(ContainsData(item.Data))
                    throw new Exception("Any value can be added to a unique column only once!");

            if(item.DataType == DataType) {
                collection.Add(item);
                //Changed?.Invoke(this,new EventArgs());
            } else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">Data to add.</param>
        internal void AddData(object data) {
            if(MochaData.IsType(DataType,data))
                Add(new MochaData(DataType,data));
            else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add item from range.
        /// </summary>
        /// <param name="items">Range to add items.</param>
        internal void AddRange(IEnumerable<MochaData> items) {
            for(int index = 0; index < items.Count(); index++)
                Add(items.ElementAt(index));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        internal void Remove(MochaData item) {
            collection.Remove(item);
            /*if(collection.Remove(item))
                OnChanged(this,new EventArgs());*/
        }

        /// <summary>
        /// Removes all data equal to sample data.
        /// </summary>
        /// <param name="data">Sample data.</param>
        internal void RemoveAllData(object data) {
            int count = collection.Count;
            collection = (
                from currentdata in collection
                where currentdata.Data != data
                select currentdata).ToList();

            /*if(collection.Count != count)
                OnChanged(this,new EventArgs());*/
        }

        /// <summary>
        /// Remove item by index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        internal void RemoveAt(int index) {
            collection.RemoveAt(index);
            //OnChanged(this,new EventArgs());
        }

        #endregion

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
        /// Return true if data is contained but return false if not exists.
        /// </summary>
        /// <param name="data">Data to check.</param>
        public bool ContainsData(object data) {
            for(int index = 0; index < Count; index++)
                if(data ==this[index])
                    return true;

            return false;
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
        public MochaData GetFirst() =>
            IsEmptyCollection() ? null : this[0];

        /// <summary>
        /// Return last element in collection.
        /// </summary>
        public MochaData GetLast() =>
            IsEmptyCollection() ? null : this[MaxIndex()];

        /// <summary>
        /// Return element by index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        public MochaData ElementAt(int index) =>
            collection.ElementAt(index);

        /// <summary>
        /// Create and return static array from collection.
        /// </summary>
        public MochaData[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Create and return List<T> from collection.
        /// </summary>
        public List<MochaData> ToList() =>
            collection.ToList();

        /// <summary>
        /// Returns values in MochaReader.
        /// </summary>
        public MochaReader<MochaData> ToReader() =>
            new MochaReader<MochaData>(collection);

        #endregion

        #region Properties

        /// <summary>
        /// Return item by index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        public MochaData this[int index] =>
            ElementAt(index);

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
        /// Count of items.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
