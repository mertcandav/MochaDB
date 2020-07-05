using System;
using System.Linq;
using MochaDB.Mhql;
using MochaDB.Streams;

namespace MochaDB.Querying {
    /// <summary>
    /// Query extension for MochaDatabases.
    /// </summary>
    public static class QueryingMochaDatabase {
        /// <summary>
        /// Execute <see cref="MochaDbCommand.ExecuteReader()"/> function.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="mhql">MHQL Command.</param>
        public static MochaReader<object> ExecuteReader(this MochaDatabase db,string mhql) =>
             new MochaDbCommand(mhql,db).ExecuteReader();

        /// <summary>
        /// Execute <see cref="MochaDbCommand.ExecuteScalar()"/> function.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="mhql">MHQL Command.</param>
        public static object ExecuteScalar(this MochaDatabase db,string mhql) =>
            new MochaDbCommand(mhql,db).ExecuteScalar();

        /// <summary>
        /// Execute <see cref="MochaDbCommand.ExecuteScalarTable()"/> function.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="mhql">MHQL Command.</param>
        public static MochaTableResult ExecuteScalarTable(this MochaDatabase db,string mhql) =>
            new MochaDbCommand(mhql,db).ExecuteScalarTable();

        /// <summary>
        /// Returns all tables in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaTable> GetTables(this MochaDatabase db,Func<MochaTable,bool> query) =>
            new MochaCollectionResult<MochaTable>(db.GetTables().Where(query));

        /// <summary>
        /// Read all tables in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        public static MochaReader<MochaTable> ReadTables(this MochaDatabase db) =>
            new MochaReader<MochaTable>(db.GetTables());

        /// <summary>
        /// Read all tables in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaTable> ReadTables(this MochaDatabase db,Func<MochaTable,bool> query) =>
            new MochaReader<MochaTable>(db.GetTables(query));

        /// <summary>
        /// Returns all columns in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaColumn> GetColumns(this MochaDatabase db,string tableName,Func<MochaColumn,bool> query) =>
            new MochaCollectionResult<MochaColumn>(db.GetColumns(tableName).Where(query));

        /// <summary>
        /// Read all columns in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        public static MochaReader<MochaColumn> ReadColumns(this MochaDatabase db,string tableName) =>
            new MochaReader<MochaColumn>(db.GetColumns(tableName));

        /// <summary>
        /// Read all columns in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaColumn> ReadColumns(this MochaDatabase db,string tableName,Func<MochaColumn,bool> query) =>
            new MochaReader<MochaColumn>(db.GetColumns(tableName,query));

        /// <summary>
        /// Returns all rows in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaRow> GetRows(this MochaDatabase db,string tableName,Func<MochaRow,bool> query) =>
            new MochaCollectionResult<MochaRow>(db.GetRows(tableName).Where(query));

        /// <summary>
        /// Read all rows in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        public static MochaReader<MochaRow> ReadRows(this MochaDatabase db,string tableName) =>
            new MochaReader<MochaRow>(db.GetRows(tableName));

        /// <summary>
        /// Read all rows in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaRow> ReadRows(this MochaDatabase db,string tableName,Func<MochaRow,bool> query) =>
            new MochaReader<MochaRow>(db.GetRows(tableName,query));

        /// <summary>
        /// Returns all datas in column in table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaData> GetDatas(this MochaDatabase db,string tableName,string columnName,Func<MochaData,bool> query) =>
            new MochaCollectionResult<MochaData>(db.GetDatas(tableName,columnName).Where(query));

        /// <summary>
        /// Read all datas in column int table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public static MochaReader<MochaData> ReadDatas(this MochaDatabase db,string tableName,string columnName) =>
            new MochaReader<MochaData>(db.GetDatas(tableName,columnName));

        /// <summary>
        /// Read all datas in column int table by name.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaData> ReadDatas(this MochaDatabase db,string tableName,string columnName,Func<MochaData,bool> query) =>
            new MochaReader<MochaData>(db.GetDatas(tableName,columnName,query));
    }
}
