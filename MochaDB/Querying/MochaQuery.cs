using MochaDB.Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MochaDB.Querying {
    /// <summary>
    /// It offers query usage and management with MochaQ.
    /// </summary>
    [Serializable]
    public class MochaQuery:IMochaQuery {
        #region Fields

        private MochaDatabase db;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaQuery.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        /// <param name="embedded">Is embedded query.</param>
        internal MochaQuery(MochaDatabase db,bool embedded) {
            this.db = db;
            MochaQ = "RETURNQUERY";
            IsDatabaseEmbedded=embedded;
        }

        /// <summary>
        /// Create new MochaQuery.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        public MochaQuery(MochaDatabase db) {
            Database = db;
            MochaQ = "RETURNQUERY";
        }

        /// <summary>
        /// Create new MochaQuery.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MOchaDB database.</param>
        /// <param name="mochaQ">MochaQuery to use.</param>
        public MochaQuery(MochaDatabase db,string mochaQ)
            : this(db) {
            MochaQ = mochaQ;
        }

        #endregion

        #region Dynamic

        /// <summary>
        /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
        /// </summary>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public IMochaResult Dynamic(string mochaQ) {
            MochaQ = mochaQ;
            return Dynamic();
        }

        /// <summary>
        /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public IMochaResult Dynamic(MochaDatabase db,string mochaQ) {
            Database = db;
            MochaQ = mochaQ;
            return Dynamic();
        }

        /// <summary>
        /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
        /// </summary>
        public IMochaResult Dynamic() {
            if(Database.State!=MochaConnectionState.Connected)
                throw new Exception("Connection is not open!");

            //Check BREAKQUERY.
            if(MochaQ.Contains("BREAKQUERY"))
                return null;

            try {
                string[] parts = MochaQ.Split(' ');
                parts[0]=parts[0].Trim().ToUpperInvariant();
                parts[2]=parts[2].Trim().ToUpperInvariant();

                if(parts[0] == "SELECT") {

                    string[] selectedColumns = parts[1].Split(',');

                    if(parts[2] == "FROM") {
                        string tableName = parts[3];

                        MochaTable table = new MochaTable(tableName);

                        for(int index = 0; index < selectedColumns.Length; index++) {
                            table.Columns.Add(Database.GetColumn(tableName,selectedColumns[index]));
                        }

                        return new MochaResult<MochaTable>(table);

                    } else
                        throw new Exception("Table not specified!");
                } else
                    throw new Exception("The first syntax is wrong, there is no such function.");

            } catch { return null; }
        }

        #endregion

        #region Run

        /// <summary>
        /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
        /// </summary>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public void Run(string mochaQ) {
            MochaQ = mochaQ;
            Run();
        }

        /// <summary>
        /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public void Run(MochaDatabase db,string mochaQ) {
            Database = db;
            MochaQ = mochaQ;
            Run();
        }

        /// <summary>
        /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
        /// </summary>
        public void Run() {
            if(Database.State!=MochaConnectionState.Connected)
                throw new Exception("Connection is not open!");

            //Check null.
            if(string.IsNullOrEmpty(MochaQ))
                throw new Exception("This MochaQ query is empty, invalid!");

            //Check BREAKQUERY.
            if(MochaQ.Contains("BREAKQUERY"))
                return;

            string[] QueryPaths = MochaQ.Split(':');
            QueryPaths[0]=QueryPaths[0].Trim().ToUpperInvariant();

            if(QueryPaths.Length == 1) {
                if(QueryPaths[0] == "RESETMOCHA") {
                    Database.Reset();
                    return;
                } else if(QueryPaths[0] == "RESETTABLES") {
                    IEnumerable<XElement> tableRange = Database.Doc.Root.Element("Tables").Elements();
                    for(int index = 0; index < tableRange.Count(); index++) {
                        tableRange.ElementAt(index).Elements().Remove();
                    }

                    Database.Save();
                    return;
                } else if(QueryPaths[0] == "CLEARSECTORS") {
                    Database.ClearSectors();
                    return;
                } else if(QueryPaths[0] == "CLEARSTACKS") {
                    Database.ClearStacks();
                    return;
                } else if(QueryPaths[0] == "CLEARTABLES") {
                    Database.ClearTables();
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 2) {
                if(QueryPaths[0] == "REMOVETABLE") {
                    Database.RemoveTable(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "CREATETABLE") {
                    Database.CreateTable(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "CREATESTACK") {
                    Database.AddStack(new MochaStack(QueryPaths[1]));
                    return;
                } else if(QueryPaths[0] == "REMOVESECTOR") {
                    Database.RemoveSector(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "REMOVESTACK") {
                    Database.RemoveStack(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "SETPASSWORD") {
                    Database.SetPassword(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "SETDESCRIPTION") {
                    Database.SetDescription(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "RESETTABLE") {
                    if(!Database.ExistsTable(QueryPaths[1]))
                        throw new Exception("Table not found in this name!");

                    Database.Doc.Root.Element("Tables").Elements(QueryPaths[1]).Elements().Remove();
                    Database.Save();
                    return;
                } else if(QueryPaths[0] == "CREATEMOCHA") {
                    MochaDatabase.CreateMochaDB(Path.Combine(QueryPaths[1]) + ".bjay",string.Empty,string.Empty);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 3) {
                if(QueryPaths[0] == "REMOVECOLUMN") {
                    Database.RemoveColumn(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETSECTORDATA") {
                    Database.SetSectorData(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETSECTORDESCRIPTION") {
                    Database.SetSectorDescription(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETSTACKDESCRIPTION") {
                    Database.SetStackDescription(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETTABLEDESCRIPTION") {
                    Database.SetTableDescription(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "REMOVEROW") {
                    Database.RemoveRow(QueryPaths[1],int.Parse(QueryPaths[2]));
                    return;
                } else if(QueryPaths[0] == "RENAMETABLE") {
                    Database.RenameTable(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "REMOVESTACKITEM") {
                    Database.RemoveStackItem(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "RENAMESECTOR") {
                    Database.RenameSector(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "RENAMESTACK") {
                    Database.RenameStack(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "CREATECOLUMN") {
                    Database.CreateColumn(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "CREATEMOCHA") {
                    MochaDatabase.CreateMochaDB(Path.Combine(QueryPaths[1] + QueryPaths[2]) + ".bjay",string.Empty,string.Empty);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 4) {
                if(QueryPaths[0] == "RENAMECOLUMN") {
                    Database.RenameColumn(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETCOLUMNDESCRIPTION") {
                    Database.SetColumnDescription(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETCOLUMNDATATYPE") {
                    Database.SetColumnDataType(QueryPaths[1],QueryPaths[2],MochaData.GetDataTypeFromName(QueryPaths[3]));
                    return;
                } else if(QueryPaths[0] == "ADDDATA") {
                    Database.AddData(QueryPaths[1],QueryPaths[2],MochaData.GetDataFromString(Database.GetColumnDataType(QueryPaths[1],QueryPaths[2])
                        ,QueryPaths[3]));
                    return;
                } else if(QueryPaths[0] == "CREATESTACKITEM") {
                    Database.AddStackItem(QueryPaths[1],QueryPaths[3],new MochaStackItem(QueryPaths[2]));
                    return;
                } else if(QueryPaths[0] == "RENAMESTACKITEM") {
                    Database.RenameStackItem(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETSTACKITEMVALUE") {
                    Database.SetStackItemValue(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETSTACKITEMDESCRIPTION") {
                    Database.SetStackItemDescription(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "UPDATEFIRSTDATA") {
                    Database.UpdateData(QueryPaths[1],QueryPaths[2],0,QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "UPDATELASTDATA") {
                    Database.UpdateData(QueryPaths[1],QueryPaths[2],Database.GetDataCount(QueryPaths[1],QueryPaths[2]) - 1,QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "ADDSECTOR") {
                    MochaSector Sector = new MochaSector(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    Database.AddSector(Sector);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 5) {
                if(QueryPaths[0] == "UPDATEDATA") {
                    Database.UpdateData(QueryPaths[1],QueryPaths[2],int.Parse(QueryPaths[3]),QueryPaths[4]);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            }
        }

        #endregion

        #region GetRun

        /// <summary>
        /// Runs the active MochaQ query. Returns the incoming value.
        /// </summary>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public IMochaResult GetRun(string mochaQ) {
            MochaQ = mochaQ;
            return GetRun();
        }

        /// <summary>
        /// Runs the active MochaQ query. Returns the incoming value.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public IMochaResult GetRun(MochaDatabase db,string mochaQ) {
            Database = db;
            MochaQ = mochaQ;
            return GetRun();
        }

        /// <summary>
        /// Runs the active MochaQ query. Returns the incoming value.
        /// </summary>
        public IMochaResult GetRun() {
            if(Database.State!=MochaConnectionState.Connected)
                throw new Exception("Connection is not open!");

            //Check null.
            if(string.IsNullOrEmpty(MochaQ))
                throw new Exception("This MochaQ query is empty, invalid!");

            //Check BREAKQUERY.
            if(MochaQ.Contains("BREAKQUERY"))
                return null;

            string[] QueryPaths = MochaQ.Split(':');
            QueryPaths[0]=QueryPaths[0].Trim().ToUpperInvariant();

            if(QueryPaths.Length == 1) {
                if(QueryPaths[0] == "GETTABLES") {
                    return Database.GetTables();
                } else if(QueryPaths[0] == "GETPASSWORD") {
                    return Database.GetPassword();
                } else if(QueryPaths[0] == "GETDESCRIPTION") {
                    return Database.GetDescription();
                } else if(QueryPaths[0] == "GETDATAS") {
                    List<MochaData> datas = new List<MochaData>();
                    IEnumerable<XElement> tableRange = Database.Doc.Root.Element("Tables").Elements();
                    for(int index = 0; index < tableRange.Count(); index++) {
                        datas.AddRange(GETDATAS(tableRange.ElementAt(index).Name.LocalName).collection);
                    }
                    return new MochaCollectionResult<MochaData>(datas);
                } else if(QueryPaths[0] == "TABLECOUNT") {
                    return new MochaResult<int>(Database.Doc.Root.Elements().Count());
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 2) {
                if(QueryPaths[0] == "GETTABLE") {
                    return Database.GetTable(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETCOLUMNS") {
                    return Database.GetColumns(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSECTOR") {
                    return Database.GetSector(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETFIRSTCOLUMN_NAME") {
                    return new MochaResult<string>(GETFIRSTCOLUMN_NAME(QueryPaths[1]));
                } else if(QueryPaths[0] == "GETROWS") {
                    return Database.GetRows(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETDATAS") {
                    return GETDATAS(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSECTORDATA") {
                    return Database.GetSectorData(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSECTORDESCRIPTION") {
                    return Database.GetSectorDescription(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETTABLEDESCRIPTION") {
                    return Database.GetTableDescription(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSTACKDESCRIPTION") {
                    return Database.GetStackDescription(QueryPaths[1]);
                } else if(QueryPaths[0] == "COLUMNCOUNT") {
                    return COLUMNCOUNT(QueryPaths[1]);
                } else if(QueryPaths[0] == "ROWCOUNT") {
                    try {
                        return new MochaResult<int>(Database.Doc.Root.Element("Tables").Elements(QueryPaths[1]).Elements(
                            GETFIRSTCOLUMN_NAME(QueryPaths[1]).Value).Elements().Count());
                    } catch(Exception excep) {
                        throw excep;
                    }
                } else if(QueryPaths[0] == "DATACOUNT") {
                    return new MochaResult<int>(Database.GetDataCount(QueryPaths[1],GETFIRSTCOLUMN_NAME(QueryPaths[1]))
                        * COLUMNCOUNT(QueryPaths[1]));
                } else if(QueryPaths[0] == "EXISTSTABLE") {
                    return Database.ExistsTable(QueryPaths[1]);
                } else if(QueryPaths[0] == "EXISTSSECTOR") {
                    return Database.ExistsSector(QueryPaths[1]);
                } else if(QueryPaths[0] == "EXISTSSTACK") {
                    return Database.ExistsStack(QueryPaths[1]);
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 3) {
                if(QueryPaths[0] == "GETCOLUMN") {
                    return Database.GetColumn(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "DATACOUNT") {
                    return Database.GetDataCount(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "EXISTSCOLUMN") {
                    return Database.ExistsColumn(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "EXISTSSTACKITEM") {
                    return Database.ExistsStackItem(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETSTACKITEMVALUE") {
                    return Database.GetStackItemValue(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETSTACKITEMDESCRIPTION") {
                    return Database.GetStackItemDescription(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETDATAS") {
                    return Database.GetDatas(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETSTACKITEM") {
                    return Database.GetStackItem(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETCOLUMNDESCRIPTION") {
                    return Database.GetColumnDescription(QueryPaths[1],QueryPaths[2]);
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 4) {
                if(QueryPaths[0] == "EXISTSDATA") {
                    return Database.ExistsData(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else
                throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Return column count of table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        private MochaResult<int> COLUMNCOUNT(string name) {
            if(!Database.ExistsTable(name))
                throw new Exception("Table not found in this name!");

            IEnumerable<XElement> columnElements = Database.Doc.Root.Element("Tables").Elements(name).Elements();
            return columnElements.Count();
        }

        /// <summary>
        /// Return all datas of table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        private MochaCollectionResult<MochaData> GETDATAS(string name) {
            if(!Database.ExistsTable(name))
                throw new Exception("Table not found in this name!");

            List<MochaData> datas = new List<MochaData>();

            IEnumerable<XElement> columnRange = Database.Doc.Root.Element("Tables").Elements(name).Elements();
            for(int index = 0; index < columnRange.Count(); index++) {
                datas.AddRange(Database.GetDatas(name,columnRange.ElementAt(index).Name.LocalName).collection);
            }

            return new MochaCollectionResult<MochaData>(datas);
        }

        /// <summary>
        /// Return first column name of table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        private MochaResult<string> GETFIRSTCOLUMN_NAME(string name) {
            if(!Database.ExistsTable(name))
                throw new Exception("Table not found in this name!");

            XElement firstColumn = (XElement)Database.Doc.Root.Element("Tables").Element(name).FirstNode;
            return firstColumn == null ? null : firstColumn.Name.LocalName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is embedded query.
        /// </summary>
        public bool IsDatabaseEmbedded { get; private set; }

        /// <summary>
        /// MochaDatabse object that provides management of the targeted MochaDB database.
        /// </summary>
        public MochaDatabase Database {
            get =>
                db;
            set {
                if(IsDatabaseEmbedded)
                    throw new Exception("This is embedded in database, can not set database!");

                if(db == null)
                    throw new Exception("This MochaDatabase is not affiliated with a MochaDB!");

                if(value == db)
                    return;


                db = value;
            }
        }

        /// <summary>
        /// Active MochaQ query.
        /// </summary>
        public string MochaQ { get; set; }

        #endregion
    }
}
