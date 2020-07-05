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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MochaDB.Connection;
using MochaDB.engine;
using MochaDB.framework;
using MochaDB.Logging;
using MochaDB.Mochaq;
using MochaDB.Querying;
using MochaDB.Streams;

namespace MochaDB {
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

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaDatabase.
        /// </summary>
        /// <param name="connectionString">Connection string for connect to MochaDb database.</param>
        public MochaDatabase(string connectionString) :
            this(new MochaProvider(connectionString)) { }

        /// <summary>
        /// Create new MochaDatabase.
        /// </summary>
        /// <param name="path">Directory path of MochaDB database.</param>
        /// <param name="password">Password of MochaDB database.</param>
        public MochaDatabase(string path,string password) :
            this(new MochaProvider($"path={path}; password={password}")) { }

        /// <summary>
        /// Create new MochaDatabase.
        /// </summary>
        /// <param name="provider">Provider for connect database.</param>
        public MochaDatabase(MochaProvider provider) {
            provider.EnableConstant();
            SuspendChangeEvents=false;
            Provider=provider;
            State=MochaConnectionState.Disconnected;
            Logs = Provider.GetBoolAttributeState("Logs");

            if(Provider.GetBoolAttributeState("AutoConnect")) {
                Connect();
            }
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaDatabase value) =>
            value.ToString();

        #endregion

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

        #endregion

        #region Events

        /// <summary>
        /// This happens before content changed.
        /// </summary>
        public event EventHandler<EventArgs> Changing;
        internal void OnChanging(object sender,EventArgs e) {
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
            if(SuspendChangeEvents)
                return;

            //Invoke.
            Changed?.Invoke(sender,e);
        }

        #endregion

