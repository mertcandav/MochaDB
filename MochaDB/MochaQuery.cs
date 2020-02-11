using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MochaDB {
    /// <summary>
    /// It offers query usage and management with MochaQ.
    /// </summary>
    [Serializable]
    public sealed class MochaQuery {
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
            IsDatabaseEmbedded=embedded;
            DB = db;
            MochaQ = "RETURNQUERY";
        }

        /// <summary>
        /// Create new MochaQuery.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        public MochaQuery(MochaDatabase db) {
            DB = db;
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
        public object Dynamic(string mochaQ) {
            MochaQ = mochaQ;
            return Dynamic();
        }

        /// <summary>
        /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public object Dynamic(MochaDatabase db,string mochaQ) {
            DB = db;
            MochaQ = mochaQ;
            return Dynamic();
        }

        /// <summary>
        /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
        /// </summary>
        public object Dynamic() {
            if(DB.State!=MochaConnectionState.Connected)
                throw new Exception("Connection is not open!");

            //Check BREAKQUERY.
            if(MochaQ.Contains("BREAKQUERY"))
                return null;

            object ReturnObject = null;

            try {
                string[] Parts = MochaQ.Split(' ');
                Parts[0]=Parts[0].Trim().ToUpperInvariant();
                Parts[2]=Parts[2].Trim().ToUpperInvariant();

                if(Parts[0] == "SELECT") {

                    string[] SelectedColumns = Parts[1].Split(',');

                    if(Parts[2] == "FROM") {
                        string TableName = Parts[3];

                        MochaTable Table = new MochaTable(TableName);

                        for(int Index = 0; Index < SelectedColumns.Length; Index++) {
                            Table.AddColumn(DB.GetColumn(TableName,SelectedColumns[Index]));
                        }

                        return Table;

                    } else
                        throw new Exception("Table not specified!");
                } else
                    throw new Exception("The first syntax is wrong, there is no such function.");

            } catch { return ReturnObject; }
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
            DB = db;
            MochaQ = mochaQ;
            Run();
        }

        /// <summary>
        /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
        /// </summary>
        public void Run() {
            if(DB.State!=MochaConnectionState.Connected)
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
                    DB.Reset();
                    return;
                } else if(QueryPaths[0] == "RESETTABLES") {
                    IEnumerable<XElement> tableRange = DB.Doc.Root.Element("Tables").Elements();
                    for(int index = 0; index < tableRange.Count(); index++) {
                        tableRange.ElementAt(index).Elements().Remove();
                    }

                    DB.Save();
                    return;
                } else if(QueryPaths[0] == "CLEARSECTORS") {
                    DB.ClearSectors();
                    return;
                } else if(QueryPaths[0] == "CLEARSTACKS") {
                    DB.ClearStacks();
                    return;
                } else if(QueryPaths[0] == "CLEARTABLES") {
                    DB.ClearTables();
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 2) {
                if(QueryPaths[0] == "REMOVETABLE") {
                    DB.RemoveTable(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "CREATETABLE") {
                    DB.CreateTable(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "CREATESTACK") {
                    DB.AddStack(new MochaStack(QueryPaths[1]));
                    return;
                } else if(QueryPaths[0] == "REMOVESECTOR") {
                    DB.RemoveSector(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "REMOVESTACK") {
                    DB.RemoveStack(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "SETPASSWORD") {
                    DB.SetPassword(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "SETDESCRIPTION") {
                    DB.SetDescription(QueryPaths[1]);
                    return;
                } else if(QueryPaths[0] == "RESETTABLE") {
                    if(!DB.ExistsTable(QueryPaths[1]))
                        throw new Exception("Table not found in this name!");

                    DB.Doc.Root.Element("Tables").Elements(QueryPaths[1]).Elements().Remove();
                    DB.Save();
                    return;
                } else if(QueryPaths[0] == "CREATEMOCHA") {
                    MochaDatabase.CreateMochaDB(Path.Combine(QueryPaths[1]) + ".bjay",string.Empty,string.Empty);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 3) {
                if(QueryPaths[0] == "REMOVECOLUMN") {
                    DB.RemoveColumn(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETSECTORDATA") {
                    DB.SetSectorData(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETSECTORDESCRIPTION") {
                    DB.SetSectorDescription(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "SETTABLEDESCRIPTION") {
                    DB.SetTableDescription(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "REMOVEROW") {
                    DB.RemoveRow(QueryPaths[1],int.Parse(QueryPaths[2]));
                    return;
                } else if(QueryPaths[0] == "RENAMETABLE") {
                    DB.RenameTable(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "REMOVESTACKITEM") {
                    DB.RemoveStackItem(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "RENAMESECTOR") {
                    DB.RenameSector(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "RENAMESTACK") {
                    DB.RenameStack(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "CREATECOLUMN") {
                    DB.CreateColumn(QueryPaths[1],QueryPaths[2]);
                    return;
                } else if(QueryPaths[0] == "CREATEMOCHA") {
                    MochaDatabase.CreateMochaDB(Path.Combine(QueryPaths[1] + QueryPaths[2]) + ".bjay",string.Empty,string.Empty);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 4) {
                if(QueryPaths[0] == "RENAMECOLUMN") {
                    DB.RenameColumn(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETCOLUMNDESCRIPTION") {
                    DB.SetColumnDescription(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "CREATESTACKITEM") {
                    DB.AddStackItem(QueryPaths[1],QueryPaths[3],new MochaStackItem(QueryPaths[2]));
                    return;
                } else if(QueryPaths[0] == "RENAMESTACKITEM") {
                    DB.RenameStackItem(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETVALUESTACKITEM") {
                    DB.SetValueStackItem(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "SETDESCRIPTIONSTACKITEM") {
                    DB.SetDescriptionStackItem(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "UPDATEFIRSTDATA") {
                    DB.UpdateData(QueryPaths[1],QueryPaths[2],0,QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "UPDATELASTDATA") {
                    DB.UpdateData(QueryPaths[1],QueryPaths[2],DB.GetDataCount(QueryPaths[1],QueryPaths[2]) - 1,QueryPaths[3]);
                    return;
                } else if(QueryPaths[0] == "ADDSECTOR") {
                    MochaSector Sector = new MochaSector(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
                    DB.AddSector(Sector);
                    return;
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 5) {
                if(QueryPaths[0] == "UPDATEDATA") {
                    DB.UpdateData(QueryPaths[1],QueryPaths[2],int.Parse(QueryPaths[3]),QueryPaths[4]);
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
        public object GetRun(string mochaQ) {
            MochaQ = mochaQ;
            return GetRun();
        }

        /// <summary>
        /// Runs the active MochaQ query. Returns the incoming value.
        /// </summary>
        /// <param name="db">MochaDatabase object that provides management of the targeted MochaDB database.</param>
        /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
        public object GetRun(MochaDatabase db,string mochaQ) {
            DB = db;
            MochaQ = mochaQ;
            return GetRun();
        }

        /// <summary>
        /// Runs the active MochaQ query. Returns the incoming value.
        /// </summary>
        public object GetRun() {
            if(DB.State!=MochaConnectionState.Connected)
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
                    return DB.GetTables();
                } else if(QueryPaths[0] == "GETPASSWORD") {
                    return DB.GetPassword();
                } else if(QueryPaths[0] == "GETDESCRIPTION") {
                    return DB.GetDescription();
                } else if(QueryPaths[0] == "GETDATAS") {
                    List<MochaData> datas = new List<MochaData>();
                    IEnumerable<XElement> tableRange = DB.Doc.Root.Element("Tables").Elements();
                    for(int index = 0; index < tableRange.Count(); index++) {
                        datas.AddRange(GETDATAS(tableRange.ElementAt(index).Name.LocalName));
                    }
                    return datas;
                } else if(QueryPaths[0] == "TABLECOUNT") {
                    return DB.Doc.Root.Elements().Count();
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 2) {
                if(string.IsNullOrEmpty(QueryPaths[1]))
                    throw new Exception("Parameter not found!");

                if(QueryPaths[0] == "GETTABLE") {
                    return DB.GetTable(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETCOLUMNS") {
                    return DB.GetColumns(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSECTOR") {
                    return DB.GetSector(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETFIRSTCOLUMN_NAME") {
                    return GETFIRSTCOLUMN_NAME(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETROWS") {
                    return DB.GetRows(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETDATAS") {
                    return GETDATAS(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSECTORDATA") {
                    return DB.GetSectorData(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETSECTORDESCRIPTION") {
                    return DB.GetSectorDescription(QueryPaths[1]);
                } else if(QueryPaths[0] == "GETTABLEDESCRIPTION") {
                    return DB.GetTableDescription(QueryPaths[1]);
                } else if(QueryPaths[0] == "COLUMNCOUNT") {
                    return COLUMNCOUNT(QueryPaths[1]);
                } else if(QueryPaths[0] == "ROWCOUNT") {
                    try {
                        return DB.Doc.Root.Element("Tables").Elements(QueryPaths[1]).Elements(
                            GETFIRSTCOLUMN_NAME(QueryPaths[1])).Elements().Count();
                    } catch(Exception excep) {
                        throw excep;
                    }
                } else if(QueryPaths[0] == "DATACOUNT") {
                    return DB.GetDataCount(QueryPaths[1],GETFIRSTCOLUMN_NAME(QueryPaths[1]))
                        * COLUMNCOUNT(QueryPaths[1]);
                } else if(QueryPaths[0] == "EXISTSTABLE") {
                    return DB.ExistsTable(QueryPaths[1]);
                } else if(QueryPaths[0] == "EXISTSSECTOR") {
                    return DB.ExistsSector(QueryPaths[1]);
                } else if(QueryPaths[0] == "EXISTSSTACK") {
                    return DB.ExistsStack(QueryPaths[1]);
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 3) {
                if(string.IsNullOrEmpty(QueryPaths[1]))
                    throw new Exception("Parameter not found!");
                else if(string.IsNullOrEmpty(QueryPaths[2]))
                    throw new Exception("Parameter not found!");

                if(QueryPaths[0] == "GETCOLUMN") {
                    return DB.GetColumn(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "DATACOUNT") {
                    return DB.GetDataCount(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "EXISTSCOLUMN") {
                    return DB.ExistsColumn(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "EXISTSSTACKITEM") {
                    return DB.ExistsStackItem(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETVALUESTACKITEM") {
                    return DB.GetValueStackItem(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETDESCRIPTIONSTACKITEM") {
                    return DB.GetDescriptionStackItem(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETDATAS") {
                    return DB.GetDatas(QueryPaths[1],QueryPaths[2]);
                } else if(QueryPaths[0] == "GETCOLUMNDESCRIPTION") {
                    return DB.GetColumnDescription(QueryPaths[1],QueryPaths[2]);
                } else
                    throw new Exception("Invalid query. The content of the query could not be processed, wrong!");
            } else if(QueryPaths.Length == 4) {
                if(string.IsNullOrEmpty(QueryPaths[1]))
                    throw new Exception("Parameter not found!");
                else if(string.IsNullOrEmpty(QueryPaths[2]))
                    throw new Exception("Parameter not found!");
                else if(string.IsNullOrEmpty(QueryPaths[3]))
                    throw new Exception("Parameter not found!");

                if(QueryPaths[0] == "EXISTSDATA") {
                    return DB.ExistsData(QueryPaths[1],QueryPaths[2],QueryPaths[3]);
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
        private int COLUMNCOUNT(string name) {
            if(!DB.ExistsTable(name))
                throw new Exception("Table not found in this name!");

            IEnumerable<XElement> columnElements = DB.Doc.Root.Element("Tables").Elements(name).Elements();
            return columnElements.Count();
        }

        /// <summary>
        /// Return all datas of table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        private IList<MochaData> GETDATAS(string name) {
            if(!DB.ExistsTable(name))
                throw new Exception("Table not found in this name!");

            List<MochaData> datas = new List<MochaData>();

            IEnumerable<XElement> columnRange = DB.Doc.Root.Element("Tables").Elements(name).Elements();
            for(int index = 0; index < columnRange.Count(); index++) {
                datas.AddRange(DB.GetDatas(name,columnRange.ElementAt(index).Name.LocalName));
            }

            return datas;
        }

        /// <summary>
        /// Return first column name of table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        private string GETFIRSTCOLUMN_NAME(string name) {
            if(!DB.ExistsTable(name))
                throw new Exception("Table not found in this name!");

            try {
                return ((XElement)DB.Doc.Root.Element("Tables").Element(name).FirstNode).Name.LocalName;
            } catch {
                return null;
            }
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
        public MochaDatabase DB {
            get =>
                db;
            set {
                if(IsDatabaseEmbedded)
                    throw new Exception("This is embedded in database, can Not set database!");

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
