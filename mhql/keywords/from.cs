using System;
using System.Text.RegularExpressions;

namespace MochaDB.mhql.keywords {
  /// <summary>
  /// MHQL FROM keyword.
  /// </summary>
  internal class Mhql_FROM:MhqlKeyword {
    #region Methods

    /// <summary>
    /// Returns true if command is FROM style, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public static bool IsFROM(string command) =>
        GetIndex(ref command) != -1;

    /// <summary>
    /// Returns index of FROM keyword.
    /// </summary>
    /// <param name="command">Command.</param>
    public static int GetIndex(ref string command) {
      string result = new Regex(@"\s").Replace(command," ");
      int dex =
        command.StartsWith("*FROM",StringComparison.OrdinalIgnoreCase) ?
          1 : result.LastIndexOf(" FROM ",StringComparison.OrdinalIgnoreCase);
      return dex;
    }

    #endregion
  }
}
