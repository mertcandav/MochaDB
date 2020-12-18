namespace MochaDB.mhql.engine {
  using System.Text.RegularExpressions;

  using MochaDB.Querying;

  /// <summary>
  /// MHQL command editor.
  /// </summary>
  internal class MhqlEng_EDITOR {
    /// <summary>
    /// Decompose command ares by brackets.
    /// </summary>
    /// <param name="command">Command.</param>
    public static void DecomposeBrackets(ref string command) {
      if(command.FirstChar() == '(') {
        if(command.LastChar() != '(')
          throw new MochaException("Bracket is open but not closed!");
        command = command.Substring(1,command.Length-1);
      }
    }

    /// <summary>
    /// Returns decompose command ares by brackets.
    /// </summary>
    /// <param name="value">Value to decompose.</param>
    public static string DecomposeBrackets(string value) {
      int dex;
      if((dex = value.IndexOf('(')) != -1)
        return value.Substring(dex+1,value.Length-dex-2);
      return value;
    }

    /// <summary>
    /// Remove all comments from code.
    /// </summary>
    /// <param name="command">Command.</param>
    public static void RemoveComments(ref string command) {
      Regex multiline = new Regex(@"/\*.*?\*/",RegexOptions.Singleline);
      command = multiline.Replace(command,string.Empty);
      Regex singleline = new Regex(@"//.*$",RegexOptions.Multiline);
      command = singleline.Replace(command,string.Empty);
      command = command.Trim();
    }
  }
}