        #region General

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            Disconnect();
            Provider=null;
        }

        #endregion

        #region Xml

        /// <summary>
        /// Return xml schema of database.
        /// </summary>
        public string GetXML() =>
            GetXDocument().ToString();

        /// <summary>
        /// Return XDocument of database.
        /// </summary>
        public XDocument GetXDocument() {
            OnConnectionCheckRequired(this,new EventArgs());

            XDocument doc = new XDocument(Doc);
            return doc;
        }

        #endregion

        #region Connection

        /// <summary>
        /// Connect to database.
        /// </summary>
        public void Connect() {
            if(State==MochaConnectionState.Connected)
                return;

            State=MochaConnectionState.Connected;

            if(!File.Exists(Provider.Path)) {
                if(Provider.GetBoolAttributeState("AutoCreate"))
                    CreateMochaDB(Provider.Path.Substring(0,Provider.Path.Length-8),"","");
                else
                    throw new MochaException("There is no MochaDB database file in the specified path!");
            } else {
                if(!IsMochaDB(Provider.Path))
                    throw new MochaException("The file shown is not a MochaDB database file!");
            }

            Doc = XDocument.Parse(aes.Decrypt(Iv,Key,File.ReadAllText(Provider.Path,Encoding.UTF8)));

            if(!CheckMochaDB())
                throw new MochaException("The MochaDB database is corrupt!");
            if(!string.IsNullOrEmpty(GetPassword()) && string.IsNullOrEmpty(Provider.Password))
                throw new MochaException("The MochaDB database is password protected!");
            else if(Provider.Password != GetPassword())
                throw new MochaException("MochaDB database password does not match the password specified!");

            FileInfo fInfo = new FileInfo(Provider.Path);

            Name = fInfo.Name.Substring(0,fInfo.Name.Length - fInfo.Extension.Length);

            sourceStream = File.Open(Provider.Path,FileMode.Open,FileAccess.ReadWrite);
            Query = new MochaQuery(this,true);
        }

        /// <summary>
        /// Disconnect from database.
        /// </summary>
        public void Disconnect() {
            if(State==MochaConnectionState.Disconnected)
                return;

            State=MochaConnectionState.Disconnected;

            sourceStream.Dispose();
            Doc=null;
            Query=null;
        }

        #endregion

        #region Static Database

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
            string content = Engine_LEXER.EmptyContent;

            if(!string.IsNullOrEmpty(password)) {
                int dex = content.IndexOf("</Password>");
                content = content.Insert(dex,password);
            }
            if(!string.IsNullOrEmpty(description)) {
                int dex = content.IndexOf("</Description>");
                content = content.Insert(dex,description);
            }

            File.WriteAllText(path.EndsWith(Engine_LEXER.Extension) ?
                path :
                path + Engine_LEXER.Extension,aes.Encrypt(Iv,Key,content));
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

        #endregion

        #region Internal Database

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
        /// Checks for the presence of the element.
        /// </summary>
        /// <param name="doc">XDocument.</param>
        /// <param name="path">Path of element.</param>
        internal bool ExistsElement(XDocument doc,string path) =>
            GetXElement(doc,path) != null;

        /// <summary>
        /// Save MochaDB database.
        /// </summary>
        internal void Save() {
            if(Provider.Readonly)
                throw new MochaException("This connection is can read only, cannot task of write!");

            string content = aes.Encrypt(Iv,Key,CDoc.ToString());
            string password = GetPassword();
            Disconnect();
            File.WriteAllText(Provider.Path,content);
            Provider.Constant=false;
            Provider.ConnectionString=$"path={Provider.Path};password={password}";
            Provider.EnableConstant();
            Connect();

            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Keep log of database.
        /// </summary>
        internal void KeepLog() {
            string id;
            do {
                id = MochaID.GetID(MochaIDType.Hash16);
            } while(ExistsLog(id));
            XElement xLog = new XElement("Log");
            xLog.Add(new XAttribute("ID",id));
            xLog.Add(new XAttribute("Time",DateTime.Now));
            string content = aes.Encrypt(Iv,Key,Doc.ToString());
            xLog.Value=content;
            XElement xLogs = GetXElement(Doc,"Logs");
            IEnumerable<XElement> logElements = xLogs.Elements();
            if(logElements.Count() >= 1000)
                logElements.Last().Remove();
            xLogs.Add(xLog);
        }

        #endregion

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
        /// Remove all sectors, stacks, tables and others.
        /// </summary>
        public void ClearAll() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement(CDoc,"Sectors").RemoveNodes();
            GetXElement(CDoc,"Stacks").RemoveNodes();
            GetXElement(CDoc,"Tables").RemoveNodes();
            Save();
        }

        /// <summary>
        /// MochaDB checks the existence of the database file and if not creates a new file. ALL DATA IS LOST!
        /// </summary>
        public void Reset() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            Doc = XDocument.Parse(Engine_LEXER.EmptyContent);
            Save();
        }

        #endregion

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
            xTable.Add(new XAttribute("Attributes",string.Empty));
            GetXElement(CDoc,"Tables").Add(xTable);

            // Columns.
            for(int index = 0;index < table.Columns.Count;index++) {
                var column = table.Columns[index];
                XElement Xcolumn = new XElement(column.Name);
                Xcolumn.Add(new XAttribute("DataType",column.DataType));
                Xcolumn.Add(new XAttribute("Description",column.Description));
                for(int dindex = 0;dindex < column.Datas.Count;dindex++)
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
        public string GetTableDescription(string name) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");

            return GetXElement(Doc,$"Tables/{name}").Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="description">Description to set.</param>
        public void SetTableDescription(string name,string description) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");

            XAttribute xDescription = GetXElement(CDoc,$"Tables/{name}").Attribute("Description");
            if(xDescription.Value==description)
                return;
            OnChanging(this,new EventArgs());

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
            //table.Rows.collection.AddRange(GetRows(name));

            return table;
        }

        /// <summary>
        /// Returns all tables in database.
        /// </summary>
        public MochaTable[] GetTables() {
            OnConnectionCheckRequired(this,new EventArgs());

            IEnumerable<XElement> tableRange = GetXElement(Doc,"Tables").Elements();
            MochaTable[] tables = new MochaTable[tableRange.Count()];
            for(int index = 0;index <tables.Length;index++) {
                tables[index] = GetTable(tableRange.ElementAt(index).Name.LocalName);
            }

            return tables;
        }

        /// <summary>
        /// Returns whether there is a table with the specified name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public bool ExistsTable(string name) =>
            ExistsElement(Doc,$"Tables/{name}");

        #endregion

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
            xColumn.Add(new XAttribute("DataType",column.DataType));
            xColumn.Add(new XAttribute("Description",column.Description));
            xColumn.Add(new XAttribute("Attributes",string.Empty));
            GetXElement(CDoc,$"Tables/{tableName}").Add(xColumn);

            // Datas.
            int rowCount = (MochaResult<int>)Query.GetRun($"ROWCOUNT:{tableName}");
            if(column.DataType==MochaDataType.AutoInt) {
                for(int index = 1;index <= rowCount;index++)
                    xColumn.Add(new XElement("Data",index));
            } else {
                for(int index = 0;index < column.Datas.Count;index++)
                    xColumn.Add(new XElement("Data",column.Datas[index].Data));

                for(int index = column.Datas.Count;index < rowCount;index++) {
                    xColumn.Add(new XElement("Data",MochaData.TryGetData(column.DataType,"")));
                }
            }

            Save();
        }

        /// <summary>
        /// Create column in table.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public void CreateColumn(string tableName,string name) {
            AddColumn(tableName,new MochaColumn(name,MochaDataType.String));
        }

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
        public string GetColumnDescription(string tableName,string name) {
            if(!ExistsColumn(tableName,name))
                throw new MochaException("Column not found in this name!");

            return GetXElement(Doc,$"Tables/{tableName}/{name}").Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of column by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="description">Description to set.</param>
        public void SetColumnDescription(string tableName,string name,string description) {
            if(!ExistsColumn(tableName,name))
                throw new MochaException("Column not found in this name!");

            XAttribute xDescription = GetXElement(CDoc,$"Tables/{tableName}/{name}").Attribute("Description");
            if(xDescription.Value==description)
                return;
            OnChanging(this,new EventArgs());

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

            MochaColumn column = new MochaColumn(name,GetColumnDataType(tableName,name));
            column.MHQLAsText = name;
            column.Description = GetColumnDescription(tableName,name);
            column.Datas.collection.AddRange(GetDatas(tableName,name));

            return column;
        }

        /// <summary>
        /// Returns all columns in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public MochaColumn[] GetColumns(string tableName) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            IEnumerable<XElement> columnsRange = GetXElement(Doc,$"Tables/{tableName}").Elements();
            MochaColumn[] columns = new MochaColumn[columnsRange.Count()];
            for(int index = 0;index < columns.Length;index++) {
                columns[index] = GetColumn(tableName,columnsRange.ElementAt(index).Name.LocalName);
            }

            return columns;
        }

        /// <summary>
        /// Returns whether there is a column with the specified name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public bool ExistsColumn(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            return GetXElement(Doc,$"Tables/{tableName}/{name}") != null;
        }

        /// <summary>
        /// Returns column datatype by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public MochaDataType GetColumnDataType(string tableName,string name) {
            if(!ExistsColumn(tableName,name))
                throw new MochaException("Column not found in this name!");

            return (MochaDataType)Enum.Parse(typeof(MochaDataType),
                GetXElement(Doc,$"Tables/{tableName}/{name}").Attribute("DataType").Value);
        }

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
                for(int index = 0;index <dataRange.Count();index++) {
                    dataRange.ElementAt(index).Value = (index + 1).ToString();
                }

                Save();
                return;
            } else if(dataType == MochaDataType.Unique) {
                for(int index = 0;index <dataRange.Count();index++) {
                    dataRange.ElementAt(index).Value = string.Empty;
                }

                Save();
                return;
            }

            for(int index = 0;index < dataRange.Count();index++) {
                if(!MochaData.IsType(dataType,dataRange.ElementAt(index).Value))
                    dataRange.ElementAt(index).Value = MochaData.TryGetData(dataType,dataRange.ElementAt(index).Value).ToString();
            }

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

        #endregion

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

            var dex = GetDataCount(tableName,columnRange.First().Name.LocalName);
            InternalAddData(tableName,columnRange.First().Name.LocalName,row.Datas[0]);
            for(int index = 1;index < columnRange.Count();index++) {
                var columnElement = columnRange.ElementAt(index);
                MochaDataType dataType =
                    GetColumnDataType(tableName,columnElement.Name.LocalName);
                if(dataType == MochaDataType.AutoInt)
                    continue;

                InternalUpdateData(tableName,columnElement.Name.LocalName,dex,row.Datas[index].Data);
            }
            Save();
        }

        /// <summary>
        /// Add row in table.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="datas">Datas of row.</param>
        public void AddRow(string tableName,params object[] datas) {
            AddRow(tableName,new MochaRow(datas));
        }

        /// <summary>
        /// Remove row from table by index.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="index">Index of row to remove.</param>
        public bool RemoveRow(string tableName,int index) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            IEnumerable<XElement> columnRange = GetXElement(CDoc,$"Tables/{tableName}").Elements();
            if(columnRange.First().Elements().Count()-1 >= index) {
                OnChanging(this,new EventArgs());
                for(int columnIndex = 0;columnIndex < columnRange.Count();columnIndex++) {
                    columnRange.ElementAt(columnIndex).Elements().
                        ElementAt(index).Remove();
                }
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

            MochaColumn[] columns = GetColumns(tableName);

            if(columns.Length == 0)
                return null;

            int rowCount = (MochaResult<int>)Query.GetRun($"ROWCOUNT:{tableName}");
            if(rowCount-1 < index)
                throw new MochaException("Index cat not bigger than row count!");

            MochaRow row = new MochaRow();
            MochaData[] datas = new MochaData[columns.Length];

            for(int columnIndex = 0;columnIndex < columns.Length;columnIndex++) {
                datas[columnIndex] = columns[columnIndex].Datas[index];
            }

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
            for(int index = 0;index < dataCount;index++) {
                rows[index] = GetRow(tableName,index);
            }

            return rows;
        }

        /// <summary>
        /// Clear all rows of table.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public void ClearRows(string tableName) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            IEnumerable<XElement> columnRange = GetXElement(CDoc,$"Tables/{tableName}").Elements();
            var count = columnRange.First().Elements().Count();
            if(count > 0) {
                OnChanging(this,new EventArgs());
                for(int columnIndex = 0;columnIndex < columnRange.Count();columnIndex++)
                    columnRange.ElementAt(columnIndex).RemoveNodes();
                Save();
            }
        }

        #endregion

        #region Internal Data

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
            if(dataType == MochaDataType.AutoInt) {
                xData.Value = (1 + GetColumnAutoIntState(tableName,columnName)).ToString();
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.Data.ToString())) {
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

            IEnumerable<XElement> columnRange = CDoc.Root.Element("Tables").Element(tableName).Elements();
            for(int columnIndex = 0;columnIndex < columnRange.Count();columnIndex++) {
                XElement element = columnRange.ElementAt(columnIndex);

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

            data = data == null ? "" : data;
            MochaDataType dataType = GetColumnDataType(tableName,columnName);
            if(!MochaData.IsType(dataType,data)) {
                throw new MochaException("The submitted data is not compatible with the targeted data!");
            }
            XElement xColumn = GetXElement(CDoc,$"Tables/{tableName}/{columnName}");

            IEnumerable<XElement> dataRange = xColumn.Elements();
            if(dataRange.Count() - 1 < index)
                throw new MochaException("This index is larger than the maximum number of data in the table!");

            XElement dataElement = dataRange.ElementAt(index);
            if(dataElement.Value==data.ToString())
                return;

            dataElement.Value=data.ToString();
        }

        /// <summary>
        /// Update first data.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">Data to replace.</param>
        internal void InternalUpdateFirstData(string tableName,string columnName,object data) {
            InternalUpdateData(tableName,columnName,0,data);
        }

        /// <summary>
        /// Update last data.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">Data to replace.</param>
        internal void InternalUpdateLastData(string tableName,string columnName,object data) {
            InternalUpdateData(tableName,columnName,
                GetDataCount(tableName,columnName) - 1,data);
        }

        #endregion

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
        public void AddData(string tableName,string columnName,object data) {
            AddData(tableName,columnName,new MochaData(GetColumnDataType(tableName,columnName),data));
        }

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
            if(dataType == MochaDataType.AutoInt) {
                throw new MochaException("The data type of this column is AutoInt, so data update cannot be done!");
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.ToString())) {
                var dex = GetDataIndex(tableName,columnName,data.ToString());
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
        public void UpdateFirstData(string tableName,string columnName,object data) {
            UpdateData(tableName,columnName,0,data);
        }

        /// <summary>
        /// Update last data.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">Data to replace.</param>
        public void UpdateLastData(string tableName,string columnName,object data) {
            UpdateData(tableName,columnName,
                GetDataCount(tableName,columnName) - 1,data);
        }

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
            IEnumerable<XElement> dataRange = GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements();
            for(int index = 0;index < dataRange.Count();index++) {
                if(dataRange.ElementAt(index).Value == stringData)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether there is a data with the specified.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">Data to check.</param>
        public bool ExistsData(string tableName,string columnName,object data) {
            return ExistsData(tableName,columnName,new MochaData() { data = data });
        }

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

            IEnumerable<XElement> dataRange = GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements();
            for(int index = 0;index < dataRange.Count();index++) {
                if(dataRange.ElementAt(index).Value == stringData)
                    return index;
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

            return new MochaData() { dataType = dataType,data = dataRange.ElementAt(index).Value };
        }

        /// <summary>
        /// Returns all datas in column in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public MochaData[] GetDatas(string tableName,string columnName) {
            if(!ExistsColumn(tableName,columnName))
                throw new MochaException("Column not found in this name!");
            IEnumerable<XElement> dataRange = GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements();
            MochaData[] datas = new MochaData[dataRange.Count()];
            for(int index = 0;index < datas.Length;index++) {
                datas[index] = GetData(tableName,columnName,index);
            }
            return datas;
        }

        /// <summary>
        /// Returns data count of table's column.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public int GetDataCount(string tableName,string columnName) {
            if(!ExistsColumn(tableName,columnName))
                throw new MochaException("Column not found in this name!");

            return GetXElement(Doc,$"Tables/{tableName}/{columnName}").Elements().Count();
        }

        #endregion

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
        public MochaLog[] GetLogs() {
            IEnumerable<XElement> elements = GetXElement(Doc,"Logs").Elements();
            MochaLog[] logs = new MochaLog[elements.Count()];
            for(int index = 0;index < logs.Length;index++) {
                XElement currentElement = elements.ElementAt(index);
                MochaLog log = new MochaLog();
                log.ID = currentElement.Attribute("ID").Value;
                log.Time = DateTime.Parse(currentElement.Attribute("Time").Value);
                log.Log=currentElement.Value;
                logs[index] = log;
            }
            return logs;
        }

        /// <summary>
        /// Real all logs.
        /// </summary>
        public MochaReader<MochaLog> ReadLogs() =>
            new MochaReader<MochaLog>(GetLogs());

        /// <summary>
        /// Restore database to last keeped log.
        /// </summary>
        public void RestoreToLastLog() {
            var logs = GetXElement(Doc,"Logs").Elements();
            if(logs.Count() == 0)
                throw new MochaException("Not exists any log!");
            RestoreToLog(logs.Last().Attribute("ID").Value);
        }

        /// <summary>
        /// Restore database to first keeped log.
        /// </summary>
        public void RestoreToFirstLog() {
            var logs = GetXElement(Doc,"Logs").Elements();
            if(logs.Count() == 0)
                throw new MochaException("Not exists any log!");
            RestoreToLog(logs.First().Attribute("ID").Value);
        }

        /// <summary>
        /// Restore database to log by id.
        /// </summary>
        /// <param name="id">ID of log to restore.</param>
        public void RestoreToLog(string id) {
            if(!ExistsLog(id))
                throw new MochaException("Log not found in this id!");
            Changing?.Invoke(this,new EventArgs());

            var log = GetXElement(Doc,"Logs").Elements().Where(x => x.Attribute("ID").Value==id).First();
            Doc=XDocument.Parse(aes.Decrypt(Iv,Key,log.Value));
            Save();
        }

        /// <summary>
        /// Returns whether there is a log with the specified id.
        /// </summary>
        /// <param name="id">ID of log.</param>
        public bool ExistsLog(string id) =>
            GetXElement(Doc,"Logs").Elements().Where(x => x.Attribute("ID").Value == id).Count() != 0;

        #endregion

        #region Static Properties

        /// <summary>
        /// Version of MochaDB.
        /// </summary>
        public static string Version =>
            Engine_LEXER.Version;

        #endregion

        #region Internal Properties

        /// <summary>
        /// XML Document.
        /// </summary>
        internal XDocument Doc { get; set; }

        /// <summary>
        /// Suspend the changeds events.
        /// </summary>
        internal bool SuspendChangeEvents { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Connection provider.
        /// </summary>
        public MochaProvider Provider { get; private set; }

        /// <summary>
        /// Log keeping status.
        /// </summary>
        public bool Logs { get; }

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

        #endregion
    }
}
