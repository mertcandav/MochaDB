namespace MochaDB {
  using System;

  /// <summary>
  /// Exception for MochaDB.
  /// </summary>
  public class MochaException:Exception {
    #region Constructors

    /// <summary>
    /// Create a new MochaException.
    /// </summary>
    public MochaException() =>
      Message=string.Empty;

    /// <summary>
    /// Create a new MochaException.
    /// </summary>
    /// <param name="msg">Message of exception.</param>
    public MochaException(string msg) =>
      Message=msg;

    #endregion Constructors

    #region Members

    /// <summary>
    /// Set exception message.
    /// </summary>
    /// <param name="msg">Message to set.</param>
    public virtual void SetMessage(string msg) =>
        Message = msg;

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns <see cref="Message"/>
    /// </summary>
    public override string ToString() =>
      Message;

    #endregion

    #region Properties

    /// <summary>
    /// Message of exception.
    /// </summary>
    public new virtual string Message { get; set; }

    #endregion Properties
  }
}
