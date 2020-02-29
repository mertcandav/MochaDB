using MochaDB.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MochaDB.FileSystem {
    /// <summary>
    /// FileSystem manager for MochaDB.
    /// </summary>
    public class MochaFileSystem:IMochaFileSystem {
        #region Fields

        private MochaDatabase database;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaFileSystem.
        /// </summary>
        /// <param name="database">MochaDatabase object to use file system processes.</param>
        /// <param name="embedded">Is embedded file system in database.</param>
        internal MochaFileSystem(MochaDatabase database,bool embedded) {
            this.database = database;
            IsDatabaseEmbedded=embedded;
        }

        /// <summary>
        /// Create new MochaFileSystem.
        /// </summary>
        /// <param name="database">MochaDatabase object to use file system processes.</param>
        public MochaFileSystem(MochaDatabase database) {
            IsDatabaseEmbedded=false;
            Database = database;
        }

        #endregion

        #region Methods

        #region Internal

        /// <summary>
        /// Process the path for use.
        /// </summary>
        /// <param name="value">Path.</param>
        internal void ProcessPath(ref string value) {
            value=value.Replace('\\','/');
        }

        #endregion

        /// <summary>
        /// Remove all disks.
        /// </summary>
        public void ClearDisks() {
            Database.OnConnectionCheckRequired(this,new EventArgs());
            Database.Doc.Root.Element("FileSystem").RemoveNodes();

            Database.Save();
        }

        /// <summary>
        /// Return file by path.
        /// </summary>
        /// <param name="path">path of file.</param>
        public MochaResult<MochaFile> GetFile(string path) {
            if(!ExistsFile(path))
                return null;

            ProcessPath(ref path);
            int lastSlash = path.LastIndexOf('/');
            XElement fileElement = Database.GetElement($"FileSystem/{path.Substring(0,lastSlash == -1 ? path.Length : lastSlash)}").Elements().Where(x =>
            x.Attribute("Type").Value=="File").First();

            MochaFile file = new MochaFile(fileElement.Name.LocalName,fileElement.Name.LocalName.Split('.').Last());
            file.Description=fileElement.Attribute("Description").Value;
            file.Bytes=Convert.FromBase64String(fileElement.Value);

            return file;
        }

        /// <summary>
        /// Return all files.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaCollectionResult<MochaFile> GetFiles(string path) {
            List<MochaFile> files = new List<MochaFile>();
            if(!ExistsDirectory(path))
                return new MochaCollectionResult<MochaFile>(files);

            ProcessPath(ref path);
            int lastSlash = path.LastIndexOf('/');
            IEnumerable<XElement> fileRange = Database.GetElement($"FileSystem/{path.Substring(0,lastSlash == -1 ? path.Length : lastSlash)}").Elements().Where(
                x => x.Attribute("Type").Value=="File");
            for(int index = 0; index < fileRange.Count(); index++)
                files.Add(GetFile($"{path}/{fileRange.ElementAt(index).Name.LocalName}"));

            return new MochaCollectionResult<MochaFile>(files);
        }

        /// <summary>
        /// Return directory by path.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaResult<MochaDirectory> GetDirectory(string path) {
            if(!ExistsDirectory(path))
                return null;

            ProcessPath(ref path);
            XElement directoryElement = Database.GetElement($"FileSystem/{path}");
            MochaDirectory directory = new MochaDirectory(directoryElement.Name.LocalName);
            directory.Description=directoryElement.Attribute("Description").Value;
            directory.Files.AddRange(GetFiles(path).collection);
            directory.Directories.AddRange(GetDirectories(path).collection);

            return directory;
        }

        /// <summary>
        /// Return all directories.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaCollectionResult<MochaDirectory> GetDirectories(string path) {
            List<MochaDirectory> directories = new List<MochaDirectory>();
            if(!ExistsDisk(path) && !ExistsDirectory(path))
                return new MochaCollectionResult<MochaDirectory>(directories);

            ProcessPath(ref path);
            int lastSlash = path.LastIndexOf('/');
            IEnumerable<XElement> directoryRange = Database.GetElement(
                $"GileSystem{path.Substring(0,lastSlash == -1 ? path.Length : lastSlash)}").Elements().Where(
                x => x.Attribute("Type").Value == "Directory");

            for(int index = 0; index < directoryRange.Count(); index++)
                directories.Add(GetDirectory($"{path}/{directoryRange.ElementAt(index).Name.LocalName}"));

            return new MochaCollectionResult<MochaDirectory>(directories);
        }

        /// <summary>
        /// Return disk by root.
        /// </summary>
        /// <param name="root">Root of disk.</param>
        public MochaResult<MochaDisk> GetDisk(string root) {
            if(!ExistsDisk(root))
                return null;

            XElement diskElement = Database.GetElement($"FileSystem/{root}");
            MochaDisk disk = new MochaDisk(diskElement.Name.LocalName,diskElement.Attribute("Name").Value);
            disk.Description = diskElement.Attribute("Description").Value;
            disk.Directories.AddRange(GetDirectories(root).collection);
            disk.Files.AddRange(GetFiles(root).collection);

            return disk;
        }

        /// <summary>
        /// Return all disks.
        /// </summary>
        public MochaCollectionResult<MochaDisk> GetDisks() {
            List<MochaDisk> disks = new List<MochaDisk>();

            IEnumerable<XElement> diskRange = Database.GetElement("FileSystem").Elements().Where(
                x => x.Attribute("Type").Value=="Disk");
            for(int index = 0; index < diskRange.Count(); index++)
                disks.Add(GetDisk(diskRange.ElementAt(index).Name.LocalName));

            return new MochaCollectionResult<MochaDisk>(disks);
        }

        /// <summary>
        /// Add disk.
        /// </summary>
        /// <param name="disk">Disk to add.</param>
        public void AddDisk(MochaDisk disk) {
            if(ExistsDisk(disk.Root))
                throw new Exception("There is already a disk with this root!");

            XElement xDisk = new XElement(disk.Root);
            xDisk.Add(new XAttribute("Type","Disk"));
            xDisk.Add(new XAttribute("Name",disk.Name));
            xDisk.Add(new XAttribute("Description",disk.Description));

            Database.Doc.Root.Element("FileSystem").Add(xDisk);

            for(int index = 0; index < disk.Directories.Count; index++)
                AddDirectory(disk.Directories[index],disk.Root);

            if(disk.Directories.Count==0)
                Database.Save();
        }

        /// <summary>
        /// Create new empty disk.
        /// </summary>
        /// <param name="root">Root of disk.</param>
        /// <param name="name">Name of disk.</param>
        public void CreateDisk(string root,string name) =>
            AddDisk(new MochaDisk(root,name));

        /// <summary>
        /// Remove disk.
        /// </summary>
        /// <param name="root">Root of disk.</param>
        public void RemoveDisk(string root) {
            if(!ExistsDisk(root))
                return;

            XElement diskElement = Database.GetElement($"FileSystem/{root}").Elements().Where(x =>
            x.Attribute("Type").Value=="Disk").First();

            diskElement.Remove();
            Database.Save();
        }

        /// <summary>
        /// Add directory.
        /// </summary>
        /// <param name="directory">Directory to add.</param>
        /// <param name="path">Path to add.</param>
        public void AddDirectory(MochaDirectory directory,string path) {
            ProcessPath(ref path);
            string[] parts = path.Split('/');
            int lastSlash = path.LastIndexOf('/');

            if(!ExistsDisk(parts[0]))
                throw new Exception("Disk not found!");
            string directoryPath = path.Substring(0,lastSlash == -1 ? path.Length : lastSlash);
            if(parts.Length != 1 && !ExistsDirectory(directoryPath))
                throw new Exception("Directory not found!");
            if(ExistsDirectory(path))
                throw new Exception("This directory already exists!");

            XElement element = Database.GetElement(parts.Length == 1 ? "FileSystem" :
                $"FileSystem/{directoryPath}").Elements().Where(x =>
            x.Attribute("Type").Value=="Disk" || x.Attribute("Type").Value=="Directory").First();

            if(element==null)
                return;

            XElement xDirectory = new XElement(directory.Name);
            xDirectory.Add(new XAttribute("Type","Directory"));
            xDirectory.Add(new XAttribute("Description",directory.Description));
            element.Add(xDirectory);

            for(int index = 0; index < directory.Files.Count; index++)
                AddFile(directory.Files[index],path);

            for(int index = 0; index < directory.Directories.Count; index++)
                AddDirectory(directory.Directories[index],path);

            if(directory.Files.Count==0 || directory.Directories.Count==0)
                Database.Save();
        }

        /// <summary>
        /// Create new empty directory.
        /// </summary>
        /// <param name="path">Path to add.</param>
        /// <param name="name">Name of directory.</param>
        public void CreateDirectory(string path,string name) =>
            AddDirectory(new MochaDirectory(name),path);

        /// <summary>
        /// Remove directory.
        /// </summary>
        /// <param name="path">Path of directory to remove.</param>
        public void RemoveDirectory(string path) {
            ProcessPath(ref path);
            string[] parts = path.Split('/');
            int lastSlash = path.LastIndexOf('/');

            XElement element = Database.GetElement($"FileSystem/{path}").Elements().Where(x =>
            x.Attribute("Type").Value=="Directory").First();

            if(element==null)
                return;

            element.Remove();
            Database.Save();
        }

        /// <summary>
        /// Remove file.
        /// </summary>
        /// <param name="path">Path of file to remove.</param>
        public void RemoveFile(string path) {
            if(!ExistsFile(path))
                return;

            ProcessPath(ref path);
            XElement element = Database.GetElement($"FileSystem/{path}").Elements().Where(x =>
            x.Attribute("Type").Value=="File").First(); ;

            element.Remove();
            Database.Save();
        }

        /// <summary>
        /// Add file.
        /// </summary>
        /// <param name="file">File to add.</param>
        /// <param name="path">Path of directory to add.</param>
        public void AddFile(MochaFile file,string path) {
            ProcessPath(ref path);
            string[] parts = path.Split('/');
            int lastSlash = path.LastIndexOf('/');

            if(!ExistsDisk(parts[0]))
                throw new Exception("Disk not found!");

            string directoryPath = path.Substring(0,lastSlash == -1 ? path.Length : lastSlash);

            if(!ExistsDirectory(directoryPath))
                throw new Exception("Directory not found!");
            if(ExistsFile($"{path}/{file.FullName}"))
                throw new Exception("This file already exists!");

            lastSlash = directoryPath.LastIndexOf('/');
            XElement element = Database.GetElement(parts.Length == 1 ? "FileSystem"
                : $"FileSystem/{directoryPath.Substring(0,lastSlash == -1 ? directoryPath.Length : lastSlash)}").Elements().Where(x =>
            x.Attribute("Type").Value=="Directory").First();

            XElement xFile = new XElement(file.FullName,Convert.ToBase64String(file.Bytes));
            xFile.Add(new XAttribute("Type","File"));
            xFile.Add(new XAttribute("Description",file.Description));
            element.Add(xFile);
            Database.Save();
        }

        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="path">Path of file.</param>
        /// <param name="virtualPath">FileSystem path of file.</param>
        public void UploadFile(string path,string virtualPath) =>
            AddFile(MochaFile.Load(path),virtualPath);

        /// <summary>
        /// Returns whether there is a disk with the specified root.
        /// </summary>
        public MochaResult<bool> ExistsDisk(string root) =>
            Database.GetElement("FileSystem").Elements().Where(x => x.Attribute("Type").Value=="Disk" &&
            x.Name.LocalName==root).Count() > 0;

        /// <summary>
        /// Returns whether there is a directory with the specified path.
        /// </summary>
        public MochaResult<bool> ExistsDirectory(string path) {
            ProcessPath(ref path);
            int lastSlash = path.LastIndexOf('/');

            XElement element = Database.GetElement($"FileSystem/{path.Substring(0,lastSlash == -1 ? path.Length : lastSlash)}");
            return element == null ? false : element.Elements().Where(
                x => x.Attribute("Type").Value=="Directory").Count() > 0;
        }

        /// <summary>
        /// Returns whether there is a file with the specified path.
        /// </summary>
        public MochaResult<bool> ExistsFile(string path) {
            ProcessPath(ref path);
            int lastSlash = path.LastIndexOf('/');

            XElement element = Database.GetElement($"FileSystem/{path.Substring(0,lastSlash == -1 ? path.Length : lastSlash)}");
            return element == null ? false : element.Elements().Where(
                x => x.Attribute("Type").Value=="File").Count() > 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is embedded file system in database.
        /// </summary>
        public bool IsDatabaseEmbedded { get; private set; }

        /// <summary>
        /// MochaDatabase object to use file system processes.
        /// </summary>
        public MochaDatabase Database {
            get =>
                database;
            set {
                if(IsDatabaseEmbedded)
                    throw new Exception("This is embedded in database, can not set database!");

                if(database == null)
                    throw new Exception("This MochaDatabase is not affiliated with a database!");

                if(value == database)
                    return;

                database = value;
            }
        }

        #endregion
    }
}