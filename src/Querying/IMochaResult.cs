namespace MochaDB.Querying {
  /// <summary>
  /// Interface for MochaDB results.
  /// </summary>
  public interface IMochaResult {
    #region Properties

    bool IsCollectionResult { get; }

    #endregion Properties
  }

  /// <summary>
  /// Iterface for MochaDB value results.
  /// </summary>
  /// <typeparam name="T">Value type.</typeparam>
  public interface IMochaResult<T>:IMochaResult {
    #region Members

    object GetObject();
    int GetHash();

    #endregion Members

    #region Properties

    T Value { get; }

    #endregion Properties
  }
}