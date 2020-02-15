namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB collection results.
    /// </summary>
    public interface IMochaCollectionResult<T> {
        #region Methods

        public int MaxIndex();

        #endregion

        #region Properties

        public T this[int index] { get; }
        public int Count { get; }

        #endregion
    }
}
