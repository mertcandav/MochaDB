namespace MochaDB.mhql.keywords {
  using System;

  /// <summary>
  /// MHQL AS keyword.
  /// </summary>
  internal class Mhql_AS {
    /// <summary>
    /// Returns as name.
    /// </summary>
    /// <param name="command">Command</param>
    /// <param name="final">As removed command.</param>
    public static string GetAS(ref string command) {
      int dex = command.IndexOf(" AS ",StringComparison.OrdinalIgnoreCase);
      if(dex==-1)
        return command;
      return
        command.Substring(dex+3).TrimStart().Substring(0,dex).Trim();
    }
  }
}
