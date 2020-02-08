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
using System.Text;
using System.Linq;
using System.Xml.Linq;
using MochaDB.Encryptors;

namespace MochaDB {
    /// <summary>
    /// MochaDatabase provides management of a MochaDB database.
    /// </summary>
    [Serializable]
    public sealed class MochaDatabase:IDisposable {
        #region Fields

        FileStream sourceStream;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MochaDatabase. If there is no MochaDB database file on the path, it will be created automatically.
        /// </summary>
        /// <param name="path">Directory path of MochaDB database.</param>
        public MochaDatabase(string path) {
            if(!IsMochaDB(path))
                throw new Exception("The file shown is not a MochaDB database file!");

            DBPath = path;

            Doc = XDocument.Parse(AES256.Decrypt(File.ReadAllText(DBPath,Encoding.UTF8)));

            if(!CheckMochaDB())
                throw new Exception("The MochaDB database is corrupt!");
            if(!string.IsNullOrEmpty(GetPassword()))
                throw new Exception("The MochaDB database is password protected!");

            FileInfo fInfo = new FileInfo(path);

            Name = fInfo.Name.Substring(0,fInfo.Name.Length - fInfo.Extension.Length);

            Query = new MochaQuery(this,true);
            sourceStream = File.Open(path,FileMode.Open,FileAccess.ReadWrite);
        }

        /// <summary>
        /// Creates a new MochaDatabase. If there is no MochaDB database file on the path, it will be created automatically.
        /// </summary>
        /// <param name="path">Directory path of MochaDB database.</param>
        /// <param name="password">Password of MochaDB database.</param>
        public MochaDatabase(string path,string password) {
            if(!IsMochaDB(path))
                throw new Exception("The file shown is not a MochaDB database file!");

            DBPath = path;

            Doc = XDocument.Parse(AES256.Decrypt(File.ReadAllText(DBPath,Encoding.UTF8)));

            if(!CheckMochaDB())
                throw new Exception("The MochaDB database is corrupt!");
            if(GetPassword() != password)
                throw new Exception("MochaDB database password does not match the password specified!");

            FileInfo fInfo = new FileInfo(path);

            Name = fInfo.Name.Substring(0,fInfo.Name.Length - fInfo.Extension.Length);

            Query = new MochaQuery(this,true);
            sourceStream = File.Open(path,FileMode.Open,FileAccess.ReadWrite);
        }

        #endregion

        #region Static Methods

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

            File.WriteAllText(path + ".mochadb",AES256.Encrypt(content));
        }

        /// <summary>
        /// Checks the suitability and robustness of the MochaDB database.
        /// </summary>
        /// <param name="path">The file path of the MochaDB database to be checked.</param>
        public static bool CheckMochaDB(string path) {
            if(!IsMochaDB(path))
                throw new Exception("The file shown is not a MochaDB database file!");

            try {
                XDocument Document = XDocument.Parse(AES256.Decrypt(File.ReadAllText(path)));
                if(Document.Root.Name.LocalName != "Mocha")
                    return false;
                else if(!ExistsElement(path,"Root/Password"))
                    return false;
                else if(!ExistsElement(path,"Root/Description"))
                    return false;
                else if(!ExistsElement(path,"Sectors"))
                    return false;
                else if(!ExistsElement(path,"Tables"))
                    return false;
                else
                    return true;
            } catch { return false; }
        }

