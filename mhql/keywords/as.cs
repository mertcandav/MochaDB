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
    public static string GetAS(ref string command) {
      int dex = command.IndexOf(" AS ",StringComparison.OrdinalIgnoreCase);
      if(dex==-1)
        return command;
      string name = command.Substring(dex + 4);
      command = command.Substring(0, dex);
      return name;
    }
  }
}
