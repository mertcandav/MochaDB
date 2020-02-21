namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB results.
    /// </summary>
    public interface IMochaResult {
        #region Properties

        bool IsCollectionResult { get; }

        #endregion
    }
}