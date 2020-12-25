namespace MochaDB.Mhq {
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Xml.Linq;

  /// <summary>
  /// It offers query usage and management with MochaQ.
  /// </summary>
  public class MochaQuery {
    #region Fields

    private MochaDatabase database;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new instance of <see cref="MochaQuery"/>.
    /// </summary>
    /// <param name="database">MochaDatabase object to use querying.</param>
    /// <param name="cmd">Command.</param>
    public MochaQuery(MochaDatabase database,string cmd = "") {
      IsDatabaseEmbedded = false;
      Database = database;
      Command = cmd;
    }

    #endregion Constructors

    #region Members

    #region Run

    /// <summary>
    /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public virtual void Run(string mochaQ) {
      Command=mochaQ;
      Run();
    }

    /// <summary>
    /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
    /// </summary>
    /// <param name="database">MochaDatabase object that provides management of the targeted MochaDB database.</param>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public virtual void Run(MochaDatabase database,string mochaQ) {
      Database = database;
      Command=mochaQ;
      Run();
    }

    /// <summary>
    /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
    /// </summary>
    public virtual void Run() {
      Database.OnConnectionCheckRequired(this,new EventArgs());

      //Check null.
      if(string.IsNullOrEmpty(Command))
        throw new MochaException("This MochaQ query is empty, invalid!");

      string[] queryPaths = Command.Split(':');
      queryPaths[0]=queryPaths[0].ToUpperInvariant();

      switch(queryPaths.Length) {
        case 1:
          switch(queryPaths[0]) {
            case "RESETMOCHA":
              Database.Reset();
              return;
            case "RESETTABLES":
              Database.OnChanging(this,new EventArgs());
              foreach(XElement element in Database.CDoc.Root.Element("Tables").Elements())
                element.Elements().Remove();
              Database.Save();
              return;
            case "CLEARTABLES":
              Database.ClearTables();
              return;
            case "CLEARALL":
              Database.ClearAll();
              return;
            case "CLEARLOGS":
              Database.ClearLogs();
              return;
            case "RESTORETOFIRSTLOG":
              Database.RestoreToFirstLog();
              return;
            case "RESTORETOLASTLOG":
              Database.RestoreToLastLog();
              return;
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 2:
          switch(queryPaths[0]) {
            case "REMOVETABLE":
              Database.RemoveTable(queryPaths[1]);
              return;
            case "CREATETABLE":
              Database.CreateTable(queryPaths[1]);
              return;
            case "SETPASSWORD":
              Database.SetPassword(queryPaths[1]);
              return;
            case "SETDESCRIPTION":
              Database.SetDescription(queryPaths[1]);
              return;
            case "RESTORETOLOG":
              Database.RestoreToLog(queryPaths[1]);
              return;
            case "CLEARROWS":
              Database.ClearRows(queryPaths[1]);
              return;
            case "RESETTABLE":
              if(!Database.ExistsTable(queryPaths[1]))
                throw new MochaException("Table not found in this name!");
              Database.OnChanging(this,new EventArgs());
              Database.CDoc.Root.Element("Tables").Elements(queryPaths[1]).Elements().Remove();
              Database.Save();
              return;
            case "CREATEMOCHA":
              MochaDatabase.CreateMochaDB(queryPaths[1],string.Empty,string.Empty);
              return;
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 3:
          switch(queryPaths[0]) {
            case "REMOVECOLUMN":
              Database.RemoveColumn(queryPaths[1],queryPaths[2]);
              return;
            case "SETTABLEDESCRIPTION":
              Database.SetTableDescription(queryPaths[1],queryPaths[2]);
              return;
            case "REMOVEROW":
              Database.RemoveRow(queryPaths[1],int.Parse(queryPaths[2]));
              return;
            case "RENAMETABLE":
              Database.RenameTable(queryPaths[1],queryPaths[2]);
              return;
            case "CREATECOLUMN":
              Database.CreateColumn(queryPaths[1],queryPaths[2]);
              return;
            case "CREATEMOCHA":
              MochaDatabase.CreateMochaDB(Path.Combine(queryPaths[1] + queryPaths[2]),string.Empty,string.Empty);
              return;
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 4:
          switch(queryPaths[0]) {
            case "RENAMECOLUMN":
              Database.RenameColumn(queryPaths[1],queryPaths[2],queryPaths[3]);
              return;
            case "SETCOLUMNDESCRIPTION":
              Database.SetColumnDescription(queryPaths[1],queryPaths[2],queryPaths[3]);
              return;
            case "SETCOLUMNDATATYPE":
              Database.SetColumnDataType(queryPaths[1],queryPaths[2],MochaData.GetDataTypeFromName(queryPaths[3]));
              return;
            case "ADDDATA":
              if(queryPaths[3] != string.Empty)
                Database.AddData(queryPaths[1],queryPaths[2],queryPaths[3]);
              else
                Database.AddData(queryPaths[1],queryPaths[2],null);
              return;
            case "UPDATEFIRSTDATA":
              Database.UpdateFirstData(queryPaths[1],queryPaths[2],queryPaths[3]);
              return;
            case "UPDATELASTDATA":
              Database.UpdateLastData(queryPaths[1],queryPaths[2],queryPaths[3]);
              return;
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 5:
          switch(queryPaths[0]) {
            case "UPDATEDATA":
              Database.UpdateData(queryPaths[1],queryPaths[2],int.Parse(queryPaths[3]),queryPaths[4]);
              return;
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        default:
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      }
    }

    #endregion Run

    #region GetRun

    /// <summary>
    /// Runs the active MochaQ query. Returns the incoming value.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public virtual object GetRun(string mochaQ) {
      Command=mochaQ;
      return GetRun();
    }

    /// <summary>
    /// Runs the active MochaQ query. Returns the incoming value.
    /// </summary>
    /// <param name="database">MochaDatabase object that provides management of the targeted MochaDB database.</param>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public virtual object GetRun(MochaDatabase database,string mochaQ) {
      Database = database;
      Command=mochaQ;
      return GetRun();
    }

    /// <summary>
    /// Runs the active MochaQ query. Returns the incoming value.
    /// </summary>
    public virtual object GetRun() {
      Database.OnConnectionCheckRequired(this,new EventArgs());

      //Check null.
      if(string.IsNullOrEmpty(Command))
        throw new MochaException("This MochaQ query is empty, invalid!");

      string[] queryPaths = Command.Split(':');
      queryPaths[0]=queryPaths[0].ToUpperInvariant();

      switch(queryPaths.Length) {
        case 1:
          switch(queryPaths[0]) {
            case "GETTABLES":
              return Database.GetTables();
            case "GETPASSWORD":
              return Database.GetPassword();
            case "GETDESCRIPTION":
              return Database.GetDescription();
            case "GETLOGS":
              return Database.GetLogs();
            case "GETDATAS":
              List<MochaData> datas = new List<MochaData>();
              foreach(XElement element in Database.Doc.Root.Element("Tables").Elements())
                datas.AddRange(GETDATAS(element.Name.LocalName));
              return datas;
            case "TABLECOUNT":
              return Database.Doc.Root.Elements().Count();
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 2:
          switch(queryPaths[0]) {
            case "GETTABLE":
              return Database.GetTable(queryPaths[1]);
            case "GETCOLUMNS":
              return Database.GetColumns(queryPaths[1]);
            case "GETFIRSTCOLUMN_NAME":
              return GETFIRSTCOLUMN_NAME(queryPaths[1]);
            case "EXISTSLOG":
              return Database.ExistsLog(queryPaths[1]);
            case "GETROWS":
              return Database.GetRows(queryPaths[1]);
            case "GETDATAS":
              return GETDATAS(queryPaths[1]);
            case "GETTABLEDESCRIPTION":
              return Database.GetTableDescription(queryPaths[1]);
            case "COLUMNCOUNT":
              return COLUMNCOUNT(queryPaths[1]);
            case "ROWCOUNT":
              try {
                return Database.Doc.Root.Element("Tables").Elements(queryPaths[1]).Elements(
                    GETFIRSTCOLUMN_NAME(queryPaths[1])).Elements().Count();
              } catch(Exception excep) {
                throw excep;
              }
            case "DATACOUNT":
              return Database.GetDataCount(queryPaths[1],GETFIRSTCOLUMN_NAME(queryPaths[1]))
                * COLUMNCOUNT(queryPaths[1]);
            case "EXISTSTABLE":
              return Database.ExistsTable(queryPaths[1]);
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 3:
          switch(queryPaths[0]) {
            case "GETCOLUMN":
              return Database.GetColumn(queryPaths[1],queryPaths[2]);
            case "DATACOUNT":
              return Database.GetDataCount(queryPaths[1],queryPaths[2]);
            case "EXISTSCOLUMN":
              return Database.ExistsColumn(queryPaths[1],queryPaths[2]);
            case "GETDATAS":
              return Database.GetDatas(queryPaths[1],queryPaths[2]);
            case "GETCOLUMNDESCRIPTION":
              return Database.GetColumnDescription(queryPaths[1],queryPaths[2]);
            case "GETCOLUMNDATATYPE":
              return Database.GetColumnDataType(queryPaths[1],queryPaths[2]);
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        case 4:
          switch(queryPaths[0]) {
            case "EXISTSDATA":
              return Database.ExistsData(queryPaths[1],queryPaths[2],queryPaths[3]);
            case "GETDATA":
              return Database.GetData(queryPaths[1],queryPaths[2],int.Parse(queryPaths[3]));
            default:
              throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
          }
        default:
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      }
    }

    #endregion GetRun

    #endregion Members

    #region Private Members

    /// <summary>
    /// Return column count of table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    protected virtual int COLUMNCOUNT(string name) {
      if(!Database.ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      IEnumerable<XElement> columnElements = Database.Doc.Root.Element("Tables").Elements(name).Elements();
      return columnElements.Count();
    }

    /// <summary>
    /// Return all datas of table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    protected virtual IEnumerable<MochaData> GETDATAS(string name) {
      if(!Database.ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      List<MochaData> datas = new List<MochaData>();

      foreach(XElement element in Database.Doc.Root.Element("Tables").Elements(name).Elements())
        datas.AddRange(Database.GetDatas(name,element.Name.LocalName));

      return datas;
    }

    /// <summary>
    /// Return first column name of table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    protected virtual string GETFIRSTCOLUMN_NAME(string name) {
      if(!Database.ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      XElement firstColumn = (XElement)Database.Doc.Root.Element("Tables").Element(name).FirstNode;
      return firstColumn == null ? null : firstColumn.Name.LocalName;
    }

    #endregion Private Members

    #region Properties

    /// <summary>
    /// Is embedded query in database.
    /// </summary>
    public virtual bool IsDatabaseEmbedded { get; internal protected set; }

    /// <summary>
    /// MochaDatabase object to use querying.
    /// </summary>
    public virtual MochaDatabase Database {
      get => database;
      set {
        if(IsDatabaseEmbedded)
          throw new MochaException("This is embedded in database, can not set database!");

        if(value == null)
          throw new MochaException("This MochaDatabase is not affiliated with a database!");

        if(value == database)
          return;

        database = value;
      }
    }

    /// <summary>
    /// Active MochaQ query.
    /// </summary>
    public virtual string Command { get; set; }

    #endregion Properties
  }
}
