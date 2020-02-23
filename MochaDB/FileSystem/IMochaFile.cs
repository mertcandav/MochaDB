using System;

namespace MochaDB.FileSystem {
    /// <summary>
    /// Interface for MochaDB file system files.
    /// </summary>
    public interface IMochaFile {
        #region Events

        event EventHandler<EventArgs> NameChanged;
        event EventHandler<EventArgs> ExtensionChanged;

        #endregion

        #region Methods

        void From(string path);

        #endregion

        #region Properties

        string Name { get; set; }
        string FullName { get; }
        string Extension { get; set; }
        string Description { get; set; }
        byte[] Bytes { get; set; }

        #endregion
    }
}