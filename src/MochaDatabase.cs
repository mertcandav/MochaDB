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
using MochaDB.Cryptography;
using MochaDB.engine;
using MochaDB.FileSystem;
using MochaDB.Logging;
using MochaDB.Mochaq;
using MochaDB.Querying;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// MochaDatabase provides management of a MochaDB database.
    /// </summary>
    public class MochaDatabase:IMochaDatabase {
        #region Fields

        private static string
            Iv = "MochaDB#$#3{2533",
            Key = "MochaDBM6YxoFsLXu33FpJdjX0R89xGF";

        private FileStream sourceStream;
        private AES aes256;

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
            aes256=new AES(Iv,Key);
            ConnectionState=MochaConnectionState.Disconnected;
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

        #region Events

        #region Internal

        /// <summary>
        /// This happens before connection check.
        /// </summary>
        internal event EventHandler<EventArgs> ConnectionCheckRequired;
        internal void OnConnectionCheckRequired(object sender,EventArgs e) {
            //Invoke.
            ConnectionCheckRequired?.Invoke(sender,e);

            if(ConnectionState!=MochaConnectionState.Connected)
                throw new MochaException("Connection is not open!");
        }

        #endregion

        /// <summary>
        /// This happens before content changed.
        /// </summary>
        public event EventHandler<EventArgs> Changing;
        internal void OnChanging(object sender,EventArgs e) {
            if(SuspendChangeEvents)
                return;

            //Invoke.
            Changing?.Invoke(sender,e);

            if(Logs)
                KeepLog(false);
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
            aes256=null;
            Provider=null;
        }

        #endregion

        #region Xml

        /// <summary>
        /// Return xml schema of database.
        /// </summary>
        public MochaResult<string> GetXML() =>
            GetXMLDocument().ToString();

        /// <summary>
        /// Return XDocument of database.
        /// </summary>
        public MochaResult<XDocument> GetXMLDocument() {
            OnConnectionCheckRequired(this,new EventArgs());

            XDocument doc = XDocument.Parse(Doc.ToString());
            doc.Root.Element("Root").Remove();
            return doc;
        }

        #endregion

        #region MochaScript

        #region Internal

        /// <summary>
        /// Returns MochaScript build code od stack item.
        /// </summary>
        /// <param name="stackName">Parent stack of StackItem.</param>
        /// <param name="path">Path of StackItem.</param>
        /// <param name="item">StackItem to get code.</param>
        internal string GetBuildStackItem(string stackName,string path,IMochaStackItem item) {
            string code = $"    CreateStackItem:{stackName}:{item.Name}:{path}\n";
            code+=$"    SetStackItemValue:{stackName}:{item.Value}:{(path==string.Empty ? item.Name : path)}\n";
            code+=$"    SetStackItemDescription:{stackName}:{item.Description}:{(path==string.Empty ? item.Name : path)}\n";
            for(int index = 0; index < item.Items.Count; index++) {
                code+=GetBuildStackItem(stackName,$"{(path==string.Empty ? item.Name : $"{path}/{item.Name}")}",item.Items[index]);
            }
            return code;
        }

        #endregion

        /// <summary>
        /// Returns MochaScript build func of sectors.
        /// </summary>
        public string GetBuildFuncSectors() {
            string func = "func BuildSectors()\n{\n";
            IMochaCollectionResult<MochaSector> sectors = GetSectors();
            for(int index = 0; index < sectors.Count; index++) {
                MochaSector currentSector = sectors[index];
                func+=$"    AddSector:{currentSector.Name}:{currentSector.Data}:{currentSector.Description}\n";
            }
            func += "}\n";
            return func;
        }

        /// <summary>
        /// Returns MochaScript build func of stacks.
        /// </summary>
        public string GetBuildFuncStacks() {
            string func = "func BuildStacks()\n{\n";
            IMochaCollectionResult<MochaStack> stacks = GetStacks();
            for(int index = 0; index < stacks.Count; index++) {
                IMochaStack stack = stacks[index];
                func+=$"    CreateStack:{stack.Name}\n";
                func+=$"    SetStackDescription:{stack.Name}:{stack.Description}\n";
                for(int itemIndex = 0; itemIndex< stack.Items.Count; itemIndex++)
                    func+=$"{GetBuildStackItem(stack.Name,string.Empty,stack.Items[itemIndex])}\n";
            }
            func += "}\n";
            return func;
        }

        /// <summary>
        /// Returns MochaScript build func of tables.
        /// </summary>
        public string GetBuildFuncTables() {
            string func = "func BuildTables()\n{\n";
            IMochaCollectionResult<MochaTable> tables = GetTables();
            for(int tableIndex = 0; tableIndex < tables.Count; tableIndex++) {
                MochaTable currentTable = tables[tableIndex];
                func += $"    CreateTable:{currentTable.Name}\n" +
                    $"    SetTableDescription:{currentTable.Name}:{currentTable.Description}\n";
                for(int columnIndex = 0; columnIndex < currentTable.Columns.Count; columnIndex++) {
                    MochaColumn currentColumn = currentTable.Columns[columnIndex];
                    func += $"    CreateColumn:{currentTable.Name}:{currentColumn.Name}\n" +
                        $"    SetColumnDescription:{currentTable.Name}:{currentColumn.Name}:{currentTable.Description}\n" +
                        $"    SetColumnDataType:{currentTable.Name}:{currentColumn.Name}:{currentColumn.DataType}\n";

                    if(currentColumn.DataType==MochaDataType.AutoInt)
                        continue;

                    for(int dataIndex = 0; dataIndex < currentColumn.Datas.Count; dataIndex++) {
                        MochaData currentData = currentColumn.Datas[dataIndex];
                        func+=columnIndex==0 ? $"    AddData:{currentTable.Name}:{currentColumn.Name}:{currentData.Data}\n" :
                            $"    UpdateData:{currentTable.Name}:{currentColumn.Name}:{dataIndex}:{currentData.Data}\n";
                    }
                }
            }

            func += "\n}\n";
            return func;
        }

        /// <summary>
        /// Return MochaScript build code of database.
        /// </summary>
        public string GetMochaScript() {
            string code = $"// Created with MochaDB Engine. Version: {Version}";
            code += $"\n\nProvider {Provider.Path} {Provider.Password}";
            code += "\n\nBegin\n";
            code += "func Main()\n{\n";
            code +=
@"    echo ""Script commands is start.""
    
    // Clear content.
    echo ""Clearing content...""
    ClearSectors
    ClearStacks
    ClearTables
    echo ""Content is cleared.""

    // Sectors.
    echo ""Sectors is building...""
    BuildSectors()
    echo ""Sectors builded successful.""

    // Stacks.
    echo ""Stacks is building...""
    BuildStacks()
    echo ""Stacks builded successful.""

    // Tables.
    echo ""Tables is building...""
    BuildTables()
    echo ""Tables builded successful.""

    echo ""Script commands is end successful.""
}

";
            code +=$"{GetBuildFuncSectors()}\n";
            code +=$"{GetBuildFuncStacks()}\n";
            code +=$"{GetBuildFuncTables()}\n";

            code += "Final";

            return code;
        }

        #endregion

        #region Connection

        /// <summary>
        /// Connect to database.
        /// </summary>
        public void Connect() {
            if(ConnectionState==MochaConnectionState.Connected)
                return;

            ConnectionState=MochaConnectionState.Connected;

            if(!File.Exists(Provider.Path)) {
                if(Provider.GetBoolAttributeState("AutoCreate"))
                    CreateMochaDB(Provider.Path.Substring(0,Provider.Path.Length-8),"","");
                else
                    throw new MochaException("There is no MochaDB database file in the specified path!");
            } else {
                if(!IsMochaDB(Provider.Path))
                    throw new MochaException("The file shown is not a MochaDB database file!");
            }

            Doc = XDocument.Parse(aes256.Decrypt(File.ReadAllText(Provider.Path,Encoding.UTF8)));

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
            FileSystem = new MochaFileSystem(this,true);
        }

        /// <summary>
        /// Disconnect from database.
        /// </summary>
        public void Disconnect() {
            if(ConnectionState==MochaConnectionState.Disconnected)
                return;

            ConnectionState=MochaConnectionState.Disconnected;

            sourceStream.Dispose();
            Doc=null;
            Query=null;
            FileSystem=null;
        }

        #endregion

        #region Static Database

        #region Internal

        /// <summary>
        /// Checks for the presence of the element. Example path: MyTable/MyColumn
        /// </summary>
        /// <param name="path">The MochaDB database file path to check.</param>
        /// <param name="elementPath">Path of element.</param>
        internal static bool ExistsElement(string path,string elementPath) {
            if(!IsMochaDB(path))
                throw new MochaException("The file shown is not a MochaDB database file!");

            string[] elementsName = elementPath.Split('/');

            try {
                XDocument document = XDocument.Parse(new AES(Iv,Key).Decrypt(File.ReadAllText(path)));
                XElement element = document.Root.Element(elementsName[0]);

                if(element.Name.LocalName != elementsName[0])
                    return false;

                for(int i = 1; i < elementsName.Length; i++) {
                    element = element.Element(elementsName[i]);
                    if(element.Name.LocalName != elementsName[i])
                        return false;
                }
                return true;
            } catch(NullReferenceException) { return false; }
        }

        #endregion

        /// <summary>
        /// Returns true if the file in the path is Mocha DB and false otherwise.
        /// </summary>
        /// <param name="path">Path to check.</param>
        public static bool IsMochaDB(string path) {
            if(!File.Exists(path))
                return false;

            FileInfo fInfo = new FileInfo(path);

            if(fInfo.Extension != ".mochadb")
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
            string content = EmptyContent;

            if(!string.IsNullOrEmpty(password)) {
                int dex = content.IndexOf("</Password>");
                content = content.Insert(dex,password);
            }
            if(!string.IsNullOrEmpty(description)) {
                int dex = content.IndexOf("</Description>");
                content = content.Insert(dex,description);
            }

            File.WriteAllText(path.EndsWith(".mochadb") ? path : path + ".mochadb",new AES(Iv,Key).Encrypt(content));
        }

        /// <summary>
        /// Checks the suitability and robustness of the MochaDB database.
        /// </summary>
        /// <param name="path">The file path of the MochaDB database to be checked.</param>
        public static bool CheckMochaDB(string path) {
            if(!IsMochaDB(path))
                throw new MochaException("The file shown is not a MochaDB database file!");

            try {
                XDocument Document = XDocument.Parse(new AES(Iv,Key).Decrypt(File.ReadAllText(path)));
                if(Document.Root.Name.LocalName != "MochaDB")
                    return false;
                else if(!ExistsElement(path,"Root/Password"))
                    return false;
                else if(!ExistsElement(path,"Root/Description"))
                    return false;
                else if(!ExistsElement(path,"Sectors"))
                    return false;
                else if(!ExistsElement(path,"Stacks"))
                    return false;
                else if(!ExistsElement(path,"Tables"))
                    return false;
                else if(!ExistsElement(path,"FileSystem"))
                    return false;
                else if(!ExistsElement(path,"Logs"))
                    return false;
                else
                    return true;
            } catch { return false; }
        }

        #endregion

        #region Database

        #region Internal

        /// <summary>
        /// Checks the suitability and robustness of the MochaDB database.
        /// </summary>
        internal bool CheckMochaDB() {
            try {
                if(Doc.Root.Name.LocalName != "MochaDB")
                    return false;
                else if(!ExistsElement("Root/Password"))
                    return false;
                else if(!ExistsElement("Root/Description"))
                    return false;
                else if(!ExistsElement("Sectors"))
                    return false;
                else if(!ExistsElement("Stacks"))
                    return false;
                else if(!ExistsElement("Tables"))
                    return false;
                else if(!ExistsElement("FileSystem"))
                    return false;
                else if(!ExistsElement("Logs"))
                    return false;
                else
                    return true;
            } catch { return false; }
        }

        /// <summary>
        /// Return element by path.
        /// </summary>
        /// <param name="path">Path of element.</param>
        internal XElement GetXElement(string path) {
            OnConnectionCheckRequired(this,new EventArgs());
            var elementsName = path.Split('/');
            try {
                var element = Doc.Root.Element(elementsName[0]);

                if(element==null)
                    return null;

                for(var i = 1; i < elementsName.Length; i++) {
                    element = element.Element(elementsName[i]);
                    if(element == null)
                        return null;
                }
                return element;
            } catch { return null; }
        }

        /// <summary>
        /// Checks for the presence of the element.
        /// </summary>
        /// <param name="path">Path of element.</param>
        internal bool ExistsElement(string path) =>
            GetXElement(path) != null;

        /// <summary>
        /// Save MochaDB database.
        /// </summary>
        internal void Save() {
            if(Provider.Readonly)
                throw new MochaException("This connection is can read only, cannot task of write!");

            string content = aes256.Encrypt(Doc.ToString());
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
        /// <param name="save">Save after log.</param>
        internal void KeepLog(bool save) {
            string id;
            do {
                id = MochaID.GetID(MochaIDType.Hash16);
            } while(ExistsLog(id));
            XElement xLog = new XElement("Log");
            xLog.Add(new XAttribute("ID",id));
            xLog.Add(new XAttribute("Time",DateTime.Now));
            string content = aes256.Encrypt(Doc.ToString());
            xLog.Value=content;
            XElement xLogs = GetXElement("Logs");
            IEnumerable<XElement> logElements = xLogs.Elements();
            if(logElements.Count() >= 1000)
                logElements.Last().Remove();
            xLogs.Add(xLog);

            if(save)
                Save();
        }

        #endregion

        /// <summary>
        /// Returns the password of the MochaDB database.
        /// </summary>
        public string GetPassword() {
            OnConnectionCheckRequired(this,new EventArgs());

            return GetXElement("Root/Password").Value;
        }

        /// <summary>
        /// Sets the MochaDB Database password.
        /// </summary>
        /// <param name="password">Password to set.</param>
        public void SetPassword(string password) {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement("Root/Password").Value = password;
            Save();
        }

        /// <summary>
        /// Returns the description of the database.
        /// </summary>
        public string GetDescription() {
            OnConnectionCheckRequired(this,new EventArgs());

            return GetXElement("Root/Description").Value;
        }

        /// <summary>
        /// Sets the description of the database.
        /// </summary>
        /// <param name="Description">Description to set.</param>
        public void SetDescription(string Description) {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement("Root/Description").Value = Description;
            Save();
        }

        /// <summary>
        /// Remove all sectors, stacks, tables and others.
        /// </summary>
        public void ClearAll() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement("Sectors").RemoveNodes();
            GetXElement("Stacks").RemoveNodes();
            GetXElement("Tables").RemoveNodes();
            Save();
        }

        /// <summary>
        /// MochaDB checks the existence of the database file and if not creates a new file. ALL DATA IS LOST!
        /// </summary>
        public void Reset() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            Doc = XDocument.Parse(EmptyContent);
            Save();
        }

        /// <summary>
        /// Copy database content from schema.
        /// </summary>
        /// <param name="schema">Database schema to copy.</param>
        public void CopySchema(MochaDatabaseSchema schema) {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            SuspendChangeEvents=true;
            ClearSectors();
            ClearStacks();
            ClearTables();
            for(int index = 0; index < schema.Sectors.Count; index++)
                AddSector(schema.Sectors[index]);
            for(int index = 0; index < schema.Stacks.Count; index++)
                AddStack(schema.Stacks[index]);
            for(int index = 0; index < schema.Tables.Count; index++)
                AddTable(schema.Tables[index]);
            SuspendChangeEvents=false;

            OnChanged(this,new EventArgs());
        }

        /// <summary>
        /// Returns schema of database.
        /// </summary>
        public MochaDatabaseSchema GetSchema() {
            var schema = new MochaDatabaseSchema();
            schema.Sectors.collection.AddRange(GetSectors());
            schema.Stacks.collection.AddRange(GetStacks());
            schema.Tables.collection.AddRange(GetTables());
            return schema;
        }

        /// <summary>
        /// Copy this database to destination database. Returns true is copying is success, returns false if not.
        /// </summary>
        /// <param name="destination">Destination MochaDatabase.</param>
        public bool Copy(MochaDatabase destination) {
            OnConnectionCheckRequired(this,new EventArgs());
            try {
                if(destination == null)
                    return false;
                if(this == destination)
                    return false;
                var descncstate = destination.ConnectionState;

                if(descncstate==MochaConnectionState.Disconnected)
                    destination.Connect();

                var schema = GetSchema();
                destination.CopySchema(schema);

                if(descncstate==MochaConnectionState.Disconnected)
                    destination.Disconnect();

                return true;
            } catch { return false; }
        }

        /// <summary>
        /// Copy database content from destination database. Returns true is copying is success, returns false if not.
        /// </summary>
        /// <param name="destination">Destination MochaDatabase.</param>
        public bool CopyFrom(MochaDatabase destination) {
            try {
                if(destination == null)
                    return false;
                if(this == destination)
                    return false;
                var descncstate = destination.ConnectionState;

                if(descncstate==MochaConnectionState.Disconnected)
                    destination.Connect();

                var schema = destination.GetSchema();
                CopySchema(schema);

                if(descncstate==MochaConnectionState.Disconnected)
                    destination.Disconnect();

                return true;
            } catch { return false; }
        }

        #endregion

        #region Sector

        /// <summary>
        /// Remove all sectors.
        /// </summary>
        public void ClearSectors() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement("Sectors").RemoveNodes();
            Save();
        }

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="sector">MochaSector object to add.</param>
        public void AddSector(MochaSector sector) {
            if(ExistsSector(sector.Name))
                throw new MochaException("There is already a sector with this name!");
            OnChanging(this,new EventArgs());

            XElement xSector = new XElement(sector.Name,sector.Data);
            xSector.Add(new XAttribute("Description",sector.Description));
            GetXElement("Sectors").Add(xSector);

            // Attributes
            for(int index = 0; index < sector.Attributes.Count; index++)
                AddSectorAttribute(sector.Name,sector.Attributes[index]);

            if(sector.Attributes.Count==0)
                Save();
        }

        /// <summary>
        /// Add attribute to sector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="attr">Attribute to add.</param>
        public void AddSectorAttribute(string name,IMochaAttribute attr) {
            if(!ExistsSector(name))
                throw new MochaException("Table not found in this name!");

            var xattr = GetXElement($"Sectors/{name}").Attribute("Attributes");
            if(Engine_ATTRIBUTES.ExistsAttribute(xattr.Value,attr.Name))
                throw new MochaException("There is already a attribute with this name!");

            xattr.Value += Engine_ATTRIBUTES.GetAttributeCode(ref attr);
            Save();
        }

        /// <summary>
        /// Remove attribute from sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="attrname">Name of attribute.</param>
        public bool RemoveSectorAttribute(string name,string attrname) {
            if(!ExistsSector(name))
                return false;

            var xtable = GetXElement($"Sectors/{name}");
            var code = xtable.Attribute("Attributes").Value;
            var copycode = code;
            var result = Engine_ATTRIBUTES.RemoveAttribute(ref code,attrname);

            if(copycode != code) Save();

            return result;
        }

        /// <summary>
        /// Returns attribute from sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="attrname">Name of attribute.</param>
        public IMochaAttribute GetSectorAttribute(string name,string attrname) {
            if(!ExistsSector(name))
                throw new MochaException("Table not found in this name!");

            var xtable = GetXElement($"Sectors/{name}");
            var attr = Engine_ATTRIBUTES.GetAttribute(xtable.Attribute("Attributes").Value,attrname);
            return attr;
        }

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="name">Name of sector to add.</param>
        public void AddSector(string name) =>
            AddSector(new MochaSector(name));

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data of sector.</param>
        public void AddSector(string name,string data) =>
            AddSector(new MochaSector(name,data));

        /// <summary>
        /// Remove sector by name. Returns true if sector is exists and removed.
        /// </summary>
        /// <param name="name">Name of sector to remove.</param>
        public bool RemoveSector(string name) {
            if(!ExistsSector(name))
                return false;
            OnChanging(this,new EventArgs());

            GetXElement($"Sectors/{name}").Remove();
            Save();
            return true;
        }

        /// <summary>
        /// Rename sector.
        /// </summary>
        /// <param name="name">Name of sector to rename.</param>
        /// <param name="newName">New name of sector.</param>
        public void RenameSector(string name,string newName) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");

            if(name == newName)
                return;

            if(ExistsSector(newName))
                throw new MochaException("There is already a sector with this name!");
            OnChanging(this,new EventArgs());

            GetXElement($"Sectors/{name}").Name=newName;
            Save();
        }

        /// <summary>
        /// Returns data of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public string GetSectorData(string name) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");

            return GetXElement($"Sectors/{name}").Value;
        }

        /// <summary>
        /// Set data of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data to set.</param>
        public void SetSectorData(string name,string data) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");
            OnChanging(this,new EventArgs());

            XElement xSector = GetXElement($"Sectors/{name}");
            if(xSector.Value==data)
                return;

            xSector.Value=data;
            Save();
        }

        /// <summary>
        /// Returns description of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public string GetSectorDescription(string name) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");

            return GetXElement($"Sectors/{name}").Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="description">Description to set.</param>
        public void SetSectorDescription(string name,string description) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");
            OnChanging(this,new EventArgs());

            XAttribute xDescription = GetXElement($"Sectors/{name}").Attribute("Description");
            if(xDescription.Value==description)
                return;

            xDescription.Value=description;

            Save();
        }

        /// <summary>
        /// Returns sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public MochaSector GetSector(string name) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");

            XElement xSector = GetXElement($"Sectors/{name}");
            MochaSector sector = new MochaSector(xSector.Name.LocalName);
            sector.Data=xSector.Value;
            sector.Description =xSector.Attribute("Description").Value;
            sector.Attributes.collection.AddRange(GetSectorAttributes(name));

            return sector;
        }

        /// <summary>
        /// Returns all sectors in database.
        /// </summary>
        public MochaCollectionResult<MochaSector> GetSectors() {
            OnConnectionCheckRequired(this,new EventArgs());

            IEnumerable<XElement> sectorRange = GetXElement("Sectors").Elements();
            MochaArray<MochaSector> sectors = new MochaSector[sectorRange.Count()];
            for(int index = 0; index < sectors.Length; index++)
                sectors[index] = GetSector(sectorRange.ElementAt(index).Name.LocalName);

            return new MochaCollectionResult<MochaSector>(sectors);
        }

        /// <summary>
        /// Returns all attributes from sector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public MochaCollectionResult<IMochaAttribute> GetSectorAttributes(string name) {
            if(!ExistsSector(name))
                throw new MochaException("Sector not found in this name!");

            XElement xSector = GetXElement($"Sectors/{name}");
            var attrs = Engine_ATTRIBUTES.GetAttributes(xSector.Attribute("Attributes").Value);

            return new MochaCollectionResult<IMochaAttribute>(attrs);
        }

        /// <summary>
        /// Returns whether there is a sector with the specified name.
        /// </summary>
        /// <param name="name">Name of sector to check.</param>
        public bool ExistsSector(string name) =>
            ExistsElement($"Sectors/{name}");

        #endregion

        #region Stack

        /// <summary>
        /// Remove all stacks.
        /// </summary>
        public void ClearStacks() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement("Stacks").RemoveNodes();
            Save();
        }

        /// <summary>
        /// Add stack.
        /// </summary>
        /// <param name="stack">MohcaStack object to add.</param>
        public void AddStack(MochaStack stack) {
            if(ExistsStack(stack.Name))
                throw new MochaException("There is already a stack with this name!");
            OnChanging(this,new EventArgs());

            XElement xStack = new XElement(stack.Name);
            xStack.Add(new XAttribute("Description",stack.Description));

            // Items
            for(int index = 0; index < stack.Items.Count; index++) {
                xStack.Add(GetMochaStackItemXML(stack.Items[index]));
            }

            // Attributes
            for(int index = 0; index < stack.Attributes.Count; index++)
                AddStackAttribute(stack.Name,stack.Attributes[index]);

            GetXElement("Stacks").Add(xStack);

            if(stack.Items.Count==0 && stack.Attributes.Count == 0)
                Save();
        }

        /// <summary>
        /// Remove stack by name. Returns stack if table is exists and removed.
        /// </summary>
        /// <param name="name">Name of stack to remove.</param>
        public bool RemoveStack(string name) {
            if(!ExistsStack(name))
                return false;
            OnChanging(this,new EventArgs());

            GetXElement($"Stacks/{name}").Remove();
            Save();
            return true;
        }

        /// <summary>
        /// Add attribute to stack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="attr">Attribute to add.</param>
        public void AddStackAttribute(string name,IMochaAttribute attr) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            var xattr = GetXElement($"Stacks/{name}").Attribute("Attributes");
            if(Engine_ATTRIBUTES.ExistsAttribute(xattr.Value,attr.Name))
                throw new MochaException("There is already a attribute with this name!");

            xattr.Value += Engine_ATTRIBUTES.GetAttributeCode(ref attr);
            Save();
        }

        /// <summary>
        /// Remove attribute from stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="attrname">Name of attribute.</param>
        public bool RemoveStackAttribute(string name,string attrname) {
            if(!ExistsStack(name))
                return false;

            var xtable = GetXElement($"Stacks/{name}");
            var code = xtable.Attribute("Attributes").Value;
            var copycode = code;
            var result = Engine_ATTRIBUTES.RemoveAttribute(ref code,attrname);

            if(copycode != code) Save();

            return result;
        }

        /// <summary>
        /// Returns attribute from stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="attrname">Name of attribute.</param>
        public IMochaAttribute GetStackAttribute(string name,string attrname) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            var xtable = GetXElement($"Stacks/{name}");
            var attr = Engine_ATTRIBUTES.GetAttribute(xtable.Attribute("Attributes").Value,attrname);
            return attr;
        }

        /// <summary>
        /// Returns description of stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public string GetStackDescription(string name) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            return GetXElement($"Stacks/{name}").Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="description">Description to set.</param>
        public void SetStackDescription(string name,string description) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");
            OnChanging(this,new EventArgs());

            XAttribute xDescription = GetXElement($"Stacks/{name}").Attribute("Description");
            if(xDescription.Value==description)
                return;

            xDescription.Value=description;

            Save();
        }

        /// <summary>
        /// Rename stack.
        /// </summary>
        /// <param name="name">Name of stack to rename.</param>
        /// <param name="newName">New name of stack.</param>
        public void RenameStack(string name,string newName) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");
            if(ExistsStack(newName))
                throw new MochaException("There is already a stack with this name!");
            OnChanging(this,new EventArgs());

            GetXElement($"Stacks/{name}").Name=newName;
            Save();
        }

        /// <summary>
        /// Returns stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public MochaStack GetStack(string name) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement xStack = GetXElement($"Stacks/{name}");
            MochaStack stack = new MochaStack(xStack.Name.LocalName);
            stack.Description=xStack.Attribute("Description").Value;
            stack.Attributes.collection.AddRange(GetStackAttributes(name));

            IEnumerable<XElement> elementRange = xStack.Elements();
            for(int index = 0; index < elementRange.Count(); index++) {
                stack.Items.collection.Add(GetStackItem(name,elementRange.ElementAt(index).Name.LocalName));
            }

            return stack;
        }

        /// <summary>
        /// Returns all stacks in database.
        /// </summary>
        /// <returns></returns>
        public MochaCollectionResult<MochaStack> GetStacks() {
            OnConnectionCheckRequired(this,new EventArgs());

            IEnumerable<XElement> stackRange = GetXElement("Stacks").Elements();
            MochaArray<MochaStack> stacks = new MochaStack[stackRange.Count()];
            if(stackRange.Count() > 0)
                for(int index = 0; index < stacks.Length; index++) {
                    stacks[index] = GetStack(stackRange.ElementAt(index).Name.LocalName);
                }

            return new MochaCollectionResult<MochaStack>(stacks);
        }

        /// <summary>
        /// Returns all attributes from stack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public MochaCollectionResult<IMochaAttribute> GetStackAttributes(string name) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement xstack = GetXElement($"Stacks/{name}");
            var attrs = Engine_ATTRIBUTES.GetAttributes(xstack.Attribute("Attributes").Value);

            return new MochaCollectionResult<IMochaAttribute>(attrs);
        }

        /// <summary>
        /// Returns whether there is a stack with the specified name.
        /// </summary>
        /// <param name="name">Name of stack to check.</param>
        public bool ExistsStack(string name) =>
            ExistsElement($"Stacks/{name}");

        #endregion

        #region StackItem

        #region Internal

        /// <summary>
        /// Return XElement from MochaStackItem.
        /// </summary>
        /// <param name="item">MochaStackItem object.</param>
        internal XElement GetMochaStackItemXML(MochaStackItem item) {
            XElement element = new XElement(item.Name,item.Value);
            element.Add(new XAttribute("Description",item.Description));
            element.Add(new XAttribute("Attributes",string.Empty) {
                Value = Engine_ATTRIBUTES.BuildCode(item.Attributes)
            });

            for(int index = 0; index < item.Items.Count; index++) {
                element.Add(GetMochaStackItemXML(item.Items[index]));
            }

            return element;
        }

        #endregion

        /// <summary>
        /// Add stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        /// <param name="item">MochaStackItem object to add.</param>
        public void AddStackItem(string name,string path,MochaStackItem item) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");
            OnChanging(this,new EventArgs());

            var element =
                !string.IsNullOrWhiteSpace(path) ?
                    GetXElement($"Stacks/{name}/{path}") : GetXElement($"Stacks/{name}");

            if(element==null)
                throw new MochaException("The road is wrong, there is no such way!");

            element.Add(GetMochaStackItemXML(item));
            Save();
        }

        /// <summary>
        /// Add attribute to stackitem.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        /// <param name="attr">Attribute to add.</param>
        public void AddStackItemAttribute(string name,string path,IMochaAttribute attr) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            var xattr = GetXElement($"Stacks/{name}/{path}").Attribute("Attributes");
            if(Engine_ATTRIBUTES.ExistsAttribute(xattr.Value,attr.Name))
                throw new MochaException("There is already a attribute with this name!");

            xattr.Value += Engine_ATTRIBUTES.GetAttributeCode(ref attr);
            Save();
        }

        /// <summary>
        /// Returns attribute from stackitem by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        /// <param name="attrname">Name of attribute.</param>
        public IMochaAttribute GetStackItemAttribute(string name,string path,string attrname) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            var xtable = GetXElement($"Stacks/{name}/{path}");
            var attr = Engine_ATTRIBUTES.GetAttribute(xtable.Attribute("Attributes").Value,attrname);
            return attr;
        }

        /// <summary>
        /// Remove attribute from stackitem by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        /// <param name="attrname">Name of attribute.</param>
        public bool RemoveStackItemAttribute(string name,string path,string attrname) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            var xitem = GetXElement($"Stacks/{name}/{path}");
            var code = xitem.Attribute("Attributes").Value;
            var copycode = code;
            var result = Engine_ATTRIBUTES.RemoveAttribute(ref code,attrname);

            if(copycode != code) Save();

            return result;
        }

        /// <summary>
        /// Remove item of stack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public bool RemoveStackItem(string name,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");
            OnChanging(this,new EventArgs());

            if(path==string.Empty) {
                GetXElement($"Stacks/{name}").RemoveAll();
                return true;
            } else {
                XElement element = GetXElement($"Stacks/{name}/{path}");
                if(element==null)
                    return false;

                element.Remove();
            }

            Save();
            return true;
        }

        /// <summary>
        /// Set value of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="path">Path of stack item.</param>
        public void SetStackItemValue(string name,string value,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement element = GetXElement($"Stacks/{name}/{path}");

            if(element==null)
                throw new MochaException("The road is wrong, there is no such way!");

            if(element.Value == value)
                return;
            OnChanging(this,new EventArgs());

            element.Value=value;
            Save();
        }

        /// <summary>
        /// Return value of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public string GetStackItemValue(string name,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement element = GetXElement($"Stacks/{name}/{path}");

            if(element==null)
                throw new MochaException("The road is wrong, there is no such way!");

            return element.Value;
        }

        /// <summary>
        /// Set description of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="description">Description to set.</param>
        /// <param name="path">Path of stack item.</param>
        public void SetStackItemDescription(string name,string description,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement element = GetXElement($"Stacks/{name}/{path}");

            if(element==null)
                throw new MochaException("The road is wrong, there is no such way!");

            if(element.Attribute("Description").Value == description)
                return;
            OnChanging(this,new EventArgs());

            element.Attribute("Description").Value=description;
            Save();
        }

        /// <summary>
        /// Return description of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public string GetStackItemDescription(string name,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement element = GetXElement($"Stacks/{name}/{path}");

            if(element==null)
                throw new MochaException("The road is wrong, there is no such way!");

            return element.Attribute("Description").Value;
        }

        /// <summary>
        /// Rename stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="newName">New name of stack item.</param>
        /// <param name="path">Path of stack item.</param>
        public void RenameStackItem(string name,string newName,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement element = GetXElement($"Stacks/{name}/{path}");

            if(element==null)
                throw new MochaException("The road is wrong, there is no such way!");

            if(element.Name.LocalName == newName)
                return;

            if(path.Contains('/') && ExistsStackItem(name,$"{path.Substring(0,path.IndexOf("/"))}/{newName}"))
                throw new MochaException("There is already a stack item with this name!");
            OnChanging(this,new EventArgs());

            element.Name=newName;
            Save();
        }

        /// <summary>
        /// Return StackItem.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public MochaStackItem GetStackItem(string name,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement xStackItem = GetXElement($"Stacks/{name}/{path}");

            MochaStackItem item = new MochaStackItem(xStackItem.Name.LocalName);
            item.Value=xStackItem.Value;
            item.Description=xStackItem.Attribute("Description").Value;
            item.Attributes.collection.AddRange(GetStackItemAttributes(name,path));

            IEnumerable<XElement> elementRange = xStackItem.Elements();
            if(elementRange.Count() >0)
                for(int index = 0; index < elementRange.Count(); index++)
                    item.Items.collection.Add(GetStackItem(name,path += $"/{elementRange.ElementAt(index).Name.LocalName}"));

            return item;
        }

        /// <summary>
        /// Returns all attributes from stackitem.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public MochaCollectionResult<IMochaAttribute> GetStackItemAttributes(string name,string path) {
            if(!ExistsStack(name))
                throw new MochaException("Stack not found in this name!");

            XElement xstackitem = GetXElement($"Stacks/{name}/{path}");
            var attrs = Engine_ATTRIBUTES.GetAttributes(xstackitem.Attribute("Attributes").Value);

            return new MochaCollectionResult<IMochaAttribute>(attrs);
        }

        /// <summary>
        /// Returns whether there is a stack item with the specified name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Path of stack item.</param>
        public bool ExistsStackItem(string name,string path) =>
            ExistsElement($"Stacks/{name}/{path}");

        #endregion

        #region Table

        /// <summary>
        /// Remove all tables.
        /// </summary>
        public void ClearTables() {
            OnConnectionCheckRequired(this,new EventArgs());
            OnChanging(this,new EventArgs());

            GetXElement("Tables").RemoveNodes();
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
            GetXElement("Tables").Add(xTable);

            // Columns.
            for(int index = 0; index < table.Columns.Count; index++) {
                var column = table.Columns[index];
                XElement Xcolumn = new XElement(column.Name);
                Xcolumn.Add(new XAttribute("DataType",column.DataType));
                Xcolumn.Add(new XAttribute("Description",column.Description));
                for(int dindex = 0; dindex < column.Datas.Count; dindex++)
                    Xcolumn.Add(new XElement("Data",column.Datas[dindex].Data));
                xTable.Add(Xcolumn);
            }

            // Attributes
            for(int index = 0; index < table.Attributes.Count; index++)
                AddTableAttribute(table.Name,table.Attributes[index]);

            if(table.Columns.Count==0 && table.Attributes.Count==0)
                Save();
        }

        /// <summary>
        /// Add attribute to table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="attr">Attribute to add.</param>
        public void AddTableAttribute(string name,IMochaAttribute attr) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");
            var xattr = GetXElement($"Tables/{name}").Attribute("Attributes");
            if(Engine_ATTRIBUTES.ExistsAttribute(xattr.Value,attr.Name))
                throw new MochaException("There is already a attribute with this name!");

            xattr.Value += Engine_ATTRIBUTES.GetAttributeCode(ref attr);
            Save();
        }

        /// <summary>
        /// Remove attribute from table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="attrname">Name of attribute.</param>
        public bool RemoveTableAttribute(string name,string attrname) {
            if(!ExistsTable(name))
                return false;

            var xtable = GetXElement($"Tables/{name}");
            var code = xtable.Attribute("Attributes").Value;
            var copycode = code;
            var result = Engine_ATTRIBUTES.RemoveAttribute(ref code,attrname);

            if(copycode != code) Save();

            return result;
        }

        /// <summary>
        /// Returns attribute from table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="attrname">Name of attribute.</param>
        public IMochaAttribute GetTableAttribute(string name,string attrname) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");

            var xtable = GetXElement($"Tables/{name}");
            var attr = Engine_ATTRIBUTES.GetAttribute(xtable.Attribute("Attributes").Value,attrname);
            return attr;
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

            GetXElement($"Tables/{name}").Remove();
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
            OnChanging(this,new EventArgs());

            GetXElement($"Tables/{name}").Name=newName;
            Save();
        }

        /// <summary>
        /// Returns description of table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public string GetTableDescription(string name) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");

            return GetXElement($"Tables/{name}").Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="description">Description to set.</param>
        public void SetTableDescription(string name,string description) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");

            XAttribute xDescription = GetXElement($"Tables/{name}").Attribute("Description");
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

            XElement xTable = GetXElement($"Tables/{name}");
            MochaTable table = new MochaTable(name);
            table.Description=xTable.Attribute("Description").Value;
            table.Attributes.collection.AddRange(GetTableAttributes(name));

            table.Columns.collection.AddRange(GetColumns(name));
            table.Rows.collection.AddRange(GetRows(name));

            return table;
        }

        /// <summary>
        /// Returns all tables in database.
        /// </summary>
        public MochaCollectionResult<MochaTable> GetTables() {
            OnConnectionCheckRequired(this,new EventArgs());

            IEnumerable<XElement> tableRange = GetXElement("Tables").Elements();
            MochaArray<MochaTable> tables = new MochaTable[tableRange.Count()];
            for(int index = 0; index <tables.Length; index++) {
                tables[index] = GetTable(tableRange.ElementAt(index).Name.LocalName);
            }

            return new MochaCollectionResult<MochaTable>(tables);
        }

        /// <summary>
        /// Returns all attributes from table.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public MochaCollectionResult<IMochaAttribute> GetTableAttributes(string name) {
            if(!ExistsTable(name))
                throw new MochaException("Table not found in this name!");

            XElement xtable = GetXElement($"Tables/{name}");
            var attrs = Engine_ATTRIBUTES.GetAttributes(xtable.Attribute("Attributes").Value);

            return new MochaCollectionResult<IMochaAttribute>(attrs);
        }

        /// <summary>
        /// Returns whether there is a table with the specified name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public bool ExistsTable(string name) =>
            ExistsElement($"Tables/{name}");

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
            GetXElement($"Tables/{tableName}").Add(xColumn);

            // Datas.
            int rowCount = (MochaResult<int>)Query.GetRun($"ROWCOUNT:{tableName}");
            if(column.DataType==MochaDataType.AutoInt) {
                for(int index = 1; index <= rowCount; index++)
                    xColumn.Add(new XElement("Data",index));
            } else {
                for(int index = 0; index < column.Datas.Count; index++)
                    xColumn.Add(new XElement("Data",column.Datas[index].Data));

                for(int index = column.Datas.Count; index < rowCount; index++) {
                    xColumn.Add(new XElement("Data",MochaData.TryGetData(column.DataType,"")));
                }
            }

            // Attributes
            for(int index = 0; index < column.Attributes.Count; index++)
                AddColumnAttribute(tableName,column.Name,column.Attributes[index]);

            if(column.Attributes.Count==0)
                Save();
        }

        /// <summary>
        /// Add attribute to column.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="attr">Attribute to add.</param>
        public void AddColumnAttribute(string tableName,string name,IMochaAttribute attr) {
            if(!ExistsColumn(tableName,name))
                throw new MochaException("Column not found in this name!");

            var xattr = GetXElement($"Tables/{tableName}/{name}").Attribute("Attributes");
            if(Engine_ATTRIBUTES.ExistsAttribute(xattr.Value,attr.Name))
                throw new MochaException("There is already a attribute with this name!");

            xattr.Value += Engine_ATTRIBUTES.GetAttributeCode(ref attr);
            Save();
        }

        /// <summary>
        /// Returns attribute from column by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="attrname">Name of attribute.</param>
        public IMochaAttribute GetColumnAttribute(string tableName,string name,string attrname) {
            if(!ExistsColumn(tableName,name))
                throw new MochaException("Column not found in this name!");

            var xtable = GetXElement($"Tables/{tableName}/{name}");
            var attr = Engine_ATTRIBUTES.GetAttribute(xtable.Attribute("Attributes").Value,attrname);
            return attr;
        }

        /// <summary>
        /// Remove attribute from column by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="attrname">Name of attribute.</param>
        public bool RemoveColumnAttribute(string tableName,string name,string attrname) {
            if(ExistsColumn(tableName,name))
                return false;

            var xcolumn = GetXElement($"Tables/{tableName}/{name}");
            var code = xcolumn.Attribute("Attributes").Value;
            var copycode = code;
            var result = Engine_ATTRIBUTES.RemoveAttribute(ref code,attrname);

            if(copycode != code) Save();

            return result;
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

            GetXElement($"Tables/{tableName}/{name}").Remove();
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
            OnChanging(this,new EventArgs());

            GetXElement($"Tables/{tableName}/{name}").Name=newName;
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

            return GetXElement($"Tables/{tableName}/{name}").Attribute("Description").Value;
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

            XAttribute xDescription = GetXElement($"Tables/{tableName}/{name}").Attribute("Description");
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
            column.Description = GetColumnDescription(tableName,name);
            column.Attributes.collection.AddRange(GetColumnAttributes(tableName,name));
            column.Datas.collection.AddRange(GetDatas(tableName,name));

            return column;
        }

        /// <summary>
        /// Returns all columns in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public MochaCollectionResult<MochaColumn> GetColumns(string tableName) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            IEnumerable<XElement> columnsRange = GetXElement($"Tables/{tableName}").Elements();
            MochaArray<MochaColumn> columns = new MochaColumn[columnsRange.Count()];
            for(int index = 0; index < columns.Length; index++) {
                columns[index] = GetColumn(tableName,columnsRange.ElementAt(index).Name.LocalName);
            }

            return new MochaCollectionResult<MochaColumn>(columns);
        }

        /// <summary>
        /// Returns whether there is a column with the specified name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public bool ExistsColumn(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            return GetXElement($"Tables/{tableName}/{name}") != null;
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
                GetXElement($"Tables/{tableName}/{name}").Attribute("DataType").Value);
        }

        /// <summary>
        /// Returns all attributes from column.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public MochaCollectionResult<IMochaAttribute> GetColumnAttributes(string tableName,string name) {
            if(!ExistsColumn(tableName,name))
                throw new MochaException("Column not found in this name!");

            var xcolumn = GetXElement($"Tables/{tableName}/{name}");
            var attrs = Engine_ATTRIBUTES.GetAttributes(xcolumn.Attribute("Attributes").Value);

            return new MochaCollectionResult<IMochaAttribute>(attrs);
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

            XElement xColumn = GetXElement($"Tables/{tableName}/{name}");
            if(xColumn.Attribute("DataType").Value==dataType.ToString())
                return;

            xColumn.Attribute("DataType").Value = dataType.ToString();

            IEnumerable<XElement> dataRange = xColumn.Elements();
            if(dataType == MochaDataType.AutoInt) {
                for(int index = 0; index <dataRange.Count(); index++) {
                    dataRange.ElementAt(index).Value = (index + 1).ToString();
                }

                Save();
                return;
            } else if(dataType == MochaDataType.Unique) {
                for(int index = 0; index <dataRange.Count(); index++) {
                    dataRange.ElementAt(index).Value = string.Empty;
                }

                Save();
                return;
            }

            for(int index = 0; index < dataRange.Count(); index++) {
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

            XElement lastData = (XElement)GetXElement($"Tables/{tableName}/{name}").LastNode;

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

            IEnumerable<XElement> columnRange = GetXElement($"Tables/{tableName}").Elements();

            if(columnRange.Count() != row.Datas.Count)
                throw new MochaException("The data count of the row is not equal to the number of columns!");

            for(int index = 0; index <columnRange.Count(); index++) {
                InternalAddData(tableName,columnRange.ElementAt(index).Name.LocalName,row.Datas[index]);
            }
        }

        /// <summary>
        /// Remove row from table by index.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="index">Index of row to remove.</param>
        public bool RemoveRow(string tableName,int index) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            IEnumerable<XElement> columnRange = GetXElement($"Tables/{tableName}").Elements();
            for(int columnIndex = 0; columnIndex < columnRange.Count(); columnIndex++) {
                IEnumerable<XElement> dataRange = columnRange.ElementAt(columnIndex).Elements();
                for(int dataIndex = 0; dataIndex < dataRange.Count(); dataIndex++) {
                    if(dataIndex == index) {
                        OnChanging(this,new EventArgs());
                        dataRange.ElementAt(dataIndex).Remove();
                        return true;
                    }
                }
            }

            Save();
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

            MochaCollectionResult<MochaColumn> columns = GetColumns(tableName);

            if(columns.Count == 0)
                return null;

            int rowCount = (MochaResult<int>)Query.GetRun($"ROWCOUNT:{tableName}");
            if(rowCount-1 < index)
                throw new MochaException("Index cat not bigger than row count!");

            MochaRow row = new MochaRow();
            MochaArray<MochaData> datas = new MochaData[columns.Count];

            for(int columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
                datas[columnIndex] = columns[columnIndex].Datas[index];
            }

            row.Datas.collection.AddRange(datas);

            return row;
        }

        /// <summary>
        /// Returns all rows in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public MochaCollectionResult<MochaRow> GetRows(string tableName) {
            if(!ExistsTable(tableName))
                throw new MochaException("Table not found in this name!");

            XElement firstColumn = (XElement)GetXElement($"Tables/{tableName}").FirstNode;

            if(firstColumn==null)
                return new MochaCollectionResult<MochaRow>();

            int dataCount = GetDataCount(tableName,firstColumn.Name.LocalName);
            MochaArray<MochaRow> rows = new MochaRow[dataCount];
            for(int index = 0; index < dataCount; index++) {
                rows[index] = GetRow(tableName,index);
            }

            return new MochaCollectionResult<MochaRow>(rows);
        }

        #endregion

        #region Data

        #region Internal

        /// <summary>
        /// Add data.
        /// </summary>
        internal void InternalAddData(string tableName,string columnName,MochaData data) {
            if(!ExistsColumn(tableName,columnName))
                throw new MochaException("Column not found in this name!");

            XElement xData = new XElement("Data",data.Data); ;

            MochaDataType dataType = GetColumnDataType(tableName,columnName);
            if(dataType == MochaDataType.AutoInt) {
                xData.Value = (1 + GetColumnAutoIntState(tableName,columnName)).ToString();
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.Data.ToString())) {
                if(ExistsData(tableName,columnName,data))
                    throw new MochaException("Any value can be added to a unique column only once!");
            }

            if(!MochaData.IsType(dataType,data.Data))
                throw new MochaException("The submitted data is not compatible with the targeted data!");

            IEnumerable<XElement> columnRange = Doc.Root.Element("Tables").Element(tableName).Elements();
            for(int columnIndex = 0; columnIndex < columnRange.Count(); columnIndex++) {
                XElement element = columnRange.ElementAt(columnIndex);

                if(element.Name.LocalName==columnName)
                    continue;

                MochaDataType _dataType = GetColumnDataType(tableName,element.Name.LocalName);
                if(_dataType == MochaDataType.AutoInt) {
                    element.Add(
                        new XElement("Data",1 + GetColumnAutoIntState(tableName,element.Name.LocalName),string.Empty));
                    continue;
                }

                element.Add(
                    new XElement("Data",MochaData.TryGetData(GetColumnDataType(tableName,element.Name.LocalName),string.Empty)));
            }

            GetXElement($"Tables/{tableName}/{columnName}").Add(xData);

            Save();
        }

        #endregion

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">MochaData object to add.</param>
        public void AddData(string tableName,string columnName,MochaData data) {
            OnChanging(this,new EventArgs());
            InternalAddData(tableName,columnName,data);
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
        /// <param name="data">Data to replace.</param>
        /// <param name="index">Index of data.</param>
        public void UpdateData(string tableName,string columnName,int index,object data) {
            if(!ExistsColumn(tableName,columnName))
                throw new MochaException("Column not found in this name!");
            OnChanging(this,new EventArgs());

            data = data == null ? "" : data;
            XElement xColumn = GetXElement($"Tables/{tableName}/{columnName}");

            MochaDataType dataType = GetColumnDataType(tableName,columnName);
            if(dataType == MochaDataType.AutoInt) {
                throw new MochaException("The data type of this column is AutoInt, so data update cannot be done!");
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.ToString())) {
                if(ExistsData(tableName,columnName,data.ToString()))
                    throw new MochaException("Any value can be added to a unique column only once!");
            } else if(!MochaData.IsType(dataType,data)) {
                throw new MochaException("The submitted data is not compatible with the targeted data!");
            }

            IEnumerable<XElement> dataRange = xColumn.Elements();
            if(dataRange.Count() - 1 < index)
                throw new MochaException("This index is larger than the maximum number of data in the table!");

            XElement dataElement = dataRange.ElementAt(index);
            if(dataElement.Value==data.ToString())
                return;

            dataElement.Value=data.ToString();

            Save();
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
            IEnumerable<XElement> dataRange = GetXElement($"Tables/{tableName}/{columnName}").Elements();
            for(int index = 0; index < dataRange.Count(); index++) {
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

            IEnumerable<XElement> dataRange = GetXElement($"Tables/{tableName}/{columnName}").Elements();
            for(int index = 0; index < dataRange.Count(); index++) {
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

            IEnumerable<XElement> dataRange = GetXElement($"Tables/{tableName}/{columnName}").Elements();
            if(dataRange.Count() - 1 < index)
                throw new MochaException("This index is larger than the maximum number of data in the table!");

            return new MochaData() { dataType = dataType,data = dataRange.ElementAt(index).Value };
        }

        /// <summary>
        /// Returns all datas in column in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public MochaCollectionResult<MochaData> GetDatas(string tableName,string columnName) {
            if(!ExistsColumn(tableName,columnName))
                throw new MochaException("Column not found in this name!");
            IEnumerable<XElement> dataRange = GetXElement($"Tables/{tableName}/{columnName}").Elements();
            MochaArray<MochaData> datas = new MochaData[dataRange.Count()];
            for(int index = 0; index < datas.Length; index++) {
                datas[index] = GetData(tableName,columnName,index);
            }
            return new MochaCollectionResult<MochaData>(datas);
        }

        /// <summary>
        /// Returns data count of table's column.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public int GetDataCount(string tableName,string columnName) {
            if(!ExistsColumn(tableName,columnName))
                throw new MochaException("Column not found in this name!");

            return GetXElement($"Tables/{tableName}/{columnName}").Elements().Count();
        }

        #endregion

        #region Log

        /// <summary>
        /// Clear all logs.
        /// </summary>
        public void ClearLogs() {
            OnChanging(this,new EventArgs());

            GetXElement("Logs").RemoveNodes();
            Save();
        }

        /// <summary>
        /// Returns all logs.
        /// </summary>
        public MochaCollectionResult<MochaLog> GetLogs() {
            IEnumerable<XElement> elements = GetXElement("Logs").Elements();
            MochaArray<MochaLog> logs = new MochaLog[elements.Count()];
            for(int index = 0; index < logs.Length; index++) {
                XElement currentElement = elements.ElementAt(index);
                MochaLog log = new MochaLog();
                log.ID = currentElement.Attribute("ID").Value;
                log.Time = DateTime.Parse(currentElement.Attribute("Time").Value);
                log.Log=currentElement.Value;
                logs[index] = log;
            }
            return new MochaCollectionResult<MochaLog>(logs);
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
            var logs = GetXElement("Logs").Elements();
            if(logs.Count() == 0)
                throw new MochaException("Not exists any log!");
            RestoreToLog(logs.Last().Attribute("ID").Value);
        }

        /// <summary>
        /// Restore database to first keeped log.
        /// </summary>
        public void RestoreToFirstLog() {
            var logs = GetXElement("Logs").Elements();
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

            var log = GetXElement("Logs").Elements().Where(x => x.Attribute("ID").Value==id).First();
            Doc=XDocument.Parse(aes256.Decrypt(log.Value));
            Save();
        }

        /// <summary>
        /// Returns whether there is a log with the specified id.
        /// </summary>
        /// <param name="id">ID of log.</param>
        public bool ExistsLog(string id) =>
            GetXElement("Logs").Elements().Where(x => x.Attribute("ID").Value == id).Count() != 0;

        #endregion

        #region Element

        /// <summary>
        /// Returns whether there is a path with the specified path.
        /// </summary>
        /// <param name="path">Path to check.</param>
        public bool ExistsElement(MochaPath path) =>
            ExistsElement(path.Path);

        /// <summary>
        /// Return element by path.
        /// </summary>
        /// <param name="path">Path of element.</param>
        public MochaElement GetElement(MochaPath path) {
            if(!path.IsDatabasePath())
                throw new MochaException("This path is not database compatible path!");
            if(!ExistsElement(path.Path))
                throw new MochaException("Element is not found!");
            var element = GetXElement(path.Path);
            var melement = new MochaElement {
                Name = element.Name.LocalName,
                Description = element.Attribute("Description") == null ?
                    string.Empty :
                    element.Attribute("Description").Value,
                Value = element.Value
            };
            return melement;
        }

        /// <summary>
        /// Returns sub elements of element in path.
        /// </summary>
        /// <param name="path">Path of base element.</param>
        public MochaCollectionResult<MochaElement> GetElements(MochaPath path) {
            if(!path.IsDatabasePath())
                throw new MochaException("This path is not database compatible path!");
            if(!ExistsElement(path.Path))
                throw new MochaException("Element is not found!");
            var elements = GetXElement(path.Path).Elements();
            MochaArray<MochaElement> array = new MochaElement[elements.Count()];
            for(int index = 0; index < array.Length; index++)
                array[index] = GetElement($"{path.Path}/{elements.ElementAt(index).Name.LocalName}");
            return new MochaCollectionResult<MochaElement>(array);
        }

        #endregion

        #region DatabaseItem

        /// <summary>
        /// Remove database item by type.
        /// </summary>
        /// <param name="item">DatabaseItem object.</param>
        public bool RemoveDatabaseItem(IMochaDatabaseItem item) {
            var type = item.GetType();
            if(type == typeof(MochaTable))
                return RemoveTable(item.Name);
            else if(type == typeof(MochaSector))
                return RemoveSector(item.Name);
            else if(type == typeof(MochaStack))
                return RemoveStack(item.Name);
            else
                return false;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns result of <see cref="GetMochaScript()"/>
        /// </summary>
        public override string ToString() {
            return GetMochaScript();
        }

        #endregion

        #region Properties

        #region Internal

        /// <summary>
        /// XML Document.
        /// </summary>
        internal XDocument Doc { get; set; }

        /// <summary>
        /// Suspend the changeds events.
        /// </summary>
        internal bool SuspendChangeEvents { get; set; }

        /// <summary>
        /// The most basic content of the database.
        /// </summary>
        internal static string EmptyContent =>
$@"<?MochaDB Version=\""{Version}""?>
<MochaDB Description=""Root element of database."">>
    <Root Description=""Root of database."">>
        <Password DataType=""String"" Description=""Password of database.""></Password>
        <Description DataType=""String"" Description=""Description of database.""></Description>
    </Root>
    <Sectors Description=""Base of sectors."">>
    </Sectors>
    <Stacks Description=""Base of stacks."">>
    </Stacks>
    <Tables Description=""Base of tables."">
    </Tables>
    <FileSystem Description=""FileSystem of database."">
        <C Type=""Disk"" Name=""Default"" Description=""Default disk.""></C>
    </FileSystem>
    <Logs Description=""Logs of database."">
    </Logs>
</MochaDB>";


        #endregion

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
        /// Mapped MochaFileSystem.
        /// </summary>
        public MochaFileSystem FileSystem { get; private set; }

        /// <summary>
        /// State of connection.
        /// </summary>
        public MochaConnectionState ConnectionState { get; private set; }

        /// <summary>
        /// Name of database.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Version of MochaDB.
        /// </summary>
        public static string Version =>
            "3.4.3";

        #endregion
    }
}
