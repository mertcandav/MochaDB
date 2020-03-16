namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB path definers.
    /// </summary>
    public interface IMochaPath {
        #region Methods

        void ParentDirectory();
        bool IsDatabasePath();

        #endregion

        #region Properties

        string Path { get; set; }

        #endregion
    }
}
