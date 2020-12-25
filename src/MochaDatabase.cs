//
// MIT License
//
// Copyright (c) 2020 Mertcan Davulcu
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

namespace MochaDB {
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Xml.Linq;

  using MochaDB.engine;
  using MochaDB.framework;
  using MochaDB.Logging;
  using MochaDB.Mochaq;

  /// <summary>
  /// MochaDatabase provides management of a MochaDB database.
  /// </summary>
  public class MochaDatabase {
    #region Fields

    private static string
        Iv = "MochaDB#$#3{2533",
        Key = "MochaDBM6YxoFsLXu33FpJdjX0R89xGF";

    internal volatile XDocument CDoc = null;
    private FileStream sourceStream = null;

    private string path, password;
    private bool autoCreate;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new instance of <see cref="MochaDatabase"/>.
    /// </summary>
    /// <param name="path">Directory path of MochaDB database.</param>
    /// <param name="password">Password of MochaDB database.</param>
    /// <param name="logs">If true, a copy of the database is kept in database whenever the content changes.</param>
    /// <param name="autoConnect">If true, it is automatically connected to the database.</param>
    /// <param name="autoCreate">If True, a new database will be created if there is no database every time a connection is opened.</param>
    /// <param name="readOnly">If true, cannot task of write in database.</param>
    public MochaDatabase(string path,string password = "",bool logs = false,bool autoConnect = false,
      bool autoCreate = false,bool readOnly = false) {
      SuspendChangeEvents=false;
      State=MochaConnectionState.Disconnected;
      this.path = path.EndsWith(".mhdb") ? path : path + ".mhdb";
      this.password = password;
      Logs = logs;
      this.autoCreate = autoCreate;
      ReadOnly = readOnly;

      if(autoConnect)
        Connect();
    }

    #endregion Constructors

    #region Internal Events

    /// <summary>
    /// This happens before connection check.
    /// </summary>
    internal event EventHandler<EventArgs> ConnectionCheckRequired;
    internal void OnConnectionCheckRequired(object sender,EventArgs e) {
      //Invoke.
      ConnectionCheckRequired?.Invoke(sender,e);

      if(State!=MochaConnectionState.Connected)
        throw new MochaException("Connection is not open!");
    }

    #endregion Internal Events

    #region Events

    /// <summary>
    /// This happens before content changed.
    /// </summary>
    public event EventHandler<EventArgs> Changing;
    internal void OnChanging(object sender,EventArgs e,bool updateCDoc = true) {
      if(updateCDoc)
        CDoc = new XDocument(Doc);

      if(SuspendChangeEvents)
        return;

      //Invoke.
      Changing?.Invoke(sender,e);
      if(Logs)
        KeepLog();
    }

    /// <summary>
    /// This happens after content changed.
    /// </summary>
    public event EventHandler<EventArgs> Changed;
    internal void OnChanged(object sender,EventArgs e) {
      CDoc = null;

      if(SuspendChangeEvents)
        return;

      //Invoke.
      Changed?.Invoke(sender,e);
    }

    #endregion Events

    #region Static Members

    #region Database

    /// <summary>
    /// Returns true if the file in the path is Mocha DB and false otherwise.
    /// </summary>
    /// <param name="path">Path to check.</param>
    public static bool IsMochaDB(string path) {
      if(!File.Exists(path))
        return false;
      FileInfo fInfo = new FileInfo(path);
      if(fInfo.Extension != Engine_LEXER.Extension)
        return false;
      return true;
    }

    /// <summary>
    /// Create new MochaDB database.
    /// </summary>
    /// <param name="path">The file path to be created. (Including name, excluding extension)</param>
    /// <param name="description">Description of database.</param>
    /// <param name="password">Password of database.</param>
    public static void CreateMochaDB(string path,string description,string password) {
      Engine_VALUES.PasswordCheckThrow(password);
      string content = Engine_LEXER.Empty;

      if(!string.IsNullOrEmpty(password)) {
        int dex = content.IndexOf("</Password>");
        content = content.Insert(dex,password);
      }
      if(!string.IsNullOrEmpty(description)) {
        int dex = content.IndexOf("</Description>");
        content = content.Insert(dex,description);
      }

      path += !path.EndsWith(Engine_LEXER.Extension) ?
              Engine_LEXER.Extension : string.Empty;

      if(File.Exists(path))
        throw new MochaException("Such a database already exists!");

      File.WriteAllText(path,aes.Encrypt(Iv,Key,content));
    }

