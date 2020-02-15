using MochaDB.Connection;
using System;

namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB database managers.
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
        public MochaConnectionState State { get; }
        public string Name { get; }

        #endregion
    }
}
