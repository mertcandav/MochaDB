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
      Regex pattern = new Regex($@"(\*| |\n)FROM(\s+.*|$)",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
      int index = command.IndexOf(Mhql_LEXER.LBRACE);
      index = index == -1 ? command.IndexOf(Mhql_LEXER.RBRACE) : index;
      index = index == -1 ? 0 : index;
      int count = index == 0 ? 0 : 1;
      for(; index < command.Length; index++) {
        char currentChar = command[index];
        if(count == 0) {
          Match match = pattern.Match(command.Substring(index));
          if(match.Success)
            return command[match.Index] == Mhql_LEXER.ALL_OPERATOR ?
              match.Index + 1 : match.Index;
        } else if(currentChar == Mhql_LEXER.LPARANT || currentChar == Mhql_LEXER.LBRACE)
          count++;
        else if(currentChar == Mhql_LEXER.RPARANT || currentChar == Mhql_LEXER.RBRACE)
          count--;
      }
      return -1;
    }

    #endregion
  }
}
