namespace MochaDB.mhql.engine {
  using System.Collections.Generic;

  /// <summary>
  /// Lexer of MHQL conditions.
  /// </summary>
  internal static class MhqlEng_CONDITION_LEXER {
    /// <summary>
    /// Operators.
    /// </summary>
    public static Dictionary<string,string> Operators =>
      new Dictionary<string,string> {
        { "EQUAL", "==" },
        { "NOTEQUAL", "!=" },
        { "BIGGEREQ", ">=" },
        { "BIGGER", ">" },
        { "LOWEREQ", "<=" },
        { "LOWER", "<" }
      };
  }
}
