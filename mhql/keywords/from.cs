namespace MochaDB.mhql.keywords {
  using System.Text.RegularExpressions;

  /// <summary>
  /// MHQL FROM keyword.
  /// </summary>
  internal class Mhql_FROM:MhqlKeyword {
    #region Members

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
      Regex pattern = new Regex($@"(\*| |\n)FROM(\s+.*|$)",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
      int count = command.StartsWith($"{Mhql_LEXER.LBRACE}") ? 1 : 0;
      for(int index = count; index < command.Length; ++index) {
        char currentChar = command[index];
        if(currentChar == Mhql_LEXER.LPARANT || currentChar == Mhql_LEXER.LBRACE)
          ++count;
        else if(currentChar == Mhql_LEXER.RPARANT || currentChar == Mhql_LEXER.RBRACE)
          --count;
        if(count == 0) {
          Match match = pattern.Match(command.Substring(index));
          if(match.Success && match.Index == 0)
            return command[0] == Mhql_LEXER.ALL_OPERATOR ? index + 1 : index;
        }
      }
      return -1;
    }

    #endregion Members
  }
}
