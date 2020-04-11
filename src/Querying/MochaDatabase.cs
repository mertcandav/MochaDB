using System;
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
        /// Execute <see cref="MochaDbCommand.ExecuteCommand()"/> function.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="mhql">MHQL Command.</param>
        public static void ExecuteCommand(this MochaDatabase db,string mhql) =>
            new MochaDbCommand(mhql,db).ExecuteCommand();

        /// <summary>
        /// Returns all sectors in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaSector> GetSectors(this MochaDatabase db,Func<MochaSector,bool> query) =>
            new MochaCollectionResult<MochaSector>(db.GetSectors().Where(query));

        /// <summary>
        /// Read all sectors in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        public static MochaReader<MochaSector> ReadSectors(this MochaDatabase db) =>
            new MochaReader<MochaSector>(db.GetSectors());

        /// <summary>
        /// Read all sectors in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaSector> ReadSectors(this MochaDatabase db,Func<MochaSector,bool> query) =>
            new MochaReader<MochaSector>(db.GetSectors(query));

        /// <summary>
        /// Returns all stacks in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaStack> GetStacks(this MochaDatabase db,Func<MochaStack,bool> query) =>
            new MochaCollectionResult<MochaStack>(db.GetStacks().Where(query));

        /// <summary>
        /// Read all stacks in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        public static MochaReader<MochaStack> ReadStacks(this MochaDatabase db) =>
            new MochaReader<MochaStack>(db.GetStacks());

        /// <summary>
        /// Read all stacks in database.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaStack> ReadStacks(this MochaDatabase db,Func<MochaStack,bool> query) =>
            new MochaReader<MochaStack>(db.GetStacks(query));

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

        /// <summary>
        /// Returns all attributes from sector.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of sector.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<IMochaAttribute> GetSectorAttributes(this MochaDatabase db,
            string name,Func<IMochaAttribute,bool> query) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetSectorAttributes(name).Where(query));

        /// <summary>
        /// Read all attributes from sector.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of sector.</param>
        public static MochaReader<IMochaAttribute> ReadSectorAttributes(this MochaDatabase db,string name) =>
            new MochaReader<IMochaAttribute>(db.GetSectorAttributes(name));

        /// <summary>
        /// Read all attributes from sector.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of sector.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<IMochaAttribute> ReadSectorAttributes(this MochaDatabase db,
            string name,Func<IMochaAttribute,bool> query) =>
            new MochaReader<IMochaAttribute>(db.GetSectorAttributes(name).Where(query));

        /// <summary>
        /// Returns all attributes from stack.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        public static MochaCollectionResult<IMochaAttribute> GetStackAttributes(this MochaDatabase db,string name) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetStackAttributes(name));

        /// <summary>
        /// Returns all attributes from stack.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<IMochaAttribute> GetStackAttributes(this MochaDatabase db,
            string name,Func<IMochaAttribute,bool> query) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetStackAttributes(name).Where(query));

        /// <summary>
        /// Read all attributes from stack.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        public static MochaReader<IMochaAttribute> ReadStackAttributes(this MochaDatabase db,string name) =>
            new MochaReader<IMochaAttribute>(db.GetStackAttributes(name));

        /// <summary>
        /// Read all attributes from stack.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<IMochaAttribute> ReadStackAttributes(this MochaDatabase db,
            string name,Func<IMochaAttribute,bool> query) =>
            new MochaReader<IMochaAttribute>(db.GetStackAttributes(name).Where(query));

        /// <summary>
        /// Returns all attributes from stackitem.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public static MochaCollectionResult<IMochaAttribute> GetStackItemAttributes(this MochaDatabase db,string name,string path) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetStackItemAttributes(name,path));

        /// <summary>
        /// Returns all attributes from stackitem.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="query">Query for filtering.</param>
        /// <param name="path">Path of stack item.</param>
        public static MochaCollectionResult<IMochaAttribute> GetStackItemAttributes(this MochaDatabase db,
            string name,string path,Func<IMochaAttribute,bool> query) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetStackItemAttributes(name,path).Where(query));

        /// <summary>
        /// Read all attributes from stackitem.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public static MochaReader<IMochaAttribute> ReadStackItemAttributes(this MochaDatabase db,string name,string path) =>
            new MochaReader<IMochaAttribute>(db.GetStackItemAttributes(name,path));

        /// <summary>
        /// Read all attributes from stackitem.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of stack.</param>
        /// <param name="query">Query for filtering.</param>
        /// <param name="path">Path of stack item.</param>
        public static MochaReader<IMochaAttribute> ReadStackItemAttributes(this MochaDatabase db,
            string name,string path,Func<IMochaAttribute,bool> query) =>
            new MochaReader<IMochaAttribute>(db.GetStackItemAttributes(name,path).Where(query));

        /// <summary>
        /// Returns all attributes from table.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of table.</param>
        public static MochaCollectionResult<IMochaAttribute> GetTableAttributes(this MochaDatabase db,string name) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetTableAttributes(name));

        /// <summary>
        /// Returns all attributes from table.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of table.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<IMochaAttribute> GetTableAttributes(this MochaDatabase db,
            string name,Func<IMochaAttribute,bool> query) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetTableAttributes(name).Where(query));

        /// <summary>
        /// Read all attributes from table.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of table.</param>
        public static MochaReader<IMochaAttribute> ReadTableAttributes(this MochaDatabase db,string name) =>
            new MochaReader<IMochaAttribute>(db.GetTableAttributes(name));

        /// <summary>
        /// Read all attributes from table.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="name">Name of table.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<IMochaAttribute> ReadTableAttributes(this MochaDatabase db,
            string name,Func<IMochaAttribute,bool> query) =>
            new MochaReader<IMochaAttribute>(db.GetTableAttributes(name).Where(query));

        /// <summary>
        /// Returns all attributes from column.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of column.</param>
        /// <param name="name">Name of column.</param>
        public static MochaCollectionResult<IMochaAttribute> GetColumnAttributes(this MochaDatabase db,
            string tableName,string name) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetColumnAttributes(tableName,name));

        /// <summary>
        /// Returns all attributes from column.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of column.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<IMochaAttribute> GetColumnAttributes(this MochaDatabase db,
            string tableName,string name,Func<IMochaAttribute,bool> query) =>
            new MochaCollectionResult<IMochaAttribute>(db.GetColumnAttributes(tableName,name).Where(query));

        /// <summary>
        /// Read all attributes from column.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of column.</param>
        /// <param name="name">Name of column.</param>
        public static MochaReader<IMochaAttribute> ReadColumnAttributes(this MochaDatabase db,string tableName,string name) =>
            new MochaReader<IMochaAttribute>(db.GetColumnAttributes(tableName,name));

        /// <summary>
        /// Read all attributes from column.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="tableName">Name of column.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<IMochaAttribute> ReadColumnAttributes(this MochaDatabase db,
            string tableName,string name,Func<IMochaAttribute,bool> query) =>
            new MochaReader<IMochaAttribute>(db.GetColumnAttributes(tableName,name).Where(query));

        /// <summary>
        /// Returns sub elements of element in path.
        /// </summary>
        /// <param name="db">Target database.</param>
        /// <param name="path">Path of base element.</param>
        /// <param name="query">Path of base element.</param>
        public static MochaCollectionResult<MochaElement> GetElements(
            this MochaDatabase db,MochaPath path,Func<MochaElement,bool> query) =>
            new MochaCollectionResult<MochaElement>(db.GetElements(path).Where(query));
    }
}