        /// <summary>
        /// Checks for the presence of the element. Example path: MyTable/MyColumn
        /// </summary>
        /// <param name="path">The MochaDB database file path to check.</param>
        /// <param name="elementPath">Path of element.</param>
        public static bool ExistsElement(string path,string elementPath) {
            if(!IsMochaDB(path))
                throw new Exception("The file shown is not a MochaDB database file!");

            string[] elementsName = elementPath.Split('/');

            try {
                XDocument document = XDocument.Parse(AES256.Decrypt(File.ReadAllText(path)));
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

        #region Events

        /// <summary>
        /// This happens after content changed.
        /// </summary>
        public event EventHandler<EventArgs> ChangeContent;
        private void OnChangeContent(object sender,EventArgs e) {
            //Invoke.
            ChangeContent?.Invoke(sender,e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the suitability and robustness of the MochaDB database.
        /// </summary>
        public bool CheckMochaDB() {
            try {
                if(Doc.Root.Name.LocalName != "Mocha")
                    return false;
                else if(!ExistsElement("Root/Password"))
                    return false;
                else if(!ExistsElement("Root/Description"))
                    return false;
                else if(!ExistsElement("Sectors"))
                    return false;
                else if(!ExistsElement("Tables"))
                    return false;
                else
                    return true;
            } catch { return false; }
        }

        /// <summary>
        /// Checks for the presence of the element. Example path: MyTable/MyColumn
        /// </summary>
        /// <param name="elementPath">Path of element.</param>
        public bool ExistsElement(string elementPath) {
            string[] elementsName = elementPath.Split('/');

            try {
                XElement element = Doc.Root.Element(elementsName[0]);

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

        /// <summary>
        /// Returns the password of the MochaDB database.
        /// </summary>
        public string GetPassword() =>
            Doc.Root.Element("Root").Element("Password").Value;

        /// <summary>
        /// Save MochaDB database.
        /// </summary>
        public void Save() {
            sourceStream.Dispose();
            File.WriteAllText(DBPath,AES256.Encrypt(Doc.ToString()));
            sourceStream = File.Open(DBPath,FileMode.Open,FileAccess.ReadWrite);

            OnChangeContent(this,new EventArgs());
        }

        /// <summary>
        /// MochaDB checks the existence of the database file and if not creates a new file. ALL DATA IS LOST!
        /// </summary>
        public void Reset() {
            sourceStream.Dispose();
            File.WriteAllText(DBPath,AES256.Encrypt(EmptyContent));
            sourceStream = File.Open(DBPath,FileMode.Open,FileAccess.ReadWrite);

            OnChangeContent(this,new EventArgs());
        }

        /// <summary>
        /// Sets the MochaDB Database password.
        /// </summary>
        /// <param name="password">Password to set.</param>
        public void SetPassword(string password) {
            Doc.Root.Element("Root").Element("Password").Value = password;
            Save();
        }

        /// <summary>
        /// Returns the description of the database.
        /// </summary>
        public string GetDescription() =>
            Doc.Root.Element("Root").Element("Description").Value;

        /// <summary>
        /// Sets the description of the database.
        /// </summary>
        /// <param name="Description">Description to set.</param>
        public void SetDescription(string Description) {
            Doc.Root.Element("Root").Element("Description").Value = Description;
            Save();
        }

        /// <summary>
        /// Return xml schema of database.
        /// </summary>
        public string GetXML() {
            XDocument doc = XDocument.Parse(Doc.ToString());
            doc.Root.Element("Root").Remove();
            return doc.ToString();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            sourceStream.Dispose();
        }

        #endregion

        #region Sector

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="sector">MochaSector object to add.</param>
        public void AddSector(MochaSector sector) {
            if(ExistsElement("Sectors/" + sector.Name))
                throw new Exception("There is already a sector with this name!");

            XElement xSector = new XElement(sector.Name,sector.Data);
            xSector.Add(new XAttribute("Description",sector.Description));

            Doc.Root.Element("Sectors").Add(xSector);
            Save();
        }

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="name">Name of sector to add.</param>
        public void AddSector(string name) {
            AddSector(new MochaSector(name));
        }

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data of sector.</param>
        public void AddSector(string name,string data) {
            AddSector(new MochaSector(name,data));
        }

        /// <summary>
        /// Remove sector by name.
        /// </summary>
        /// <param name="name">Name of sector to remove.</param>
        public void RemoveSector(string name) {
            if(!ExistsSector(name))
                return;

            Doc.Root.Element("Sectors").Element(name).Remove();
            Save();
        }

        /// <summary>
        /// Rename sector.
        /// </summary>
        /// <param name="name">Name of sector to rename.</param>
        /// <param name="newName">New name of sector.</param>
        public void RenameSector(string name,string newName) {
            if(!ExistsSector(name))
                throw new Exception("Sector not found in this name!");
            if(ExistsSector(newName))
                throw new Exception("There is already a sector with this name!");

            Doc.Root.Element("Sectors").Element(name).Name=newName;
            Save();
        }

        /// <summary>
        /// Return data of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public string GetSectorData(string name) {
            if(!ExistsSector(name))
                throw new Exception("Sector not found in this name!");

            return Doc.Root.Element("Sectors").Element(name).Value;
        }

        /// <summary>
        /// Set data of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data to set.</param>
        public void SetSectorData(string name,string data) {
            if(!ExistsSector(name))
                throw new Exception("Sector not found in this name!");

            Doc.Root.Element("Sectors").Element(name).Value = data;
            Save();
        }

        /// <summary>
        /// Get description of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public string GetSectorDescription(string name) {
            if(!ExistsSector(name))
                throw new Exception("Sector not found in this name!");

            return Doc.Root.Element("Sectors").Element(name).Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="description">Description to set.</param>
        public void SetSectorDescription(string name,string description) {
            if(!ExistsSector(name))
                throw new Exception("Sector not found in this name!");

            Doc.Root.Element("Sectors").Element(name).Attribute("Description").Value = description;
            Save();
        }

        /// <summary>
        /// Return sectors in database.
        /// </summary>
        public List<MochaSector> GetSectors() {
            List<MochaSector> sectors = new List<MochaSector>();

            MochaSector sector;
            IEnumerable<XElement> sectorRange = Doc.Root.Element("Sectors").Elements();
            for(int index = 0; index < sectorRange.Count(); index++) {
                XElement xSector = sectorRange.ElementAt(index);
                sector =
                    new MochaSector(xSector.Name.LocalName,xSector.Value,xSector.Attribute("Description").Value);
                sectors.Add(sector);
            }

            return sectors;
        }

        /// <summary>
        /// Returns whether there is a sector with the specified name.
        /// </summary>
        /// <param name="name">Name of sector to check.</param>
        public bool ExistsSector(string name) =>
            ExistsElement("Sectors/" + name);

        #endregion

        #region Table

        /// <summary>
        /// Add table.
        /// </summary>
        /// <param name="table">MochaTable object to add.</param>
        public void AddTable(MochaTable table) {
            if(ExistsTable(table.Name))
                throw new Exception("There is already a table with this name!");

            XElement Xtable = new XElement(table.Name);
            Doc.Root.Element("Tables").Add(Xtable);

            for(int columnIndex = 0; columnIndex < table.ColumnCount; columnIndex++) {
                AddColumn(table.Name,table.Columns[columnIndex]);
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
        /// Remove table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public void RemoveTable(string name) {
            if(!ExistsTable(name))
                return;

            Doc.Root.Element("Tables").Element(name).Remove();
            Save();
        }

        /// <summary>
        /// Rename table.
        /// </summary>
        /// <param name="name">Name of table to rename.</param>
        /// <param name="newName">New name of table.</param>
        public void RenameTable(string name,string newName) {
            if(!ExistsTable(name))
                throw new Exception("Table not found in this name!");
            if(ExistsTable(newName))
                throw new Exception("There is already a table with this name!");

            Doc.Root.Element("Tables").Element(name).Name=newName;
            Save();
        }

        /// <summary>
        /// Return table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public MochaTable GetTable(string name) {
            if(!ExistsTable(name))
                throw new Exception("Table not found in this name!");

            MochaTable table = new MochaTable(name);

            IEnumerable<XElement> columnRange = Doc.Root.Element("Tables").Element(name).Elements();
            for(int index = 0; index < columnRange.Count(); index++) {
                table.AddColumn(GetColumn(name,columnRange.ElementAt(index).Name.LocalName));
            }

            List<MochaRow> rows = GetRows(name);
            for(int index = 0; index < rows.Count; index++) {
                table.AddRow(rows[index]);
            }

            return table;
        }

        /// <summary>
        /// Return tables in database.
        /// </summary>
        public List<MochaTable> GetTables() {
            List<MochaTable> tables = new List<MochaTable>();

            IEnumerable<XElement> tableRange = Doc.Root.Element("Tables").Elements();
            for(int index = 0; index <tableRange.Count(); index++) {
                tables.Add(GetTable(tableRange.ElementAt(index).Name.LocalName));
            }

            return tables;
        }

        /// <summary>
        /// Returns whether there is a table with the specified name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public bool ExistsTable(string name) =>
            ExistsElement("Tables/" + name);

        #endregion

        #region Column

        /// <summary>
        /// Add colum in table.
        /// </summary>
        /// <param name="tableName">Name of column.</param>
        /// <param name="column">MochaColumn object to add.</param>
        public void AddColumn(string tableName,MochaColumn column) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(ExistsColumn(tableName,column.Name))
                throw new Exception("There is already a column with this name!");

            XElement Xcolumn = new XElement(column.Name);
            Xcolumn.Add(new XAttribute("DataType",column.DataType));
            Xcolumn.Add(new XAttribute("Description",column.Description));

                int rowCount = (int)Query.GetRun("ROWCOUNT:" + tableName);
            if(column.DataType==MochaDataType.AutoInt) {
                for(int index = 1; index <= rowCount; index++)
                    Xcolumn.Add(new XElement(index.ToString()));
            } else {
                for(int index = 1; index <= column.DataCount; index++)
                    Xcolumn.Add(new XElement((string)column.Datas[index].Data));

                for(int index = column.DataCount-1;index < rowCount; index++) {
                    Xcolumn.Add(new XElement((string)MochaData.TryGetData(MochaDataType.Byte,"WrongData")));
                }
            }

            Doc.Root.Element("Tables").Element(tableName).Add(Xcolumn);
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
        public void RemoveColumn(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                return;

            Doc.Root.Element("Tables").Element(tableName).Element(name).Remove();
            Save();
        }

        /// <summary>
        /// Rename column.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column to rename.</param>
        /// <param name="newName">New name of column.</param>
        public void RenameColumn(string tableName,string name,string newName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");
            if(ExistsColumn(tableName,newName))
                throw new Exception("There is already a column with this name!");

            Doc.Root.Element("Tables").Element(tableName).Element(name).Name=newName;
            Save();
        }

        /// <summary>
        /// Get description of column by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public string GetColumnDescription(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");

            return Doc.Root.Element("Tables").Element(tableName).Element(name).Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of column by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="description">Description to set.</param>
        public void SetColumnDescription(string tableName,string name,string description) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");

            Doc.Root.Element("Tables").Element(tableName).Element(name).Attribute("Description").Value = description;
            Save();
        }

        /// <summary>
        /// Returns whether there is a column with the specified name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public bool ExistsColumn(string tableName,string name) =>
            ExistsElement("Tables/"+tableName + "/" + name);

        /// <summary>
        /// Get column from table by name
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public MochaColumn GetColumn(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");

            MochaColumn column = new MochaColumn(name,GetColumnDataType(tableName,name));
            column.Description = GetColumnDescription(tableName,name);

            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(name).Elements();
            for(int index = 0; index < dataRange.Count(); index++) {
                column.AddData(GetData(tableName,name,index));
            }

            return column;
        }

        /// <summary>
        /// Return columns in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public List<MochaColumn> GetColumns(string tableName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");

            List<MochaColumn> columns = new List<MochaColumn>();

            IEnumerable<XElement> columnsRange = Doc.Root.Element("Tables").Element(tableName).Elements();
            for(int index = 0; index < columnsRange.Count(); index++) {
                columns.Add(GetColumn(tableName,columnsRange.ElementAt(index).Name.LocalName));
            }

            return columns;
        }

        /// <summary>
        /// Return column datatype by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public MochaDataType GetColumnDataType(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");

            return Enum.Parse<MochaDataType>(Doc.Root.Element("Tables").Element(tableName).Element(name).Attribute("DataType").Value);
        }

        /// <summary>
        /// Set column datatype by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        /// <param name="dataType">MochaDataType to set.</param>
        public void SetColumnDataType(string tableName,string name,MochaDataType dataType) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");

            XElement column = Doc.Root.Element("Tables").Element(tableName).Element(name);
            column.Attribute("DataType").Value = dataType.ToString();

            IEnumerable<XElement> dataRange = column.Elements();
            if(dataType == MochaDataType.AutoInt) {
                for(int index = 0; index <dataRange.Count(); index++) {
                    dataRange.ElementAt(index).Value = index.ToString();
                }

                return;
            }

            for(int index = 0; index < dataRange.Count(); index++) {
                if(!MochaData.IsType(dataType,dataRange.ElementAt(index).Value))
                    dataRange.ElementAt(index).Value = MochaData.TryGetData(dataType,dataRange.ElementAt(index).Value).ToString();
            }

            Save();
        }

        /// <summary>
        /// Return column's last AutoInt value by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public int GetColumnAutoIntState(string tableName,string name) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,name))
                throw new Exception("Column not found in this name!");

            XElement lastData = (XElement)Doc.Root.Element("Tables").Element(tableName).Element(name).LastNode;

            MochaDataType dataType = GetColumnDataType(tableName,name);

            if(dataType != MochaDataType.AutoInt)
                throw new Exception("This column's datatype is not AutoInt!");

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
                throw new Exception("Table not found in this name!");

            IEnumerable<XElement> columnRange = Doc.Root.Element("Tables").Element(tableName).Elements();

            if(columnRange.Count() != row.DataCount)
                throw new Exception("The data count of the row is not equal to the number of columns!");

            for(int index = 0; index <columnRange.Count(); index++) {
                AddData(tableName,columnRange.ElementAt(index).Name.LocalName,row.Datas[index]);
            }
        }

