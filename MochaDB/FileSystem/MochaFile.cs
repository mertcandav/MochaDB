using System;
using System.IO;
using System.Text;
using MochaDB.Streams;

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
            Stream=new MochaStream();
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaFile value) =>
            value.ToString();

        #endregion

        #region Events

        /// <summary>
        /// This happens after name changed.
        /// </summary>
        public event EventHandler<EventArgs> NameChanged;
        private void OnNameChanged(object sender,EventArgs e) {
            if(string.IsNullOrEmpty(FullName))
                throw new NullReferenceException("At least one of the names and extensions must not be empty!");

            //Invoke.
            NameChanged?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// This happens after extension changed.
        /// </summary>
        public event EventHandler<EventArgs> ExtensionChanged;
        private void OnExtensionChanged(object sender,EventArgs e) {
            if(string.IsNullOrEmpty(FullName))
                throw new NullReferenceException("At least one of the names and extensions must not be empty!");

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
            file.Stream.Bytes = File.ReadAllBytes(path);
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
            Stream=file.Stream;
        }

        /// <summary>
        /// Set data from string with UTF8.
        /// </summary>
        /// <param name="value">String value.</param>
        public void SetFromString(string value) {
            Stream.Bytes=Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Set data from string with encoding.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="encoding">Encoding to use.</param>
        public void SetFromString(string value,Encoding encoding) {
            Stream.Bytes=encoding.GetBytes(value);
        }

        /// <summary>
        /// Set data from stream.
        /// </summary>
        /// <param name="stream">Stream to use.</param>
        public void SetFromStream(MemoryStream stream) {
            Stream.Bytes=stream.ToArray();
        }

        /// <summary>
        /// Returns bytes converted to Base64.
        /// </summary>
        public string ToBase64() =>
            Convert.ToBase64String(Stream.Bytes);

        /// <summary>
        /// Returns bytes converted to MemoryStream.
        /// </summary>
        public MemoryStream ToStream() =>
            new MemoryStream(Stream.Bytes);

        /// <summary>
        /// Returns bytes converted to text with UTF8.
        /// </summary>
        public string ToText() =>
            Encoding.UTF8.GetString(Stream.Bytes);

        /// <summary>
        /// Returns bytes converted to text with encoding.
        /// </summary>
        /// <param name="encoding">Encoding to use with converting.</param>
        public string ToText(Encoding encoding) =>
            encoding.GetString(Stream.Bytes);

        #endregion

        #region Overrides

        /// <summary>
        /// Execute <see cref="ToText()"/> function and returns result.
        /// </summary>
        public override string ToString() {
            return ToText();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of file.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                value=value.TrimStart().TrimEnd();
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
                value=value.TrimStart().TrimEnd();
                if(!string.IsNullOrEmpty(value))
                    value = value[0] != '.' ? $".{value}" : value;
                if(value==extension)
                    return;

                extension=value;
                OnExtensionChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// File value.
        /// </summary>
        public MochaStream Stream { get; set; }

        /// <summary>
        /// Description of file.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
