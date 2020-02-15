using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB database management classes.
    /// </summary>
    public interface IMochaDatabase:IDisposable {
        #region Events

        public event EventHandler<EventArgs> ChangeContent;

        #endregion

        #region Methods

        public void Connect();
        public void Disconnect();

        public void AddSector(MochaSector sector);
        public void RemoveSector(string name);
        public MochaSector GetSector(string name);
        public bool ExistsSector(string name);

        public void AddStack(MochaStack stack);
        public void RemoveStack(string name);
        public void AddStackItem(string name,string path,MochaStackItem item);
        public void RemoveStackItem(string name,string path);
        public MochaStack GetStack(string name);
        public bool ExistsStack(string name);
        public bool ExistsStackItem(string name,string path);

        public void AddTable(MochaTable table);
        public void RemoveTable(string name);
        public MochaTable GetTable(string name);
        public bool ExistsTable(string name);

        public void AddColumn(string tableName,MochaColumn column);
        public void RemoveColumn(string tableName,string name);
        public MochaColumn GetColumn(string tableName,string name);
        public bool ExistsColumn(string tableName,string name);

        public void AddRow(string tableName,MochaRow row);
        public void RemoveRow(string tableName,int index);
        public MochaRow GetRow(string tableName,int index);

        public void AddData(string tableName,string columnName,MochaData data);
        public void UpdateData(string tableName,string columnName,int index,object data);
        public MochaData GetData(string tableName,string columnName,int index);

        #endregion

        #region Properties

        public MochaProvider Provider { get; }
        public MochaQuery Query { get; }
        public MochaConnectionState State { get; }
        public string Name { get; }

        #endregion
    }

    /// <summary>
    /// Sector interface for MochaDB sectors.
    /// </summary>
    public interface IMochaSector {
        #region Properties

        public string Name { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }

        #endregion
    }

    /// <summary>
    /// Data interface for MochaDB datas.
    /// </summary>
    public interface IMochaData {
        #region Properties

        public object Data { get; set; }
        public MochaDataType DataType { get; set; }

        #endregion
    }

    /// <summary>
    /// Column interface for MochaDB columns.
    /// </summary>
    public interface IMochaColumn {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public MochaColumnDataCollection Datas { get; }
        public MochaDataType DataType { get; set; }

        #endregion
    }

    /// <summary>
    /// Row interface for MochaDB rows.
    /// </summary>
    public interface IMochaRow {
        #region Properties

        public MochaDataCollection Datas { get; }

        #endregion
    }

    /// <summary>
    /// Table interface for MochaDB tables.
    /// </summary>
    public interface IMochaTable {
        #region Methods

        public void ShortDatas(int index);
        public void ShortColumns();

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public MochaColumnCollection Columns { get; }
        public MochaRowCollection Rows { get; }

        #endregion
    }

    /// <summary>
    /// Stack interface for MochaDB stacks.
    /// </summary>
    public interface IMochaStack {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public MochaStackItemCollection Items { get; }

        #endregion
    }

    /// <summary>
    /// StackItem interface for MochaDB stack items.
    /// </summary>
    public interface IMochaStackItem {
        #region Events

        public event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public MochaStackItemCollection Items { get; }

        #endregion
    }

    /// <summary>
    /// Provider interface for MochaDB providers.
    /// </summary>
    public interface IMochaProvider {
        #region Methods

        public void EnableReadonly();
        public MochaProviderAttribute GetAttribute(string name);

        #endregion

        #region Properties

        public string ConnectionString { get; set; }
        public string Path { get; }
        public string Password { get; }
        public bool Readonly { get; }

        #endregion
    }

    /// <summary>
    /// Provider attribute interface for MochaDB provider attributes.
    /// </summary>
    public interface IMochaProviderAttribute {
        #region Properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }

    /// <summary>
    /// Collection interface for MochaDB.
    /// </summary>
    /// <typeparam name="T">Type of collector.</typeparam>
    public interface IMochaCollection<T> {
        #region Events

        public event EventHandler<EventArgs> Changed;

        #endregion

        #region Methods

        public void Clear();
        public void Add(T item);
        public void AddRange(IEnumerable<T> items);
        public void Remove(T item);
        public void RemoveAt(int index);
        public int IndexOf(T item);
        public bool Contains(T item);
        public int MaxIndex();

        #endregion

        #region Properties

        public int Count { get; }
        public T this[int index] { get; }

        #endregion
    }
}
