using System.Linq;

namespace MochaDB {
    /// <summary>
    /// MochaDB path definer.
    /// </summary>
    public struct MochaPath {
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
            Path.StartsWith("Root") ||
            Path.StartsWith("Tables") ||
            Path.StartsWith("Sectors") ||
            Path.StartsWith("Stacks");

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Path"/>.
        /// </summary>
        public override string ToString() {
            return Path;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Root path.
        /// </summary>
        public static MochaPath Root =>
            "Root";

        /// <summary>
        /// Sectors path.
        /// </summary>
        public static MochaPath Sectors =>
            "Sectors";

        /// <summary>
        /// Stacks path.
        /// </summary>
        public static MochaPath Stacks =>
            "Stacks";

        /// <summary>
        /// Tables path.
        /// </summary>
        public static MochaPath Tables =>
            "Tables";

        /// <summary>
        /// Logs path.
        /// </summary>
        public static MochaPath Logs =>
            "Logs";

        #endregion

        #region Properties

        /// <summary>
        /// Path.
        /// </summary>
        public string Path {
            get =>
                path;
            set {
                value=value.Trim();
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
