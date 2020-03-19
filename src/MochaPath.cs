using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaDB path definer.
    /// </summary>
    public struct MochaPath:IMochaPath {
        #region Fields

        private string path;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaPath.
        /// </summary>
        /// <param name="path">Path.</param>
        public MochaPath(string path) {
            this.path=path;
        }

        #endregion

        #region Operators

        public static implicit operator MochaPath(string value) =>
            new MochaPath(value);

        public static implicit operator string(MochaPath value) =>
            value.ToString();

        public static explicit operator MochaElement(MochaPath value) =>
            value.ToElement();

        #endregion

        #region Methods

        /// <summary>
        /// Go to the parent directory.
        /// </summary>
        public void ParentDirectory() {
            var dex = Path.LastIndexOf('/');
            Path=dex!=-1 ? Path.Substring(0,dex) : Path;
        }

        /// <summary>
        /// Return parent directory's MochaPath object.
        /// </summary>
        public MochaPath ParentPath() {
            var dex = Path.LastIndexOf('/');
            var path = dex!=-1 ? Path.Substring(0,dex) : Path;
            return path;
        }

        /// <summary>
        /// Returns name of current directory or file.
        /// </summary>
        /// <returns></returns>
        public string Name() {
            var dex = Path.LastIndexOf('/');
            return dex!=-1 ? Path.Remove(0,dex+1) : Path;
        }

        /// <summary>
        /// Returns true if the path is compatible with database paths, false if not.
        /// </summary>
        public bool IsDatabasePath() =>
            Path.StartsWith("Tables") ||
            Path.StartsWith("Sectors") ||
            Path.StartsWith("Stacks") ||
            Path.StartsWith("FileSystem");

        /// <summary>
        /// Returns MochaPath converted to MochaElement.
        /// </summary>
        public MochaElement ToElement() =>
            new MochaElement(new System.IO.FileInfo(Path).Name,string.Empty,Path);

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Path"/>.
        /// </summary>
        public override string ToString() {
            return Path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Path.
        /// </summary>
        public string Path {
            get =>
                path;
            set {
                value=value.TrimStart().TrimEnd();
                if(string.IsNullOrEmpty(value))
                    throw new MochaException("Path is cannot null or whitespace!");

                value=value.Replace('\\','/');
                value = value.Last()=='/' ? value.Remove(value.Length-1,1) : value;
                if(value==path)
                    return;

                path=value;
            }
        }

        #endregion
    }
}
