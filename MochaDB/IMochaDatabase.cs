using System;
using MochaDB.Connection;
using MochaDB.Querying;

namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB database managers.
    /// </summary>
    public interface IMochaDatabase:IDisposable {
        #region Events

        event EventHandler<EventArgs> ChangeContent;

        #endregion

        #region Methods

        void Connect();
        void Disconnect();

        void AddSector(MochaSector sector);
        void RemoveSector(string name);
        MochaResult<MochaSector> GetSector(string name);
        MochaResult<bool> ExistsSector(string name);

        void AddStack(MochaStack stack);
        void RemoveStack(string name);
        MochaResult<MochaStack> GetStack(string name);
        MochaResult<bool> ExistsStack(string name);

        void AddStackItem(string name,string path,MochaStackItem item);
        void RemoveStackItem(string name,string path);
        MochaResult<MochaStackItem> GetStackItem(string name,string path);
        MochaResult<bool> ExistsStackItem(string name,string path);

        void AddTable(MochaTable table);
        void RemoveTable(string name);
        MochaResult<MochaTable> GetTable(string name);
        MochaResult<bool> ExistsTable(string name);

        void AddColumn(string tableName,MochaColumn column);
        void RemoveColumn(string tableName,string name);
        MochaResult<MochaColumn> GetColumn(string tableName,string name);
        MochaResult<bool> ExistsColumn(string tableName,string name);

        void AddRow(string tableName,MochaRow row);
        void RemoveRow(string tableName,int index);
        MochaResult<MochaRow> GetRow(string tableName,int index);

        void AddData(string tableName,string columnName,MochaData data);
        void UpdateData(string tableName,string columnName,int index,object data);
        MochaResult<MochaData> GetData(string tableName,string columnName,int index);

        #endregion

        #region Properties

        MochaProvider Provider { get; }
        MochaConnectionState ConnectionState { get; }
        string Name { get; }

        #endregion
    }
}
