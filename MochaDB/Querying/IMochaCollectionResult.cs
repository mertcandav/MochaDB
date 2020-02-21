namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB collection results.
    /// </summary>
    public interface IMochaCollectionResult<T>:IMochaQueryableCollection<T>, IMochaResult {
        #region Methods

        int MaxIndex();

        #endregion

        #region Properties

        MochaResult<T> this[int index] { get; }
        int Count { get; }

        #endregion
    }
}
