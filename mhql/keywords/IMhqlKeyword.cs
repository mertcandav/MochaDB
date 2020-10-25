namespace MochaDB.mhql.keywords {
  /// <summary>
  /// Interface for Mhql keywords.
  /// </summary>
  internal interface IMhqlKeyword {
    #region Properties

    /// <summary>
    /// Target database.
    /// </summary>
    MochaDatabase Tdb { get; set; }

    /// <summary>
    /// MHQL Command.
    /// </summary>
    string Command { get; set; }

    #endregion Properties
  }
}
