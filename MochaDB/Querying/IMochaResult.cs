namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB results.
    /// </summary>
    public interface IMochaResult {
        #region Properties

        public bool IsCollectionResult { get; }

        #endregion
    }
}