using System;

namespace MochaDB.FileSystem {
    /// <summary>
    /// Interface for MochaDB file system disks.
    /// </summary>
    public interface IMochaDisk {
        #region Events

        event EventHandler<EventArgs> NameChanged;
        event EventHandler<EventArgs> RootChanged;

        #endregion

        #region Properties

        string Root { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        MochaDirectoryCollection Directories { get; }
        MochaFileCollection Files { get; }

        #endregion
    }
}
