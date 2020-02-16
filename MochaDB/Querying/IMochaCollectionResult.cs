namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB collection results.
    /// </summary>
    public interface IMochaCollectionResult<T>:IMochaQueryableCollection<T>,IMochaResult {
        #region Methods

        public int MaxIndex();

        #endregion

        #region Properties

        public MochaResult<T> this[int index] { get; }
        public int Count { get; }

        #endregion
    }
}
