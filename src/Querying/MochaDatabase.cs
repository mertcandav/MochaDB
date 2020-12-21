namespace MochaDB.Querying {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using MochaDB.Mhql;
  using MochaDB.Streams;

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
    /// Returns all tables in database.
    /// </summary>
    /// <param name="db">Target database.</param>
    /// <param name="query">Query for filtering.</param>
    public static IEnumerable<MochaTable> GetTables(this MochaDatabase db,Func<MochaTable,bool> query) =>
        db.GetTables().Where(query);

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
    public static IEnumerable<MochaColumn> GetColumns(this MochaDatabase db,string tableName,
      Func<MochaColumn,bool> query) =>
        db.GetColumns(tableName).Where(query);

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
    public static MochaReader<MochaColumn> ReadColumns(this MochaDatabase db,string tableName,
      Func<MochaColumn,bool> query) =>
        new MochaReader<MochaColumn>(db.GetColumns(tableName,query));

    /// <summary>
    /// Returns all rows in table by name.
    /// </summary>
    /// <param name="db">Target database.</param>
    /// <param name="tableName">Name of table.</param>
    /// <param name="query">Query for filtering.</param>
    public static IEnumerable<MochaRow> GetRows(this MochaDatabase db,string tableName,
      Func<MochaRow,bool> query) =>
        db.GetRows(tableName).Where(query);

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
    public static MochaReader<MochaRow> ReadRows(this MochaDatabase db,string tableName,
      Func<MochaRow,bool> query) =>
        new MochaReader<MochaRow>(db.GetRows(tableName,query));

    /// <summary>
    /// Returns all datas in column in table by name.
    /// </summary>
    /// <param name="db">Target database.</param>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="query">Query for filtering.</param>
    public static IEnumerable<MochaData> GetDatas(this MochaDatabase db,string tableName,
      string columnName,Func<MochaData,bool> query) =>
        db.GetDatas(tableName,columnName).Where(query);

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
    public static MochaReader<MochaData> ReadDatas(this MochaDatabase db,string tableName,
      string columnName,Func<MochaData,bool> query) =>
        new MochaReader<MochaData>(db.GetDatas(tableName,columnName,query));
  }
}
