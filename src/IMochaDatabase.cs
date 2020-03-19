using System;
using MochaDB.Connection;

namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB database managers.
    /// </summary>
    public interface IMochaDatabase:IDisposable {
        #region Events

        event EventHandler<EventArgs> Changing;
        event EventHandler<EventArgs> Changed;

        #endregion

        #region Methods

        void Connect();
        void Disconnect();

        void AddSector(MochaSector sector);
        bool RemoveSector(string name);
        MochaSector GetSector(string name);
        bool ExistsSector(string name);

        void AddStack(MochaStack stack);
        bool RemoveStack(string name);
        MochaStack GetStack(string name);
        bool ExistsStack(string name);

        void AddStackItem(string name,string path,MochaStackItem item);
        bool RemoveStackItem(string name,string path);
        MochaStackItem GetStackItem(string name,string path);
        bool ExistsStackItem(string name,string path);

        void AddTable(MochaTable table);
        bool RemoveTable(string name);
        MochaTable GetTable(string name);
        bool ExistsTable(string name);

        void AddColumn(string tableName,MochaColumn column);
        bool RemoveColumn(string tableName,string name);
        MochaColumn GetColumn(string tableName,string name);
        bool ExistsColumn(string tableName,string name);

        void AddRow(string tableName,MochaRow row);
        bool RemoveRow(string tableName,int index);
        MochaRow GetRow(string tableName,int index);

        void AddData(string tableName,string columnName,MochaData data);
        void UpdateData(string tableName,string columnName,int index,object data);
        MochaData GetData(string tableName,string columnName,int index);

        void ClearLogs();
        void RestoreToLog(string id);

        #endregion

        #region Properties

        MochaProvider Provider { get; }
        MochaConnectionState ConnectionState { get; }
        string Name { get; }

        #endregion
    }
}
