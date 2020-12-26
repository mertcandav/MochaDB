namespace MochaDB {
  using System;
  using System.Linq;

  using IOPath = System.IO.Path;

  /// <summary>
  /// MochaDB path definer.
  /// </summary>
  public struct MochaPath {
    #region Fields

    private string path;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new MochaPath.
    /// </summary>
    /// <param name="path">Path.</param>
    public MochaPath(string path) =>
      this.path=path;

    #endregion Constructors

    #region Operators

    public static implicit operator MochaPath(string value) =>
        new MochaPath(value);

    public static implicit operator string(MochaPath value) =>
        value.ToString();

    #endregion Operators

    #region Members

    /// <summary>
    /// Go to the parent directory.
    /// </summary>
    public void ParentDirectory() {
      int dex = Path.LastIndexOf(IOPath.DirectorySeparatorChar);
      Path=dex!=-1 ? Path.Substring(0,dex) : Path;
    }

    /// <summary>
    /// Return parent directory's MochaPath object.
    /// </summary>
    public MochaPath ParentPath() {
      int dex = Path.LastIndexOf(IOPath.DirectorySeparatorChar);
      string path = dex!=-1 ? Path.Substring(0,dex) : Path;
      return path;
    }

    /// <summary>
    /// Returns name of current directory or file.
    /// </summary>
    /// <returns></returns>
    public string Name() {
      int dex = Path.LastIndexOf(IOPath.DirectorySeparatorChar);
      return dex!=-1 ? Path.Remove(0,dex+1) : Path;
    }

    /// <summary>
    /// Returns true if the path is compatible with database paths, false if not.
    /// </summary>
    public bool IsDatabasePath() =>
        Path.StartsWith("Root") ||
        Path.StartsWith("Tables") ||
        Path.StartsWith("Logs");

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns <see cref="Path"/>.
    /// </summary>
    public override string ToString() =>
      Path;

    #endregion Overrides

    #region Static Properties

    /// <summary>
    /// Root path.
    /// </summary>
    public static MochaPath Root =>
        "Root";

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

    #endregion Static Properties

    #region Properties

    /// <summary>
    /// Path.
    /// </summary>
    public string Path {
      get => path;
      set {
        value=value.Trim();
        if(string.IsNullOrEmpty(value))
          throw new NullReferenceException("Path is cannot null or whitespace!");

        value=value.Replace('\\',IOPath.DirectorySeparatorChar);
        value = value.Last() == IOPath.DirectorySeparatorChar ? value.Remove(value.Length-1,1) : value;
        if(value==path)
          return;

        path=value;
      }
    }

    #endregion Properties
  }
}