    /// <summary>
    /// Checks the suitability and robustness of the MochaDB database.
    /// </summary>
    /// <param name="path">The file path of the MochaDB database to be checked.</param>
    public static bool CheckMochaDB(string path) {
      if(!IsMochaDB(path))
        throw new MochaException("The file shown is not a MochaDB database file!");

      try {
        XDocument document = XDocument.Parse(aes.Decrypt(Iv,Key,File.ReadAllText(path)));
        return Engine_STRUCTURE.CheckMochaDB(document);
      } catch { return false; }
    }

    #endregion Database

    #endregion Static Members

    #region Internal Members

    #region Database

    /// <summary>
    /// Checks the suitability and robustness of the MochaDB database.
    /// </summary>
    internal bool CheckMochaDB() {
      OnConnectionCheckRequired(this,new EventArgs());
      return Engine_STRUCTURE.CheckMochaDB(Doc);
    }

    /// <summary>
    /// Return element by path.
    /// </summary>
    /// <param name="doc">XDocument.</param>
    /// <param name="path">Path of element.</param>
    internal XElement GetXElement(XDocument doc,string path) {
      OnConnectionCheckRequired(this,new EventArgs());
      return Framework_XML.GetXElement(doc,path);
    }

    /// <summary>
    /// Save MochaDB database.
    /// </summary>
    internal void Save() {
      if(ReadOnly)
        throw new MochaException("This connection is can read only, cannot task of write!");

      password = CDoc.Root.Element("Root").Element("Password").Value;
      Disconnect();
      File.WriteAllText(path,aes.Encrypt(Iv,Key,CDoc.ToString()));
      Connect();

      OnChanged(this,new EventArgs());
    }

    /// <summary>
    /// Keep log of database.
    /// </summary>
    internal void KeepLog() {
      string generateId() {
        Random rnd = new Random();
        string value = string.Empty;
        for(int counter = 1; counter <= 16; ++counter)
          value += $"{rnd.Next(0,9).GetHashCode()}";
        return value;
      }
      string id;
      do { id = generateId(); } while(ExistsLog(id));
      XElement xLog = new XElement("Log");
      xLog.Add(new XAttribute("Id",id));
      xLog.Add(new XAttribute("Time",DateTime.Now));
      string content = aes.Encrypt(Iv,Key,Doc.ToString());
      xLog.Value=content;
      XElement xLogs = GetXElement(CDoc,"Logs");
      IEnumerable<XElement> logElements = xLogs.Elements();
      if(logElements.Count() >= 1000)
        logElements.Last().Remove();
      xLogs.Add(xLog);
    }

    #endregion Database

    #region Data

    /// <summary>
    /// Add data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">MochaData object to add.</param>
    internal void InternalAddData(string tableName,string columnName,MochaData data) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");

      XElement xData = new XElement("Data");

