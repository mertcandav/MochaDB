using System;
using System.IO;
using System.Text;

namespace MochaDB.FileSystem {
    /// <summary>
    /// File for MochaDB file system.
    /// </summary>
    public class MochaFile:IMochaFile {
        #region Fields

        private string name;
        private string extension;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaFile.
        /// </summary>
        /// <param name="name">Name of file.</param>
        /// <param name="extension">Extension of file.</param>
        public MochaFile(string name,string extension) {
            Name=name;
            Extension=extension;
            Description=string.Empty;
            Bytes=new byte[0];
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens after name changed.
        /// </summary>
        public event EventHandler<EventArgs> NameChanged;
        private void OnNameChanged(object sender,EventArgs e) {
            //Invoke.
            NameChanged?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// This happens after extension changed.
        /// </summary>
        public event EventHandler<EventArgs> ExtensionChanged;
        private void OnExtensionChanged(object sender,EventArgs e) {
            //Invoke.
            ExtensionChanged?.Invoke(this,new EventArgs());
        }

        #endregion

        #region Static

        /// <summary>
        /// Return file from path.
        /// </summary>
        /// <param name="path">Path of target file.</param>
        public static MochaFile Load(string path) {
            FileInfo fi = new FileInfo(path);
            if(!fi.Exists)
                throw new Exception("This path does not show a file!");
            
            MochaFile file = new MochaFile(fi.Name,fi.Extension);
            file.Bytes = File.ReadAllBytes(path);
            return file;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set datas by file.
        /// </summary>
        /// <param name="path">Path of target file.</param>
        public void From(string path) {
            MochaFile file = Load(path);
            Name=file.Name;
            Extension=file.Extension;
            Bytes=file.Bytes;
        }

        /// <summary>
        /// Returns bytes converted to Base64.
        /// </summary>
        public string ToBase64() =>
            Convert.ToBase64String(Bytes);

        /// <summary>
        /// Returns bytes converted to MemoryStream.
        /// </summary>
        /// <returns></returns>
        public MemoryStream ToStream() =>
            new MemoryStream(Bytes);

        /// <summary>
        /// Returns bytes converted to text with UTF8.
        /// </summary>
        public string ToText() =>
            Encoding.UTF8.GetString(Bytes);

        /// <summary>
        /// Returns bytes converted to text with encoding.
        /// </summary>
        /// <param name="encoding">Encoding to use with converting.</param>
        public string ToText(Encoding encoding) =>
            encoding.GetString(Bytes);

        #endregion

        #region Properties

        /// <summary>
        /// Name of file.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                value = string.IsNullOrWhiteSpace(value) ? string.Empty : value;

                if(value==name)
                    return;

                name=value;
                OnNameChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Name and extension of file.
        /// </summary>
        public string FullName =>
            $"{Name}{Extension}";

        /// <summary>
        /// Extension of file.
        /// </summary>
        public string Extension {
            get =>
                extension;
            set {
                if(value==extension)
                    return;

                extension=value;
                OnExtensionChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// File value.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Description of file.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}