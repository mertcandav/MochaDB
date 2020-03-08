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
        void RemoveDisk(string root);
        void RemoveDirectory(MochaPath path);
        void RemoveFile(MochaPath path);
        MochaResult<bool> ExistsDisk(string root);
        MochaResult<bool> ExistsDirectory(MochaPath path);
        MochaResult<bool> ExistsFile(MochaPath path);

        #endregion

        #region Properties

        MochaDatabase Database { get; set; }

        #endregion
    }
}