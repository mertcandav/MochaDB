using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MochaDB.mhql.keywords {
  /// <summary>
  /// MHQL AND keyword.
  /// </summary>
  internal class Mhql_AND {
    /// <summary>
    /// Returns seperated commands by or.
    /// </summary>
    /// <param name="command">Command.</param>
    public static List<string> GetParts(string command) {
      Regex pattern = new Regex("AND",
          RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
      List<string> parts = new List<string>();
      StringBuilder value = new StringBuilder();
      int count = 0;
      for(int index = 0; index < command.Length; ++index) {
        char? currentChar = command[index];
        if(count == 0 && (currentChar == 'A' || currentChar == 'a')) {
          if(command.Length - 1 - index >= 3)
            if(pattern.IsMatch(command.Substring(index,3))) {
              parts.Add(value.ToString().Trim());
              value.Clear();
              index+=2;
              continue;
            }
        } else if(currentChar == Mhql_LEXER.LPARANT) ++count;
        else if(currentChar == Mhql_LEXER.RPARANT) --count;
        else if(currentChar == Mhql_LEXER.LBRACE) ++count;
        else if(currentChar == Mhql_LEXER.RBRACE) --count;
        else if(currentChar == '\'' || currentChar == '"') {
          ++index;
          value.Append(currentChar);
          for(; index < command.Length; ++index) {
            char currentCh = command[index];
            value.Append(currentCh);
            if(currentCh == currentChar && command[index - 1] != '\\') {
              currentChar = null;
              break;
            }
          }
          if(currentChar != null)
            throw new MochaException("Error in char/string declaration!");
          continue;
        }
        value.Append(currentChar);
      }
      parts.Add(value.ToString().Trim());
      return parts;
    }
  }
}