      MochaDataType dataType = GetColumnDataType(tableName,columnName);
      if(dataType == MochaDataType.AutoInt)
        xData.Value = (1 + GetColumnAutoIntState(tableName,columnName)).ToString();
      else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.Data.ToString())) {
        if(ExistsData(tableName,columnName,data))
          throw new MochaException("Any value can be added to a unique column only once!");
        if(!MochaData.IsType(dataType,data.Data))
          throw new MochaException("The submitted data is not compatible with the targeted data!");
        xData.Value = data.Data.ToString();
      } else {
        if(!MochaData.IsType(dataType,data.Data))
          throw new MochaException("The submitted data is not compatible with the targeted data!");
        xData.Value = data.Data.ToString();
      }

      foreach(XElement element in CDoc.Root.Element("Tables").Element(tableName).Elements()) {
        if(element.Name.LocalName==columnName)
          continue;

        MochaDataType _dataType = GetColumnDataType(tableName,element.Name.LocalName);
        if(_dataType == MochaDataType.AutoInt) {
          element.Add(
              new XElement("Data",
              1 + GetColumnAutoIntState(tableName,element.Name.LocalName)));
          continue;
        }

        element.Add(
            new XElement("Data",
            MochaData.TryGetData(GetColumnDataType(tableName,element.Name.LocalName),string.Empty)));
      }
      string val = xData.Value;
      Engine_VALUES.CheckFloat(dataType,ref val);
      xData.Value = val;
      GetXElement(CDoc,$"Tables/{tableName}/{columnName}").Add(xData);
    }

    /// <summary>
    /// Update data by index.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="index">Index of data.</param>
    /// <param name="data">Data to replace.</param>
    internal void InternalUpdateData(string tableName,string columnName,int index,object data) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");

      data = data ?? string.Empty;
      MochaDataType dataType = GetColumnDataType(tableName,columnName);
      if(!MochaData.IsType(dataType,data))
        throw new MochaException("The submitted data is not compatible with the targeted data!");

      XElement xColumn = GetXElement(CDoc,$"Tables/{tableName}/{columnName}");
      IEnumerable<XElement> dataRange = xColumn.Elements();
      if(dataRange.Count() - 1 < index)
        throw new MochaException("This index is larger than the maximum number of data in the table!");

      XElement dataElement = dataRange.ElementAt(index);
      string ddata = data.ToString();
      Engine_VALUES.CheckFloat(dataType,ref ddata);
      if(dataElement.Value==ddata)
        return;

      dataElement.Value=ddata;
    }

    /// <summary>
    /// Update first data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to replace.</param>
    internal void InternalUpdateFirstData(string tableName,string columnName,object data) =>
      InternalUpdateData(tableName,columnName,0,data);

    /// <summary>
    /// Update last data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to replace.</param>
    internal void InternalUpdateLastData(string tableName,string columnName,object data) =>
      InternalUpdateData(tableName,columnName,
          GetDataCount(tableName,columnName) - 1,data);

    #endregion Data

    #endregion Internal Members

    #region Members

    #region Connection

    /// <summary>
    /// Connect to database.
    /// </summary>
    public void Connect() {
      if(State == MochaConnectionState.Connected)
        return;

      State = MochaConnectionState.Connected;

      if(!File.Exists(path)) {
        if(autoCreate)
          CreateMochaDB(path,string.Empty,string.Empty);
        else
          throw new MochaException("There is no MochaDB database file in the specified path!");
      } else {
        if(!IsMochaDB(path))
          throw new MochaException("The file shown is not a MochaDB database file!");
      }

      Doc = XDocument.Parse(aes.Decrypt(Iv,Key,File.ReadAllText(path,Encoding.UTF8)));

      if(!CheckMochaDB())
        throw new MochaException("The MochaDB database is corrupt!");
      if(!string.IsNullOrEmpty(GetPassword()) && string.IsNullOrEmpty(password))
        throw new MochaException("The MochaDB database is password protected!");
      else if(password != GetPassword())
        throw new MochaException("MochaDB database password does not match the password specified!");

      FileInfo fInfo = new FileInfo(path);
      Name = fInfo.Name.Substring(0,fInfo.Name.Length - fInfo.Extension.Length);
      sourceStream = File.Open(path,FileMode.Open,FileAccess.ReadWrite);
      Query = new MochaQuery(this,true);
    }

    /// <summary>
    /// Disconnect from database.
    /// </summary>
    public void Disconnect() {
      if(State == MochaConnectionState.Disconnected)
        return;

      State = MochaConnectionState.Disconnected;

      sourceStream.Dispose();
      Doc = null;
      Query = null;
    }

    #endregion Connection

    #region Database

    /// <summary>
    /// Returns the password of the MochaDB database.
    /// </summary>
    public string GetPassword() {
      OnConnectionCheckRequired(this,new EventArgs());
      return GetXElement(Doc,"Root/Password").Value;
    }

    /// <summary>
    /// Sets the MochaDB Database password.
    /// </summary>
    /// <param name="password">Password to set.</param>
    public void SetPassword(string password) {
      OnConnectionCheckRequired(this,new EventArgs());
      OnChanging(this,new EventArgs());
      Engine_VALUES.PasswordCheckThrow(password);
      GetXElement(CDoc,"Root/Password").Value = password;
      Save();
    }

    /// <summary>
    /// Returns the description of the database.
    /// </summary>
    public string GetDescription() {
      OnConnectionCheckRequired(this,new EventArgs());
      return GetXElement(Doc,"Root/Description").Value;
    }

    /// <summary>
    /// Sets the description of the database.
    /// </summary>
    /// <param name="Description">Description to set.</param>
    public void SetDescription(string Description) {
      OnConnectionCheckRequired(this,new EventArgs());
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,"Root/Description").Value = Description;
      Save();
    }

    /// <summary>
    /// Remove all tables and others.
    /// </summary>
    public void ClearAll() {
      OnConnectionCheckRequired(this,new EventArgs());
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,"Tables").RemoveNodes();
      Save();
    }

    /// <summary>
    /// MochaDB checks the existence of the database file and if not creates a new file. ALL DATA IS LOST!
    /// </summary>
    public void Reset() {
      OnConnectionCheckRequired(this,new EventArgs());
      OnChanging(this,new EventArgs());

      Doc = XDocument.Parse(Engine_LEXER.Empty);
      Save();
    }

    #endregion Database

    #region Table

    /// <summary>
    /// Remove all tables.
    /// </summary>
    public void ClearTables() {
      OnConnectionCheckRequired(this,new EventArgs());
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,"Tables").RemoveNodes();
      Save();
    }

    /// <summary>
    /// Add table.
    /// </summary>
    /// <param name="table">MochaTable object to add.</param>
    public void AddTable(MochaTable table) {
      if(ExistsTable(table.Name))
        throw new MochaException("There is already a table with this name!");
      OnChanging(this,new EventArgs());

      XElement xTable = new XElement(table.Name);
      xTable.Add(new XAttribute("Description",table.Description));
      GetXElement(CDoc,"Tables").Add(xTable);

      // Columns.
      for(int index = 0; index < table.Columns.Count; ++index) {
        MochaColumn column = table.Columns[index];
        XElement Xcolumn = new XElement(column.Name);
        Xcolumn.Add(new XAttribute("DataType",column.DataType));
        Xcolumn.Add(new XAttribute("Description",column.Description));
        for(int dindex = 0; dindex < column.Datas.Count; ++dindex)
          Xcolumn.Add(new XElement("Data",column.Datas[dindex].Data));
        xTable.Add(Xcolumn);
      }
      Save();
    }

    /// <summary>
    /// Create new table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    public void CreateTable(string name) {
      MochaTable table = new MochaTable(name);
      AddTable(table);
    }

    /// <summary>
    /// Remove table by name. Returns true if table is exists and removed.
    /// </summary>
    /// <param name="name">Name of table.</param>
    public bool RemoveTable(string name) {
      if(!ExistsTable(name))
        return false;
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,$"Tables/{name}").Remove();
      Save();
      return true;
    }

    /// <summary>
    /// Rename table.
    /// </summary>
    /// <param name="name">Name of table to rename.</param>
    /// <param name="newName">New name of table.</param>
    public void RenameTable(string name,string newName) {
      if(!ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      if(name == newName)
        return;

      if(ExistsTable(newName))
        throw new MochaException("There is already a table with this name!");

      Engine_NAMES.CheckThrow(newName);

      OnChanging(this,new EventArgs());

      GetXElement(CDoc,$"Tables/{name}").Name=newName;
      Save();
    }

    /// <summary>
    /// Returns description of table by name.
    /// </summary>
    /// <param name="name">Name of table.</param>
    public string GetTableDescription(string name) =>
      !ExistsTable(name) ?
        throw new MochaException("Table not found in this name!") :
        GetXElement(Doc,$"Tables/{name}").Attribute("Description").Value;

    /// <summary>
    /// Set description of table by name.
    /// </summary>
    /// <param name="name">Name of table.</param>
    /// <param name="description">Description to set.</param>
    public void SetTableDescription(string name,string description) {
      if(!ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      XAttribute xDescription = GetXElement(CDoc = new XDocument(Doc),$"Tables/{name}").Attribute("Description");
      if(xDescription.Value==description)
        return;
      OnChanging(this,new EventArgs(),false);

      xDescription.Value=description;
      Save();
    }

    /// <summary>
    /// Returns table by name.
    /// </summary>
    /// <param name="name">Name of table.</param>
    public MochaTable GetTable(string name) {
      if(!ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      XElement xTable = GetXElement(Doc,$"Tables/{name}");
      MochaTable table = new MochaTable(name);
      table.Description=xTable.Attribute("Description").Value;
      table.Columns.collection.AddRange(GetColumns(name));
      table.SetRowsByDatas();
      return table;
    }

    /// <summary>
    /// Returns all tables in database.
    /// </summary>
    public List<MochaTable> GetTables() {
      OnConnectionCheckRequired(this,new EventArgs());

      List<MochaTable> tables = new List<MochaTable>();
      foreach(XElement element in GetXElement(Doc,"Tables").Elements())
        tables.Add(GetTable(element.Name.LocalName));
      return tables;
    }

    /// <summary>
    /// Returns whether there is a table with the specified name.
    /// </summary>
    /// <param name="name">Name of table.</param>
    public bool ExistsTable(string name) =>
        GetXElement(Doc,$"Tables/{name}") != null;

    #endregion Table

    #region Column

    /// <summary>
    /// Add colum in table.
    /// </summary>
    /// <param name="tableName">Name of column.</param>
    /// <param name="column">MochaColumn object to add.</param>
    public void AddColumn(string tableName,MochaColumn column) {
      if(ExistsColumn(tableName,column.Name))
        throw new MochaException("There is already a column with this name!");
      OnChanging(this,new EventArgs());

      XElement xColumn = new XElement(column.Name);
      xColumn.Add(new XAttribute("Description",column.Description));
      xColumn.Add(new XAttribute("DataType",column.DataType));
      GetXElement(CDoc,$"Tables/{tableName}").Add(xColumn);

      // Datas.
      int rowCount = (int)Query.GetRun($"ROWCOUNT:{tableName}");
      if(column.DataType==MochaDataType.AutoInt)
        for(int index = 1; index <= rowCount; ++index)
          xColumn.Add(new XElement("Data",index));
      else {
        for(int index = 0; index < column.Datas.Count; ++index)
          xColumn.Add(new XElement("Data",column.Datas[index].Data));

        for(int index = column.Datas.Count; index < rowCount; ++index)
          xColumn.Add(new XElement("Data",MochaData.TryGetData(column.DataType,string.Empty)));
      }
      Save();
    }

    /// <summary>
    /// Create column in table.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public void CreateColumn(string tableName,string name) =>
      AddColumn(tableName,new MochaColumn(name,dataType: MochaDataType.String));

    /// <summary>
    /// Remove column from table by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public bool RemoveColumn(string tableName,string name) {
      if(!ExistsColumn(tableName,name))
        return false;
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,$"Tables/{tableName}/{name}").Remove();
      Save();
      return true;
    }

    /// <summary>
    /// Rename column.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column to rename.</param>
    /// <param name="newName">New name of column.</param>
    public void RenameColumn(string tableName,string name,string newName) {
      if(!ExistsColumn(tableName,name))
        throw new MochaException("Column not found in this name!");
      if(name == newName)
        return;
      if(ExistsColumn(tableName,newName))
        throw new MochaException("There is already a column with this name!");

      Engine_NAMES.CheckThrow(newName);
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,$"Tables/{tableName}/{name}").Name=newName;
      Save();
    }

    /// <summary>
    /// Returns description of column by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public string GetColumnDescription(string tableName,string name) =>
      !ExistsColumn(tableName,name) ?
        throw new MochaException("Column not found in this name!") :
        GetXElement(Doc,$"Tables/{tableName}/{name}").Attribute("Description").Value;

    /// <summary>
    /// Set description of column by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    /// <param name="description">Description to set.</param>
    public void SetColumnDescription(string tableName,string name,string description) {
      if(!ExistsColumn(tableName,name))
        throw new MochaException("Column not found in this name!");

      XAttribute xDescription = GetXElement(CDoc = new XDocument(Doc),$"Tables/{tableName}/{name}").Attribute("Description");
      if(xDescription.Value==description)
        return;

      OnChanging(this,new EventArgs(),false);

      xDescription.Value = description;
      Save();
    }

    /// <summary>
    /// Returns column from table by name
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public MochaColumn GetColumn(string tableName,string name) {
      if(!ExistsColumn(tableName,name))
        throw new MochaException("Column not found in this name!");

      MochaColumn column = new MochaColumn(name,dataType: GetColumnDataType(tableName,name));
      column.MHQLAsText = name;
      column.Description = GetColumnDescription(tableName,name);
      column.Datas.collection.AddRange(GetDatas(tableName,name));
      return column;
    }

    /// <summary>
    /// Returns all columns in table by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    public List<MochaColumn> GetColumns(string tableName) {
      if(!ExistsTable(tableName))
        throw new MochaException("Table not found in this name!");

      List<MochaColumn> columns = new List<MochaColumn>();
      foreach(XElement element in GetXElement(Doc,$"Tables/{tableName}").Elements())
        columns.Add(GetColumn(tableName,element.Name.LocalName));
      return columns;
    }

    /// <summary>
    /// Returns whether there is a column with the specified name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public bool ExistsColumn(string tableName,string name) =>
      !ExistsTable(tableName) ?
        throw new MochaException("Table not found in this name!") :
        GetXElement(Doc,$"Tables/{tableName}/{name}") != null;

    /// <summary>
    /// Returns column datatype by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public MochaDataType GetColumnDataType(string tableName,string name) =>
      !ExistsColumn(tableName,name) ?
        throw new MochaException("Column not found in this name!") :
        (MochaDataType)Enum.Parse(typeof(MochaDataType),
          GetXElement(Doc,$"Tables/{tableName}/{name}").Attribute("DataType").Value);

    /// <summary>
    /// Set column datatype by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    /// <param name="dataType">MochaDataType to set.</param>
    public void SetColumnDataType(string tableName,string name,MochaDataType dataType) {
      if(!ExistsColumn(tableName,name))
        throw new MochaException("Column not found in this name!");
      OnChanging(this,new EventArgs());

      XElement xColumn = GetXElement(CDoc,$"Tables/{tableName}/{name}");
      if(xColumn.Attribute("DataType").Value==dataType.ToString())
        return;

      xColumn.Attribute("DataType").Value = dataType.ToString();
      IEnumerable<XElement> dataRange = xColumn.Elements();
      if(dataType == MochaDataType.AutoInt) {
        int counter = 1;
        foreach(XElement element in dataRange)
          element.Value = counter++.ToString();
        Save();
        return;
      } else if(dataType == MochaDataType.Unique) {
        foreach(XElement element in dataRange)
          element.Value = string.Empty;
        Save();
        return;
      }
      foreach(XElement element in dataRange)
        if(!MochaData.IsType(dataType,element.Value))
          element.Value = MochaData.TryGetData(dataType,element.Value).ToString();
      Save();
    }

    /// <summary>
    /// Returns column's last AutoInt value by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="name">Name of column.</param>
    public int GetColumnAutoIntState(string tableName,string name) {
      if(!ExistsColumn(tableName,name))
        throw new MochaException("Column not found in this name!");

      XElement lastData = (XElement)GetXElement(Doc,$"Tables/{tableName}/{name}").LastNode;
      MochaDataType dataType = GetColumnDataType(tableName,name);

      if(dataType != MochaDataType.AutoInt)
        throw new MochaException("This column's datatype is not AutoInt!");
      if(lastData != null)
        return int.Parse(lastData.Value);
      else
        return 0;
    }

    #endregion Column

    #region Row

    /// <summary>
    /// Add row in table.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="row">MochaRow object to add.</param>
    public void AddRow(string tableName,MochaRow row) {
      if(!ExistsTable(tableName))
        throw new MochaException("Table not found in this name!");
      OnChanging(this,new EventArgs());

      IEnumerable<XElement> columnRange = GetXElement(CDoc,$"Tables/{tableName}").Elements();

      if(columnRange.Count() != row.Datas.Count)
        throw new MochaException("The data count of the row is not equal to the number of columns!");

      int dex = GetDataCount(tableName,columnRange.First().Name.LocalName);
      InternalAddData(tableName,columnRange.First().Name.LocalName,row.Datas[0]);
      int index = 0;
      foreach(XElement columnElement in columnRange) {
        MochaDataType dataType =
            GetColumnDataType(tableName,columnElement.Name.LocalName);
        if(dataType == MochaDataType.AutoInt)
          continue;
        InternalUpdateData(tableName,columnElement.Name.LocalName,dex,row.Datas[index++].Data);
      }
      Save();
    }

    /// <summary>
    /// Add row in table.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="datas">Datas of row.</param>
    public void AddRow(string tableName,params object[] datas) =>
      AddRow(tableName,new MochaRow(datas));

    /// <summary>
    /// Remove row from table by index.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="index">Index of row to remove.</param>
    public bool RemoveRow(string tableName,int index) {
      if(!ExistsTable(tableName))
        throw new MochaException("Table not found in this name!");

      IEnumerable<XElement> columnRange = GetXElement(CDoc = new XDocument(Doc),
        $"Tables/{tableName}").Elements();
      if(columnRange.First().Elements().Count()-1 >= index) {
        OnChanging(this,new EventArgs(),false);
        foreach(XElement element in columnRange)
          element.Elements().ElementAt(index).Remove();
        Save();
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns row from table by index.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="index">Index of row.</param>
    public MochaRow GetRow(string tableName,int index) {
      if(!ExistsTable(tableName))
        throw new MochaException("Table not found in this name!");
      if(index < 0)
        throw new MochaException("Index can not lower than 0!");

      List<MochaColumn> columns = GetColumns(tableName);

      if(columns.Count == 0)
        return null;

      int rowCount = (int)Query.GetRun($"ROWCOUNT:{tableName}");
      if(rowCount-1 < index)
        throw new MochaException("Index cat not bigger than row count!");

      MochaRow row = new MochaRow();
      MochaData[] datas = new MochaData[columns.Count];
      for(int columnIndex = 0; columnIndex < columns.Count; ++columnIndex)
        datas[columnIndex] = columns[columnIndex].Datas[index];
      row.Datas.collection.AddRange(datas);
      return row;
    }

    /// <summary>
    /// Returns all rows in table by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    public MochaRow[] GetRows(string tableName) {
      if(!ExistsTable(tableName))
        throw new MochaException("Table not found in this name!");

      XElement firstColumn = (XElement)GetXElement(Doc,$"Tables/{tableName}").FirstNode;

      if(firstColumn==null)
        return new MochaRow[0];

      int dataCount = GetDataCount(tableName,firstColumn.Name.LocalName);
      MochaRow[] rows = new MochaRow[dataCount];
      for(int index = 0; index < dataCount; ++index)
        rows[index] = GetRow(tableName,index);
      return rows;
    }

    /// <summary>
    /// Clear all rows of table.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    public void ClearRows(string tableName) {
      if(!ExistsTable(tableName))
        throw new MochaException("Table not found in this name!");

      bool changed = false;
      foreach(XElement element in GetXElement(CDoc = new XDocument(Doc),$"Tables/{tableName}").Elements()) {
        if(!changed)
          OnChanging(this,new EventArgs(),false);
        changed = true;
        element.RemoveNodes();
      }
      if(changed)
        Save();
    }

    #endregion Row

    #region Data

    /// <summary>
    /// Add data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">MochaData object to add.</param>
    public void AddData(string tableName,string columnName,MochaData data) {
      OnChanging(this,new EventArgs());
      InternalAddData(tableName,columnName,data);
      Save();
    }

    /// <summary>
    /// Add data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to add.</param>
    public void AddData(string tableName,string columnName,object data) =>
      AddData(tableName,columnName,new MochaData(GetColumnDataType(tableName,columnName),data));

    /// <summary>
    /// Update data by index.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="index">Index of data.</param>
    /// <param name="data">Data to replace.</param>
    public void UpdateData(string tableName,string columnName,int index,object data) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");

      MochaDataType dataType = GetColumnDataType(tableName,columnName);
      if(dataType == MochaDataType.AutoInt)
        throw new MochaException("The data type of this column is AutoInt, so data update cannot be done!");
      else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.ToString())) {
        int dex = GetDataIndex(tableName,columnName,data.ToString());
        if(dex != -1 && dex != index)
          throw new MochaException("Any value can be added to a unique column only once!");
        else if(dex == index)
          return;
      }

      OnChanging(this,new EventArgs());
      InternalUpdateData(tableName,columnName,index,data);
      Save();
    }

    /// <summary>
    /// Update first data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to replace.</param>
    public void UpdateFirstData(string tableName,string columnName,object data) =>
      UpdateData(tableName,columnName,0,data);

    /// <summary>
    /// Update last data.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to replace.</param>
    public void UpdateLastData(string tableName,string columnName,object data) =>
      UpdateData(tableName,columnName,
          GetDataCount(tableName,columnName) - 1,data);

    /// <summary>
    /// Returns whether there is a data with the specified.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">MochaData object to check.</param>
    public bool ExistsData(string tableName,string columnName,MochaData data) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");

      string stringData = data.Data.ToString();
      foreach(XElement element in GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements())
        if(element.Value == stringData)
          return true;
      return false;
    }

    /// <summary>
    /// Returns whether there is a data with the specified.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to check.</param>
    public bool ExistsData(string tableName,string columnName,object data) =>
      ExistsData(tableName,columnName,new MochaData { data = data });

    /// <summary>
    /// Returns data index. If there are two of the same data, it returns the index of the one you found first!
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="data">Data to find index.</param>
    public int GetDataIndex(string tableName,string columnName,object data) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");

      string stringData = data.ToString();
      int index = 0;
      foreach(XElement element in GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements()) {
        if(element.Value == stringData)
          return index;
        ++index;
      }
      return -1;
    }

    /// <summary>
    /// Returns data by index.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    /// <param name="index">Index of data.</param>
    public MochaData GetData(string tableName,string columnName,int index) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");

      if(index < 0)
        throw new MochaException("Index can not lower than 0!");

      MochaDataType dataType = GetColumnDataType(tableName,columnName);
      IEnumerable<XElement> dataRange = GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements();
      if(dataRange.Count() - 1 < index)
        throw new MochaException("This index is larger than the maximum number of data in the table!");

      return new MochaData { dataType = dataType,
                             data = MochaData.GetDataFromString(dataType,dataRange.ElementAt(index).Value) };
    }

    /// <summary>
    /// Returns all datas in column in table by name.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    public List<MochaData> GetDatas(string tableName,string columnName) {
      if(!ExistsColumn(tableName,columnName))
        throw new MochaException("Column not found in this name!");
      MochaDataType dataType = GetColumnDataType(tableName,columnName);
      List<MochaData> datas = new List<MochaData>();
      foreach(XElement element in GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements())
        datas.Add(new MochaData { dataType = dataType,
                                  data = MochaData.GetDataFromString(dataType, element.Value) });
      return datas;
    }

    /// <summary>
    /// Returns data count of table's column.
    /// </summary>
    /// <param name="tableName">Name of table.</param>
    /// <param name="columnName">Name of column.</param>
    public int GetDataCount(string tableName,string columnName) =>
      !ExistsColumn(tableName,columnName) ?
        throw new MochaException("Column not found in this name!") :
        GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements().Count();

    #endregion Data

    #region Log

    /// <summary>
    /// Clear all logs.
    /// </summary>
    public void ClearLogs() {
      OnChanging(this,new EventArgs());

      GetXElement(CDoc,"Logs").RemoveNodes();
      Save();
    }

    /// <summary>
    /// Returns all logs.
    /// </summary>
    public List<MochaLog> GetLogs() {
      List<MochaLog> logs = new List<MochaLog>();
      foreach(XElement element in GetXElement(Doc,"Logs").Elements())
        logs.Add(new MochaLog(element.Attribute("Id").Value,
                              element.Value,
                              DateTime.Parse(element.Attribute("Time").Value)));
      return logs;
    }

    /// <summary>
    /// Restore database to last keeped log.
    /// Returns false if not exists any log, true if not.
    /// </summary>
    public bool RestoreToLastLog() {
      IEnumerable<XElement> logs = GetXElement(Doc,"Logs").Elements();
      if(!logs.Any())
        return false; //throw new MochaException("Not exists any log!");
      RestoreToLog(logs.Last().Attribute("Id").Value);
      return true;
    }

    /// <summary>
    /// Restore database to first keeped log.
    /// Returns false if not exists any log, true if not.
    /// </summary>
    public bool RestoreToFirstLog() {
      IEnumerable<XElement> logs = GetXElement(Doc,"Logs").Elements();
      if(!logs.Any())
        return false; // throw new MochaException("Not exists any log!");
      RestoreToLog(logs.First().Attribute("Id").Value);
      return true;
    }

    /// <summary>
    /// Restore database to log by id.
    /// </summary>
    /// <param name="id">ID of log to restore.</param>
    public void RestoreToLog(string id) {
      if(!ExistsLog(id))
        throw new MochaException("Log not found in this id!");
      Changing?.Invoke(this,new EventArgs());

      XElement log = GetXElement(Doc,"Logs").Elements().Where(x => x.Attribute("Id").Value==id).First();
      CDoc = XDocument.Parse(aes.Decrypt(Iv,Key,log.Value));
      Save();
    }

    /// <summary>
    /// Returns whether there is a log with the specified id.
    /// </summary>
    /// <param name="id">ID of log.</param>
    public bool ExistsLog(string id) =>
      GetXElement(Doc,"Logs").Elements().Where(x => x.Attribute("Id").Value == id).Count() != 0;

    #endregion Log

    #region XML

    /// <summary>
    /// Return XDocument of database.
    /// </summary>
    public XDocument GetXDocument() {
      OnConnectionCheckRequired(this,new EventArgs());

      XDocument doc = new XDocument(Doc);
      return doc;
    }

    #endregion XML

    /// <summary>
    /// Dispose.
    /// </summary>
    public void Dispose() {
      Disconnect();
      path = password = null;
    }

    #endregion Members

    #region Static Properties

    /// <summary>
    /// Version of MochaDB.
    /// </summary>
    public static string Version =>
        Engine_LEXER.Version;

    #endregion Static Properties

    #region Internal Properties

    /// <summary>
    /// XML Document.
    /// </summary>
    internal XDocument Doc { get; set; }

    /// <summary>
    /// Suspend the changeds events.
    /// </summary>
    internal bool SuspendChangeEvents { get; set; }

    #endregion Internal Properties

    #region Properties

    /// <summary>
    /// Log keeping status.
    /// </summary>
    public bool Logs { get; }

    /// <summary>
    /// Readonly state of connection.
    /// </summary>
    public bool ReadOnly { get; }

    /// <summary>
    /// Mapped MochaQuery.
    /// </summary>
    public MochaQuery Query { get; private set; }

    /// <summary>
    /// State of connection.
    /// </summary>
    public MochaConnectionState State { get; private set; }

    /// <summary>
    /// Name of database.
    /// </summary>
    public string Name { get; private set; }

    #endregion Properties
  }

  /// <summary>
  /// Connection states of MochaDB.
  /// </summary>
  public enum MochaConnectionState {
    Disconnected = 0,
    Connected = 1
  }
}
