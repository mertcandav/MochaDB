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

using MochaDB.Connection;
using MochaDB.Cryptography;
using MochaDB.Querying;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaDatabase provides management of a MochaDB database.
    /// </summary>
    [Serializable]
    public class MochaDatabase:IMochaDatabase {
        #region Fields

        private static string
            Iv = "MochaDB#$#3{2533",
            Key = "MochaDBM6YxoFsLXu33FpJdjX0R89xGF";

        private FileStream sourceStream;
        private AES256 aes256;

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
            this(new MochaProvider("path="+path+";password="+password)) { }

        /// <summary>
        /// Create new MochaDatabase.
        /// </summary>
        /// <param name="provider">Provider for connect database.</param>
        public MochaDatabase(MochaProvider provider) {
            provider.EnableReadonly();
            Provider=provider;
            aes256=new AES256(Iv,Key);
            State=MochaConnectionState.Disconnected;

            MochaProviderAttribute autoConnect = Provider.GetAttribute("AutoConnect");
            if(autoConnect!=null && autoConnect.Value.Equals("True",StringComparison.InvariantCultureIgnoreCase)) {
                Connect();
            }
        }

        #endregion

        #region Events

        #region Internal

        /// <summary>
        /// This happens before connection check.
        /// </summary>
        internal event EventHandler<EventArgs> ConnectionCheckRequired;
        private void OnConnectionCheckRequired(object sender,EventArgs e) {
            //Invoke.
            ConnectionCheckRequired?.Invoke(sender,e);

            if(State!=MochaConnectionState.Connected)
                throw new Exception("Connection is not open!");
        }

        #endregion

        /// <summary>
        /// This happens after content changed.
        /// </summary>
        public event EventHandler<EventArgs> ChangeContent;
        private void OnChangeContent(object sender,EventArgs e) {
            //Invoke.
            ChangeContent?.Invoke(sender,e);
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
                throw new Exception("The file shown is not a MochaDB database file!");

            string[] elementsName = elementPath.Split('/');

            try {
                XDocument document = XDocument.Parse(new AES256(Iv,Key).Decrypt(File.ReadAllText(path)));
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

            File.WriteAllText(path + ".mochadb",new AES256(Iv,Key).Encrypt(content));
        }

        /// <summary>
        /// Checks the suitability and robustness of the MochaDB database.
        /// </summary>
        /// <param name="path">The file path of the MochaDB database to be checked.</param>
        public static bool CheckMochaDB(string path) {
            if(!IsMochaDB(path))
                throw new Exception("The file shown is not a MochaDB database file!");

            try {
                XDocument Document = XDocument.Parse(new AES256(Iv,Key).Decrypt(File.ReadAllText(path)));
                if(Document.Root.Name.LocalName != "Mocha")
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
                else
                    return true;
            } catch { return false; }
        }

        #endregion

        #region Xml

        /// <summary>
        /// Return xml schema of database.
        /// </summary>
        public string GetXML() {
            OnConnectionCheckRequired(this,new EventArgs());

            XDocument doc = XDocument.Parse(Doc.ToString());
            doc.Root.Element("Root").Remove();
            return doc.ToString();
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

            Doc = XDocument.Parse(aes256.Decrypt(File.ReadAllText(Provider.Path,Encoding.UTF8)));

            if(!CheckMochaDB())
                throw new Exception("The MochaDB database is corrupt!");
            if(!string.IsNullOrEmpty(GetPassword()) && string.IsNullOrEmpty(Provider.Password))
                throw new Exception("The MochaDB database is password protected!");
            else if(Provider.Password != GetPassword())
                throw new Exception("MochaDB database password does not match the password specified!");

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

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            Disconnect();
        }

        #endregion

        #region Database

        #region Internal

        /// <summary>
        /// Checks the suitability and robustness of the MochaDB database.
        /// </summary>
        internal bool CheckMochaDB() {
            try {
                if(Doc.Root.Name.LocalName != "Mocha")
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
                else
                    return true;
            } catch { return false; }
        }

        /// <summary>
        /// Return element by path.
        /// </summary>
        /// <param name="path">Path of element.</param>
        internal XElement GetElement(string path) {
            string[] elementsName = path.Split('/');

            XElement element = Doc.Root.Element(elementsName[0]);

            if(element==null)
                return null;

            for(int i = 1; i < elementsName.Length; i++) {
                element = element.Element(elementsName[i]);
                if(element == null)
                    return null;
            }

            return element;
        }

        /// <summary>
        /// Checks for the presence of the element.
        /// </summary>
        /// <param name="path">Path of element.</param>
        internal bool ExistsElement(string path) =>
            GetElement(path) != null;

        /// <summary>
        /// Save MochaDB database.
        /// </summary>
        internal void Save() {
            string content = aes256.Encrypt(Doc.ToString());
            string password = GetPassword();
            Disconnect();
            File.WriteAllText(Provider.Path,content);
            Provider.Readonly=false;
            Provider.ConnectionString="path="+Provider.Path+";password="+password;
            Provider.EnableReadonly();
            Connect();

            OnChangeContent(this,new EventArgs());
        }

        #endregion

        /// <summary>
        /// Returns the password of the MochaDB database.
        /// </summary>
        public string GetPassword() {
            OnConnectionCheckRequired(this,new EventArgs());

            return Doc.Root.Element("Root").Element("Password").Value;
        }

        /// <summary>
        /// Sets the MochaDB Database password.
        /// </summary>
        /// <param name="password">Password to set.</param>
        public void SetPassword(string password) {
            OnConnectionCheckRequired(this,new EventArgs());

            Doc.Root.Element("Root").Element("Password").Value = password;
            Save();
        }

        /// <summary>
        /// Returns the description of the database.
        /// </summary>
        public string GetDescription() {
            OnConnectionCheckRequired(this,new EventArgs());

            return Doc.Root.Element("Root").Element("Description").Value;
        }

        /// <summary>
        /// Sets the description of the database.
        /// </summary>
        /// <param name="Description">Description to set.</param>
        public void SetDescription(string Description) {
            OnConnectionCheckRequired(this,new EventArgs());

            Doc.Root.Element("Root").Element("Description").Value = Description;
            Save();
        }

        /// <summary>
        /// MochaDB checks the existence of the database file and if not creates a new file. ALL DATA IS LOST!
        /// </summary>
        public void Reset() {
            OnConnectionCheckRequired(this,new EventArgs());

            Disconnect();
            File.WriteAllText(Provider.Path,aes256.Encrypt(EmptyContent));
            Provider.Readonly=false;
            Provider.ConnectionString="path="+Provider.Path;
            Provider.EnableReadonly();
            Connect();

            OnChangeContent(this,new EventArgs());
        }

        #endregion

        #region Sector

        /// <summary>
        /// Remove all sectors.
        /// </summary>
        public void ClearSectors() {
            OnConnectionCheckRequired(this,new EventArgs());

            Doc.Root.Element("Sectors").RemoveNodes();
            Save();
        }

        /// <summary>
        /// Add sector.
        /// </summary>
        /// <param name="sector">MochaSector object to add.</param>
        public void AddSector(MochaSector sector) {
            if(ExistsSector(sector.Name))
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

            if(name == newName)
                return;

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

            XElement xSector = Doc.Root.Element("Sectors").Element(name);
            if(xSector.Value==data)
                return;

            xSector.Value=data;
            Save();
        }

        /// <summary>
        /// Return description of sector by name.
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

            XAttribute xDescription = Doc.Root.Element("Sectors").Element(name).Attribute("Description");
            if(xDescription.Value==description)
                return;

            xDescription.Value=description;

            Save();
        }

        /// <summary>
        /// Return sector by name.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public MochaSector GetSector(string name) {
            if(!ExistsSector(name))
                throw new Exception("Sector not found in this name!");

            XElement xSector = Doc.Root.Element("Sectors").Element(name);
            MochaSector sector = new MochaSector(xSector.Name.LocalName);
            sector.Data=xSector.Value;
            sector.Description =xSector.Attribute("Description").Value;

            return sector;
        }

        /// <summary>
        /// Return sectors in database.
        /// </summary>
        public MochaCollectionResult<MochaSector> GetSectors() {
            OnConnectionCheckRequired(this,new EventArgs());

            List<MochaSector> sectors = new List<MochaSector>();

            IEnumerable<XElement> sectorRange = Doc.Root.Element("Sectors").Elements();
            for(int index = 0; index < sectorRange.Count(); index++)
                sectors.Add(GetSector(sectorRange.ElementAt(index).Name.LocalName));

            return new MochaCollectionResult<MochaSector>(sectors);
        }

        /// <summary>
        /// Returns whether there is a sector with the specified name.
        /// </summary>
        /// <param name="name">Name of sector to check.</param>
        public bool ExistsSector(string name) =>
            ExistsElement("Sectors/" + name);

        #endregion

        #region Stack

        /// <summary>
        /// Remove all stacks.
        /// </summary>
        public void ClearStacks() {
            OnConnectionCheckRequired(this,new EventArgs());

            Doc.Root.Element("Stacks").RemoveNodes();
            Save();
        }

        /// <summary>
        /// Add stack.
        /// </summary>
        /// <param name="stack">MohcaStack object to add.</param>
        public void AddStack(MochaStack stack) {
            if(ExistsStack(stack.Name))
                throw new Exception("There is already a stack with this name!");

            XElement xStack = new XElement(stack.Name);
            xStack.Add(new XAttribute("Description",stack.Description));

            if(stack.Items.Count > 0)
                for(int index = 0; index < stack.Items.Count; index++) {
                    xStack.Add(GetMochaStackItemXML(stack.Items[index]));
                }

            Doc.Root.Element("Stacks").Add(xStack);

            Save();
        }

        /// <summary>
        /// Remove stack by name.
        /// </summary>
        /// <param name="name">Name of stack to remove.</param>
        public void RemoveStack(string name) {
            if(!ExistsStack(name))
                return;

            Doc.Root.Element("Stacks").Element(name).Remove();
            Save();
        }

        /// <summary>
        /// Return description of stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public string GetStackDescription(string name) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            return Doc.Root.Element("Stacks").Element(name).Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="description">Description to set.</param>
        public void SetStackDescription(string name,string description) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XAttribute xDescription = Doc.Root.Element("Stacks").Element(name).Attribute("Description");
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
                throw new Exception("Stack not found in this name!");
            if(ExistsStack(newName))
                throw new Exception("There is already a stack with this name!");

            Doc.Root.Element("Stacks").Element(name).Name=newName;
            Save();
        }

        /// <summary>
        /// Return stack by name.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public MochaStack GetStack(string name) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement xStack = Doc.Root.Element("Stacks").Element(name);
            MochaStack stack = new MochaStack(xStack.Name.LocalName);
            stack.Description=xStack.Attribute("Description").Value;

            IEnumerable<XElement> elementRange = xStack.Elements();
            if(elementRange.Count() > 0)
                for(int index = 0; index < elementRange.Count(); index++) {
                    stack.Items.Add(GetStackItem(name,elementRange.ElementAt(index).Name.LocalName));
                }

            return stack;
        }

        /// <summary>
        /// Get all stacks in database.
        /// </summary>
        /// <returns></returns>
        public MochaCollectionResult<MochaStack> GetStacks() {
            OnConnectionCheckRequired(this,new EventArgs());

            List<MochaStack> stacks = new List<MochaStack>();

            IEnumerable<XElement> stackRange = Doc.Root.Element("Stacks").Elements();

            if(stackRange.Count() > 0)
                for(int index = 0; index < stackRange.Count(); index++) {
                    stacks.Add(GetStack(stackRange.ElementAt(index).Name.LocalName));
                }

            return new MochaCollectionResult<MochaStack>(stacks);
        }

        /// <summary>
        /// Returns whether there is a stack with the specified name.
        /// </summary>
        /// <param name="name">Name of stack to check.</param>
        public bool ExistsStack(string name) =>
            ExistsElement("Stacks/"+name);

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

            if(item.Items.Count >=0)
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
        /// <param name="path">Path of stack item to add.</param>
        /// <param name="item">MochaStackItem object to add.</param>
        public void AddStackItem(string name,string path,MochaStackItem item) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement element = !string.IsNullOrWhiteSpace(path) ? GetElement("Stacks/"+name +"/"+path) :
                Doc.Root.Element("Stacks").Element(name);
            if(element==null)
                throw new Exception("The road is wrong, there is no such way!");

            element.Add(GetMochaStackItemXML(item));
            Save();
        }

        /// <summary>
        /// Remove item of stack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Name path of stack item to remove.</param>
        public void RemoveStackItem(string name,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            if(path==string.Empty)
                Doc.Root.Element("Stacks").Element(name).RemoveAll();
            else {
                XElement element = GetElement("Stacks/"+name+"/"+path);
                if(element==null)
                    return;

                element.Remove();
            }

            Save();
        }

        /// <summary>
        /// Set value of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="path">Name path of stack item to set value.</param>
        public void SetStackItemValue(string name,string value,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement element = path.Contains('/') ? GetElement("Stacks/"+name +"/"+path) :
                Doc.Root.Element("Stacks").Element(name).Element(path);

            if(element==null)
                throw new Exception("The road is wrong, there is no such way!");

            if(element.Value == value)
                return;

            element.Value=value;
            Save();
        }

        /// <summary>
        /// Return value of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Name path of stack item to get value.</param>
        public string GetStackItemValue(string name,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement element = path.Contains('/') ? GetElement("Stacks/"+name +"/"+path) :
                Doc.Root.Element("Stacks").Element(name).Element(path);

            if(element==null)
                throw new Exception("The road is wrong, there is no such way!");

            return element.Value;
        }

        /// <summary>
        /// Set description of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="description">Description to set.</param>
        /// <param name="path">Name path of stack item to set description.</param>
        public void SetStackItemDescription(string name,string description,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement element = path.Contains('/') ? GetElement("Stacks/"+name +"/"+path) :
                Doc.Root.Element("Stacks").Element(name).Element(path);

            if(element==null)
                throw new Exception("The road is wrong, there is no such way!");

            if(element.Attribute("Description").Value == description)
                return;

            element.Attribute("Description").Value=description;
            Save();
        }

        /// <summary>
        /// Return description of stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Name path of stack item to get description.</param>
        public string GetStackItemDescription(string name,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement element = path.Contains('/') ? GetElement("Stacks/"+name +"/"+path) :
                Doc.Root.Element("Stacks").Element(name).Element(path);

            if(element==null)
                throw new Exception("The road is wrong, there is no such way!");

            return element.Attribute("Description").Value;
        }

        /// <summary>
        /// Rename stack item.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="newName">New name of stack item.</param>
        /// <param name="path">Name path of stack item to get description.</param>
        public void RenameStackItem(string name,string newName,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement element = path.Contains('/') ? GetElement("Stacks/"+name +"/"+path) :
                Doc.Root.Element("Stacks").Element(name).Element(path);

            if(element==null)
                throw new Exception("The road is wrong, there is no such way!");

            if(element.Name.LocalName == newName)
                return;

            if(path.Contains('/') && ExistsStackItem(name,path[0..path.IndexOf("/")] + "/"+newName))
                throw new Exception("There is already a stack item with this name!");

            element.Name=newName;
            Save();
        }

        /// <summary>
        /// Return StackItem.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="path">Name path of stack item to get description.</param>
        public MochaStackItem GetStackItem(string name,string path) {
            if(!ExistsStack(name))
                throw new Exception("Stack not found in this name!");

            XElement xStackItem = GetElement("Stacks/" + name + "/" + path);

            MochaStackItem item = new MochaStackItem(xStackItem.Name.LocalName);
            item.Description=xStackItem.Attribute("Description").Value;
            item.Value=xStackItem.Value;

            IEnumerable<XElement> elementRange = xStackItem.Elements();
            if(elementRange.Count() >0)
                for(int index = 0; index < elementRange.Count(); index++)
                    item.Items.Add(GetStackItem(name,path += "/" + elementRange.ElementAt(index).Name.LocalName));

            return item;
        }

        /// <summary>
        /// Returns whether there is a stack item with the specified name.
        /// </summary>
        /// <param name="name">Name of stack item.</param>
        /// <param name="path">Name path of item to check.</param>
        public bool ExistsStackItem(string name,string path) =>
            ExistsElement("Stacks/"+name+"/"+path);

        #endregion

        #region Table

        /// <summary>
        /// Remove all tables.
        /// </summary>
        public void ClearTables() {
            OnConnectionCheckRequired(this,new EventArgs());

            Doc.Root.Element("Tables").RemoveNodes();
            Save();
        }

        /// <summary>
        /// Add table.
        /// </summary>
        /// <param name="table">MochaTable object to add.</param>
        public void AddTable(MochaTable table) {
            if(ExistsTable(table.Name))
                throw new Exception("There is already a table with this name!");

            XElement xTable = new XElement(table.Name);
            xTable.Add(new XAttribute("Description",table.Description));
            Doc.Root.Element("Tables").Add(xTable);

            for(int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++) {
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

            if(name == newName)
                return;

            if(ExistsTable(newName))
                throw new Exception("There is already a table with this name!");

            Doc.Root.Element("Tables").Element(name).Name=newName;
            Save();
        }

        /// <summary>
        /// Get description of table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public string GetTableDescription(string name) {
            if(!ExistsTable(name))
                throw new Exception("Table not found in this name!");

            return Doc.Root.Element("Tables").Element(name).Attribute("Description").Value;
        }

        /// <summary>
        /// Set description of table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="description">Description to set.</param>
        public void SetTableDescription(string name,string description) {
            if(!ExistsTable(name))
                throw new Exception("Table not found in this name!");

            XAttribute xDescription = Doc.Root.Element("Tables").Element(name).Attribute("Description");
            if(xDescription.Value==description)
                return;

            xDescription.Value=description;

            Save();
        }

        /// <summary>
        /// Return table by name.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public MochaTable GetTable(string name) {
            if(!ExistsTable(name))
                throw new Exception("Table not found in this name!");

            XElement xTable = Doc.Root.Element("Tables").Element(name);
            MochaTable table = new MochaTable(name);
            table.Description=xTable.Attribute("Description").Value;

            table.Columns.collection.AddRange(GetColumns(name).collection);
            table.Rows.collection.AddRange(GetRows(name).collection);

            return table;
        }

        /// <summary>
        /// Return tables in database.
        /// </summary>
        public MochaCollectionResult<MochaTable> GetTables() {
            OnConnectionCheckRequired(this,new EventArgs());

            List<MochaTable> tables = new List<MochaTable>();

            IEnumerable<XElement> tableRange = Doc.Root.Element("Tables").Elements();
            for(int index = 0; index <tableRange.Count(); index++) {
                tables.Add(GetTable(tableRange.ElementAt(index).Name.LocalName));
            }

            return new MochaCollectionResult<MochaTable>(tables);
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
                for(int index = 1; index <= column.Datas.Count; index++)
                    Xcolumn.Add(new XElement(column.Datas[index].Data.ToString()));

                for(int index = column.Datas.Count; index < rowCount; index++) {
                    Xcolumn.Add(new XElement("Data",MochaData.TryGetData(column.DataType,"").ToString()));
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

            if(name == newName)
                return;

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

            XAttribute xDescription = Doc.Root.Element("Tables").Element(tableName).Element(name).Attribute("Description");
            if(xDescription.Value==description)
                return;

            xDescription.Value = description;
            Save();
        }

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
                column.Datas.collection.Add(GetData(tableName,name,index));
            }

            return column;
        }

        /// <summary>
        /// Return columns in table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public MochaCollectionResult<MochaColumn> GetColumns(string tableName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");

            List<MochaColumn> columns = new List<MochaColumn>();

            IEnumerable<XElement> columnsRange = Doc.Root.Element("Tables").Element(tableName).Elements();
            for(int index = 0; index < columnsRange.Count(); index++) {
                columns.Add(GetColumn(tableName,columnsRange.ElementAt(index).Name.LocalName));
            }

            return new MochaCollectionResult<MochaColumn>(columns);
        }

        /// <summary>
        /// Returns whether there is a column with the specified name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="name">Name of column.</param>
        public bool ExistsColumn(string tableName,string name) =>
            ExistsElement("Tables/"+tableName + "/" + name);

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

            XElement xColumn = Doc.Root.Element("Tables").Element(tableName).Element(name);
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
            
            if(columnRange.Count() != row.Datas.Count)
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

            if(index < 0)
                throw new Exception("Index can not lower than 0!");

            MochaCollectionResult<MochaColumn> columns = GetColumns(tableName);

            if(columns.Count == 0)
                return null;

            int rowCount = (int)Query.GetRun("ROWCOUNT:" + tableName);
            if(rowCount-1 < index)
                throw new Exception("Index cat not bigger than row count!");

            MochaRow row = new MochaRow();
            MochaData[] datas = new MochaData[columns.Count];

            for(int columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
                datas[columnIndex] = columns[columnIndex].Datas[index];
            }

            row.Datas.AddRange(datas);

            return row;
        }

        /// <summary>
        /// Return rows from table.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        public MochaCollectionResult<MochaRow> GetRows(string tableName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");

            List<MochaRow> rows = new List<MochaRow>();
            XElement firstColumn = (XElement)Doc.Root.Element("Tables").Element(tableName).FirstNode;

            if(firstColumn==null)
                return new MochaCollectionResult<MochaRow>(rows);

            int dataCount = GetDataCount(tableName,firstColumn.Name.LocalName);
            for(int index = 0; index < dataCount; index++) {
                rows.Add(GetRow(tableName,index));
            }

            return new MochaCollectionResult<MochaRow>(rows);
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

            XElement xData = new XElement("Data",data.Data);

            XElement xColumn = Doc.Root.Element("Tables").Element(tableName).Element(columnName);

            MochaDataType dataType = Enum.Parse<MochaDataType>(xColumn.Attribute("DataType").Value);
            if(dataType == MochaDataType.AutoInt) {
                xData.Value = (1 + GetColumnAutoIntState(tableName,columnName)).ToString();
            } else if(dataType == MochaDataType.Unique && !string.IsNullOrEmpty(data.Data.ToString())) {
                if(ExistsData(tableName,columnName,data))
                    throw new Exception("Any value can be added to a unique column only once!");
            }

            if(!MochaData.IsType(dataType,data.Data))
                throw new Exception("The submitted data is not compatible with the targeted data!");

            IEnumerable<XElement> columnRange = Doc.Root.Element("Tables").Element(tableName).Elements();
            for(int columnIndex = 0; columnIndex < columnRange.Count(); columnIndex++) {
                XElement element = columnRange.ElementAt(columnIndex);

                if(element.Name.LocalName==columnName)
                    continue;

                MochaDataType _dataType = Enum.Parse<MochaDataType>(element.Attribute("DataType").Value);
                if(_dataType == MochaDataType.AutoInt) {
                    element.Add(
                        new XElement("Data",1 + GetColumnAutoIntState(tableName,element.Name.LocalName),string.Empty));
                    continue;
                }

                element.Add(
                    new XElement("Data",MochaData.TryGetData(GetColumnDataType(tableName,element.Name.LocalName),string.Empty)));
            }

            Doc.Root.Element("Tables").Element(tableName).Element(columnName).Add(xData);

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

            if(index < 0)
                throw new Exception("Index can not lower than 0!");

            MochaDataType dataType = GetColumnDataType(tableName,columnName);

            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements();
            if(dataRange.Count() - 1 < index)
                throw new Exception("This index is larger than the maximum number of data in the table!");

            return new MochaData() { dataType = dataType,data = dataRange.ElementAt(index).Value };
        }

        /// <summary>
        /// Return datas in column from table by name.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="columnName">Name of column.</param>
        public MochaCollectionResult<MochaData> GetDatas(string tableName,string columnName) {
            if(!ExistsTable(tableName))
                throw new Exception("Table not found in this name!");
            if(!ExistsColumn(tableName,columnName))
                throw new Exception("Column not found in this name!");

            List<MochaData> datas = new List<MochaData>();

            IEnumerable<XElement> dataRange = Doc.Root.Element("Tables").Element(tableName).Element(columnName).Elements();
            for(int index = 0; index < dataRange.Count(); index++) {
                datas.Add(GetData(tableName,columnName,index));
            }
            return new MochaCollectionResult<MochaData>(datas);
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

        #region Internal

        /// <summary>
        /// XML Document.
        /// </summary>
        internal XDocument Doc { get; set; }

        /// <summary>
        /// The most basic content of the database.
        /// </summary>
        internal static string EmptyContent => "<?MochaDB Version=\"" + Version + "\"?>\n" +
                        "<Mocha Description=\"Root element of database.\">>\n" +
                        "  <Root Description=\"Base of root.\">>\n" +
                        "    <Password DataType=\"String\" Description=\"Password of database.\"></Password>\n" +
                        "    <Description DataType=\"String\" Description=\"Description of database.\"></Description>" +
                        "  </Root>\n" +
                        "  <Sectors Description=\"Base of sectors.\">>\n" +
                        "  </Sectors>\n" +
                        "  <Stacks Description=\"Base of stacks.\">>\n" +
                        "  </Stacks>\n" +
                        "  <Tables Description=\"Base of tables.\">\n" +
                        "  </Tables>\n" +
                        "</Mocha>";

        /// <summary>
        /// Version of MochaDB.
        /// </summary>
        internal static string Version =>
            "2.0.0";

        #endregion

        /// <summary>
        /// Connection provider.
        /// </summary>
        public MochaProvider Provider { get; private set; }

        /// <summary>
        /// MochaQuery object.
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
