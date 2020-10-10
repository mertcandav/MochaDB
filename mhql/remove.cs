using System;

namespace MochaDB.mhql {
  /// <summary>
  /// MHQL REMOVE keyword.
  /// </summary>
  internal class Mhql_REMOVE:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Create a new Mhql_REMOVE.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_REMOVE(MochaDatabase db) {
      Command = string.Empty;
      Tdb = db;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns true if command is remove, return false if not.
    /// </summary>
    public bool IsRemoveCmd() =>
        Command.EndsWith("REMOVE",StringComparison.OrdinalIgnoreCase);

    #endregion
  }
}
