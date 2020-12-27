namespace MochaDB.Logging {
  using System;

  /// <summary>
  /// Log for MochaDB.
  /// </summary>
  public struct MochaLog {
    #region Constructors

    /// <summary>
    /// Create a new instance of <see cref="MochaLog"/>.
    /// </summary>
    /// <param name="log">Log content.</param>
    /// <param name="time">Log time.</param>
    public MochaLog(string id,string log,DateTime time) {
      Id = id;
      Log = log;
      Time = time;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// ID of log.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Log content.
    /// </summary>
    public string Log { get; set; }

    /// <summary>
    /// Log time.
    /// </summary>
    public DateTime Time { get; set; }

    #endregion Properties
  }
}
