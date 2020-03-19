using System;
using MochaDB.FileSystem;
using MochaDB.Streams;

namespace MochaDB.Querying {
    /// <summary>
    /// Query extension for MochaFileSystems.
    /// </summary>
    public static class QueryingMochaFileSystem {
        /// <summary>
        /// Return all disks.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaDisk> GetDisks(this MochaFileSystem fs,Func<MochaDisk,bool> query) =>
            new MochaCollectionResult<MochaDisk>(fs.GetDisks().Where(query));

        /// <summary>
        /// Read all disks.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        public static MochaReader<MochaDisk> ReadDisks(this MochaFileSystem fs) =>
            new MochaReader<MochaDisk>(fs.GetDisks());

        /// <summary>
        /// Read all disks.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaDisk> ReadDisks(this MochaFileSystem fs,Func<MochaDisk,bool> query) =>
            new MochaReader<MochaDisk>(fs.GetDisks().Where(query));

        /// <summary>
        /// Returns all directories.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaDirectory> GetDirectories(this MochaFileSystem fs,MochaPath path,Func<MochaDirectory,bool> query) =>
            new MochaCollectionResult<MochaDirectory>(fs.GetDirectories(path).Where(query));

        /// <summary>
        /// Read all directories.
        /// </summary>
        /// <param name="path">Path of directory.</param>
        public static MochaReader<MochaDirectory> ReadDirectories(this MochaFileSystem fs,MochaPath path) =>
            new MochaReader<MochaDirectory>(fs.GetDirectories(path));

        /// <summary>
        /// Read all directories.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaDirectory> ReadDirectories(this MochaFileSystem fs,MochaPath path,Func<MochaDirectory,bool> query) =>
            new MochaReader<MochaDirectory>(fs.GetDirectories(path).Where(query));

        /// <summary>
        /// Returns all files.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaCollectionResult<MochaFile> GetFiles(this MochaFileSystem fs,MochaPath path,Func<MochaFile,bool> query) =>
            new MochaCollectionResult<MochaFile>(fs.GetFiles(path).Where(query));

        /// <summary>
        /// Read all files.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="path">Path of directory.</param>
        public static MochaReader<MochaFile> ReadFiles(this MochaFileSystem fs,MochaPath path) =>
            new MochaReader<MochaFile>(fs.GetFiles(path));

        /// <summary>
        /// Read all files.
        /// </summary>
        /// <param name="fs">Target filesystem.</param>
        /// <param name="path">Path of directory.</param>
        /// <param name="query">Query for filtering.</param>
        public static MochaReader<MochaFile> ReadFiles(this MochaFileSystem fs,MochaPath path,Func<MochaFile,bool> query) =>
            new MochaReader<MochaFile>(fs.GetFiles(path).Where(query));
    }
}
