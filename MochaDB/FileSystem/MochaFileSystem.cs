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
using System.Linq;
using System.Xml.Linq;
using MochaDB.Querying;
using MochaDB.Streams;

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

        #region Disk

        /// <summary>
        /// Remove all disks.
        /// </summary>
        public void ClearDisks() {
            Database.OnConnectionCheckRequired(this,new EventArgs());
            Database.OnChanging(this,new EventArgs());

            Database.Doc.Root.Element("FileSystem").RemoveNodes();

            Database.Save();
        }

        /// <summary>
        /// Return disk by root.
        /// </summary>
        /// <param name="root">Root of disk.</param>
        public MochaResult<MochaDisk> GetDisk(string root) {
            if(!ExistsDisk(root))
                return null;

            var diskElement = Database.GetElement($"FileSystem/{root}");
            var disk = new MochaDisk(diskElement.Name.LocalName,diskElement.Attribute("Name").Value);
            disk.Description = diskElement.Attribute("Description").Value;
            disk.Directories.AddRange(GetDirectories(root).collection);
            disk.Files.AddRange(GetFiles(root).collection);

            return disk;
        }

        /// <summary>
        /// Return all disks.
        /// </summary>
        public MochaCollectionResult<MochaDisk> GetDisks() {
            var disks = new List<MochaDisk>();

            var diskRange = Database.GetElement("FileSystem").Elements().Where(
                x => x.Attribute("Type").Value=="Disk");
            for(int index = 0; index < diskRange.Count(); index++)
                disks.Add(GetDisk(diskRange.ElementAt(index).Name.LocalName));

            return new MochaCollectionResult<MochaDisk>(disks);
        }

        /// <summary>
        /// Return all disks.
        /// </summary>
        /// <param name="query">Query for filtering.</param>
        public MochaCollectionResult<MochaDisk> GetDisks(Func<MochaDisk,bool> query) =>
            new MochaCollectionResult<MochaDisk>(GetDisks().collection.Where(query));

        /// <summary>
        /// Read all disks.
        /// </summary>
        public MochaReader<MochaDisk> ReadDisks() =>
            new MochaReader<MochaDisk>(GetDisks().collection);

        /// <summary>
        /// Read all disks.
        /// </summary>
        /// <param name="query">Query for filtering.</param>
        public MochaReader<MochaDisk> ReadDisks(Func<MochaDisk,bool> query) =>
            new MochaReader<MochaDisk>(GetDisks().collection.Where(query));

        /// <summary>
        /// Add disk.
        /// </summary>
        /// <param name="disk">Disk to add.</param>
        public void AddDisk(MochaDisk disk) {
            if(ExistsDisk(disk.Root))
                throw new Exception("There is already a disk with this root!");
            Database.OnChanging(this,new EventArgs());

            var xDisk = new XElement(disk.Root);
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
        /// Remove disk. Returns true if disk is exists and removed.
        /// </summary>
        /// <param name="root">Root of disk.</param>
        public bool RemoveDisk(string root) {
            if(!ExistsDisk(root))
                return false;
            Database.OnChanging(this,new EventArgs());

            var diskElement = Database.GetElement($"FileSystem/{root}");

            diskElement.Remove();
            Database.Save();
            return true;
        }

        /// <summary>
        /// Returns whether there is a disk with the specified root.
        /// </summary>
        public MochaResult<bool> ExistsDisk(string root) =>
            Database.GetElement("FileSystem").Elements().Select(x => x.Name.LocalName==root).Count() > 0;

        #endregion

        #region Directory

        #region Internal

        /// <summary>
        /// Return directory xml element by path.
        /// </summary>
        /// <param name="path">Path of xml element.</param>
        internal XElement GetDirectoryElement(MochaPath path) {
            var originalname = path.Name();
            path = path.ParentPath();
            var elements = Database.GetElement($"FileSystem/{path.Path}").Elements().Where(x =>
            x.Attribute("Type").Value=="Directory");
            return elements.Count()==0 ? null : elements.First();
        }

        #endregion

        /// <summary>
        /// Return directory by path.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaResult<MochaDirectory> GetDirectory(MochaPath path) {
            if(!ExistsDirectory(path))
                return null;

            var originalpath = path.Path;
            var directoryElement = GetDirectoryElement(path);
            var directory = new MochaDirectory(directoryElement.Name.LocalName);
            directory.Description=directoryElement.Attribute("Description").Value;
            directory.Files.AddRange(GetFiles(originalpath).collection);
            directory.Directories.AddRange(GetDirectories(originalpath).collection);

            return directory;
        }

        /// <summary>
        /// Return all directories.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaCollectionResult<MochaDirectory> GetDirectories(MochaPath path) {
            var directories = new List<MochaDirectory>();
            if(!ExistsDisk(path.Path) && !ExistsDirectory(path))
                return new MochaCollectionResult<MochaDirectory>(directories);

            var directoryRange = Database.GetElement(
                $"FileSystem/{path.Path}").Elements().Where(
                x => x.Attribute("Type").Value == "Directory");

            for(int index = 0; index < directoryRange.Count(); index++) {
                MochaDirectory directory = GetDirectory($"{path.Path}/{directoryRange.ElementAt(index).Name.LocalName}");
                if(directory==null)
                    continue;

                directories.Add(directory);
            }

            return new MochaCollectionResult<MochaDirectory>(directories);
        }

        /// <summary>
        /// Return all directories.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public MochaCollectionResult<MochaDirectory> GetDirectories(MochaPath path,Func<MochaDirectory,bool> query) =>
            new MochaCollectionResult<MochaDirectory>(GetDirectories(path).collection.Where(query));

        /// <summary>
        /// Read all directories.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaReader<MochaDirectory> ReadDirectories(MochaPath path) =>
            new MochaReader<MochaDirectory>(GetDirectories(path).collection);

        /// <summary>
        /// Read all directories.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public MochaReader<MochaDirectory> ReadDirectories(MochaPath path,Func<MochaDirectory,bool> query) =>
            new MochaReader<MochaDirectory>(GetDirectories(path).collection.Where(query));

        /// <summary>
        /// Add directory.
        /// </summary>
        /// <param name="directory">Directory to add.</param>
        /// <param name="path">Path to add.</param>
        public void AddDirectory(MochaDirectory directory,MochaPath path) {
            var parts = path.Path.Split('/');

            if(!ExistsDisk(parts[0]))
                throw new Exception("Disk not found!");
            if(parts.Length != 1 && !ExistsDirectory(path.Path))
                throw new Exception("Directory not found!");
            if(ExistsDirectory($"{path.Path}/{directory.Name}"))
                throw new Exception("This directory already exists!");
            Database.OnChanging(this,new EventArgs());

            var originalname = path.Name();
            path = path.ParentPath();
            var element = Database.GetElement(parts.Length == 1 ? "FileSystem" :
                $"FileSystem/{path.Path}").Elements().Where(x =>
            (x.Attribute("Type").Value=="Disk" || x.Attribute("Type").Value=="Directory") &&
            x.Name.LocalName==originalname).First();

            if(element==null)
                return;

            var xDirectory = new XElement(directory.Name);
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
        public void CreateDirectory(MochaPath path,string name) =>
            AddDirectory(new MochaDirectory(name),path);

        /// <summary>
        /// Remove directory. Returns true if directory is exists and removed.
        /// </summary>
        /// <param name="path">Path of directory to remove.</param>
        public bool RemoveDirectory(MochaPath path) {
            Database.OnChanging(this,new EventArgs());

            var element = GetDirectoryElement(path);
            if(element == null)
                return false;

            element.Remove();
            Database.Save();
            return true;
        }

        /// <summary>
        /// Returns whether there is a directory with the specified path.
        /// </summary>
        public MochaResult<bool> ExistsDirectory(MochaPath path) {
            return GetDirectoryElement(path) != null;
        }

        #endregion

        #region File

        /// <summary>
        /// Return file by path.
        /// </summary>
        /// <param name="path">path of file.</param>
        public MochaResult<MochaFile> GetFile(MochaPath path) {
            if(!ExistsFile(path))
                return null;

            var originalname = path.Name();
            path= path.ParentPath();
            var fileElement = Database.GetElement($"FileSystem/{path.Path}").Elements().Where(x =>
            x.Attribute("Type").Value=="File" && x.Name.LocalName==originalname).First();

            var nameParts = fileElement.Name.LocalName.Split('.');
            var file = new MochaFile(nameParts.First(),nameParts.Length==1 ? string.Empty : nameParts.Last());
            file.Description=fileElement.Attribute("Description").Value;
            file.Stream.Bytes=Convert.FromBase64String(fileElement.Value);

            return file;
        }

        /// <summary>
        /// Return all files.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaCollectionResult<MochaFile> GetFiles(MochaPath path) {
            var files = new List<MochaFile>();
            if(!ExistsDirectory(path))
                return new MochaCollectionResult<MochaFile>(files);

            var fileRange = Database.GetElement($"FileSystem/{path.Path}").Elements().Where(
                x => x.Attribute("Type").Value=="File");
            for(int index = 0; index < fileRange.Count(); index++) {
                MochaFile file = GetFile($"{path.Path}/{fileRange.ElementAt(index).Name.LocalName}");
                files.Add(file);
            }

            return new MochaCollectionResult<MochaFile>(files);
        }

        /// <summary>
        /// Return all files.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public MochaCollectionResult<MochaFile> GetFiles(MochaPath path,Func<MochaFile,bool> query) =>
            new MochaCollectionResult<MochaFile>(GetFiles(path).collection.Where(query));

        /// <summary>
        /// Read all files.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public MochaReader<MochaFile> ReadFiles(MochaPath path) =>
            new MochaReader<MochaFile>(GetFiles(path).collection);

        /// <summary>
        /// Read all files.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public MochaReader<MochaFile> ReadFiles(MochaPath path,Func<MochaFile,bool> query) =>
            new MochaReader<MochaFile>(GetFiles(path).collection.Where(query));

        /// <summary>
        /// Remove file. Returns true if file is exists and removed.
        /// </summary>
        /// <param name="path">Path of file to remove.</param>
        public bool RemoveFile(MochaPath path) {
            if(!ExistsFile(path))
                return false;
            Database.OnChanging(this,new EventArgs());

            var originalname = path.Name();
            path= path.ParentPath();
            Database.GetElement($"FileSystem/{path.Path}").Elements().Where(x =>
                x.Attribute("Type").Value=="File" && x.Name.LocalName==originalname).First().Remove();

            Database.Save();
            return true;
        }

        /// <summary>
        /// Add file.
        /// </summary>
        /// <param name="file">File to add.</param>
        /// <param name="path">Path of directory to add.</param>
        public void AddFile(MochaFile file,MochaPath path) {
            var parts = path.Path.Split('/');

            if(parts.Length == 1)
                throw new Exception("Files is cannot add in disks directly!");
            if(!ExistsDisk(parts[0]))
                throw new Exception("Disk not found!");
            if(!ExistsDirectory(path))
                throw new Exception("Directory not found!");
            if(ExistsFile($"{path}/{file.FullName}"))
                throw new Exception("This file already exists!");
            Database.OnChanging(this,new EventArgs());

            var originalname = path.Name();
            path = path.ParentPath();
            var element = Database.GetElement($"FileSystem/{path.Path}").Elements().Where(x =>
                    x.Attribute("Type").Value=="Directory").First();

            var xFile = new XElement(file.FullName,Convert.ToBase64String(file.Stream.Bytes));
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
        public void UploadFile(string path,MochaPath virtualPath) =>
            AddFile(MochaFile.Load(path),virtualPath);

        /// <summary>
        /// Returns whether there is a file with the specified path.
        /// </summary>
        public MochaResult<bool> ExistsFile(MochaPath path) {
            var originalname = path.Name();
            path= path.ParentPath();
            return Database.GetElement($"FileSystem/{path.Path}").Elements().Where(x =>
                x.Attribute("Type").Value=="File" && x.Name.LocalName==originalname).Count() > 0;
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
