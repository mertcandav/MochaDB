namespace MochaDB.Streams {
    /// <summary>
    /// Interface for MochaDB readers.
    /// </summary>
    public interface IMochaReader<T> {
        #region Methods

        bool Read();

        #endregion

        #region Properties

        object Value { get; }
        int Position { get; }
        int Count { get; }

        #endregion
    }
}
