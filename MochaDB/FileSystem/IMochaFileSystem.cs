using MochaDB.Querying;

namespace MochaDB.FileSystem {
    /// <summary>
    /// Interface for MochaDB file systems.
    /// </summary>
    public interface IMochaFileSystem {
        #region Methods

        MochaCollectionResult<MochaFile> GetFiles(string path);
        MochaCollectionResult<MochaDirectory> GetDirectories(string path);
        MochaCollectionResult<MochaDisk> GetDisks();
        void AddDisk(MochaDisk disk);
        void AddDirectory(MochaDirectory directory,string path);
        void AddFile(MochaFile file,string path);
        void RemoveDisk(string root);
        void RemoveDirectory(string path);
        void RemoveFile(string path);
        MochaResult<bool> ExistsDisk(string root);
        MochaResult<bool> ExistsDirectory(string path);
        MochaResult<bool> ExistsFile(string path);

        #endregion

        #region Properties

        MochaDatabase Database { get; set; }

        #endregion
    }
}