using MochaDB.Querying;

namespace MochaDB.FileSystem {
    /// <summary>
    /// Interface for MochaDB file systems.
    /// </summary>
    public interface IMochaFileSystem {
        #region Methods

        MochaCollectionResult<MochaFile> GetFiles(MochaPath path);
        MochaCollectionResult<MochaDirectory> GetDirectories(MochaPath path);
        MochaCollectionResult<MochaDisk> GetDisks();
        void AddDisk(MochaDisk disk);
        void AddDirectory(MochaDirectory directory,MochaPath path);
        void AddFile(MochaFile file,MochaPath path);
        bool RemoveDisk(string root);
        bool RemoveDirectory(MochaPath path);
        bool RemoveFile(MochaPath path);
        bool ExistsDisk(string root);
        bool ExistsDirectory(MochaPath path);
        bool ExistsFile(MochaPath path);

        #endregion

        #region Properties

        MochaDatabase Database { get; set; }

        #endregion
    }
}