        /// <summary>
        /// Remove row from table by index.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="index">Index of row to remove.</param>
        public void RemoveRow(string tableName,int index) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");

            IEnumerable<XElement> columnRange = Doc.Root.Element("Tables").Element(tableName).Elements();
            for(int columnIndex = 0; columnIndex < columnRange.Count(); columnIndex++) {
                IEnumerable<XElement> dataRange = columnRange.ElementAt(columnIndex).Elements();
                for(int dataIndex = 0; dataIndex < dataRange.Count(); dataIndex++) {
                    if(dataIndex == index) {
                        dataRange.ElementAt(dataIndex).Remove();
                        break;
                    }
                }
            }

            Save();
        }

        /// <summary>
        /// Return row from table by index.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="index">Index of row.</param>
        public MochaRow GetRow(string tableName,int index) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");

            IList<MochaColumn> columns = GetColumns(tableName);

            if(columns.Count == 0)
                return new MochaRow();

            MochaRow row = new MochaRow();
            MochaData[] datas = new MochaData[columns.Count];

            for(int columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
                datas[columnIndex] = columns[columnIndex].Datas[index];
            }

            //

            for(int dataIndex = 0; dataIndex < datas.Length; dataIndex++) {
                if(datas[dataIndex] != null)
                    continue;

                datas[dataIndex] = new MochaData(columns[dataIndex].DataType,
                    MochaData.TryGetData(columns[dataIndex].DataType,""));
            }

            row.AddDataRange(datas);

            return row;
        }

        /// <summary>
        /// Return rows from table.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public List<MochaRow> GetRows(string tableName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");

            List<MochaRow> rows = new List<MochaRow>();

            try {
                XElement firstColumn = (XElement)Doc.Root.Element("Tables").Element(tableName).FirstNode;

                int dataCount = GetDataCount(tableName,firstColumn.Name.LocalName);
                for(int index = 0; index < dataCount; index++) {
                    rows.Add(GetRow(tableName,index));
                }

                return rows;
            } catch {
                return rows;
            }
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
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            XElement Xdata = new XElement("Data",data.Data);

            XElement column = Doc.Root.Element("Tables").Element(tableName).Element(columnName);

            MochaDataType dataType = Enum.Parse<MochaDataType>(column.Attribute("DataType").Value);
            if(dataType == MochaDataType.AutoInt) {
                Xdata.Value = (1 + GetColumnAutoIntState(tableName,columnName)).ToString();
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.Data.ToString())) {
                if(ExistsData(tableName,columnName,data))
                    throw new Exception("Any value can be added to a unique column only once!");
            }

            if(!MochaData.IsType(dataType,data.Data))
                throw new Exception("The submitted data is not compatible with the targeted data!");

            Doc.Root.Element("Tables").Element(tableName).Element(columnName).Add(Xdata);
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
        /// <param name="data">Data to replace.</param>
        /// <param name="index">Index of data.</param>
        public void UpdateData(string tableName,string columnName,int index,object data) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            data = data == null ? "" : data;
            XElement xColumn = Doc.Root.Element("Tables").Element(tableName).Element(columnName);

            MochaDataType dataType = GetColumnDataType(tableName,columnName);
            if(dataType == MochaDataType.AutoInt) {
                throw new Exception("The data type of this column is AutoInt, so data update cannot be done!");
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.ToString())) {
                if(ExistsData(tableName,columnName,data.ToString()))
                    throw new Exception("Any value can be added to a unique column only once!");
            } else if(!MochaData.IsType(dataType,data)) {
                throw new Exception("The submitted data is not compatible with the targeted data!");
            }

            IEnumerable<XElement> dataRange = xColumn.Elements();
            if(dataRange.Count() - 1 < index)
                throw new Exception("This index is larger than the maximum number of data in the table!");

            dataRange.ElementAt(index).Value=data.ToString();

            Save();
        }

        /// <summary>
        /// Returns whether there is a data with the specified.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">MochaData object to check.</param>
        public bool ExistsData(string tableName,string columnName,MochaData data) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            string stringData = data.Data.ToString();
            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements();
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
        /// Return data index. If there are two of the same data, it returns the index of the one you found first!
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="data">Data to find index.</param>
        public int GetDataIndex(string tableName,string columnName,object data) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            string stringData = data.ToString();

            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements();
            for(int index = 0; index < dataRange.Count(); index++) {
                if(dataRange.ElementAt(index).Value == stringData)
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Return data by index.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        /// <param name="index">Index of data.</param>
        public MochaData GetData(string tableName,string columnName,int index) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            MochaDataType dataType = GetColumnDataType(tableName,columnName);

            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements();
            if(dataRange.Count() - 1 < index)
                throw new Exception("This index is larger than the maximum number of data in the table!");

            return new MochaData() { dataType = dataType, data = dataRange.ElementAt(index).Value };
        }

        /// <summary>
        /// Return datas in column from table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public List<MochaData> GetDatas(string tableName,string columnName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            List<MochaData> datas = new List<MochaData>();

            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements();
            for(int index = 0; index < dataRange.Count(); index++) {
                datas.Add(GetData(tableName,columnName,index));
            }
            return datas;
        }

        /// <summary>
        /// Get data count of table's column.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public int GetDataCount(string tableName,string columnName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            return Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements().Count();
        }

        #endregion

        #region Properties

        /// <summary>
        /// MochaQuery object.
        /// </summary>
        public MochaQuery Query { get; private set; }

        /// <summary>
        /// Directory path of database.
        /// </summary>
        public string DBPath { get; private set; }

        /// <summary>
        /// Name of database.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Accessibility to the database.
        /// </summary>
        public bool IsConnected =>
            File.Exists(DBPath);

        /// <summary>
        /// XML Document.
        /// </summary>
        internal XDocument Doc { get; set; }

        /// <summary>
        /// The most basic content of the database.
        /// </summary>
        internal static string EmptyContent => "<?MochaDB version=\"" + Version + "\"?>\n" +
                        "<Mocha>\n" +
                        "  <Root>\n" +
                        "    <Password DataType=\"String\" Description=\"Password of database.\"></Password>\n" +
                        "    <Description DataType=\"String\" Description=\"Description of database.\"></Description>" +
                        "  </Root>\n" +
                        "  <Sectors>\n" +
                        "  </Sectors>\n" +
                        "  <Tables>\n" +
                        "  </Tables>\n" +
                        "</Mocha>";

        /// <summary>
        /// Version of MochaDB.
        /// </summary>
        internal static string Version =>
            "1.0.0";

        #endregion
    }
}
