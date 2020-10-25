using System;

namespace MochaDB.Logging {
  /// <summary>
  /// Log for MochaDB.
  /// </summary>
  public struct MochaLog {
    #region Constructors

    /// <summary>
    /// Create a new MochaLog.
    /// </summary>
    /// <param name="log">Log content.</param>
    public MochaLog(string log) {
      ID = new MochaID(MochaIDType.Hash16);
      Log=log;
      Time =DateTime.Now;
    }

    /// <summary>
    /// Create a new MochaLog.
    /// </summary>
    /// <param name="log">Log content.</param>
    /// <param name="time">Log time.</param>
    public MochaLog(string log,DateTime time) :
        this(log) => Time=time;

    #endregion Constructors

    #region Operators

    public static implicit operator string(MochaLog value) =>
        value.Log;

    #endregion Operators

    #region Members

    /// <summary>
    /// Take a new MochaID by default algorithm.
    /// </summary>
    public void TakeID() =>
      ID.SetValue(MochaIDType.Hash16);

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns <see cref="Log"/>.
    /// </summary>
    public override string ToString() =>
      Log;

    #endregion Overrides

    #region Properties

    /// <summary>
    /// ID of log.
    /// </summary>
    public MochaID ID { get; set; }

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
