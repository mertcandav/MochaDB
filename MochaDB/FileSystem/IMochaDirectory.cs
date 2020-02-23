using System;

namespace MochaDB.FileSystem {
    /// <summary>
    /// Interface for MochaDB file system directorys.
    /// </summary>
    public interface IMochaDirectory {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        string Name { get; set; }
        string Description { get; set; }
        MochaFileCollection Files { get; }
        MochaDirectoryCollection Directories { get; }

        #endregion
    }
}