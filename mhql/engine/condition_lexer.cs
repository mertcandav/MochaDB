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

  /// <summary>
  /// Condition type.
  /// </summary>
  internal enum ConditionType {
    None = 0,
    Equal = 1,
    NotEqual = 2,
    Bigger = 3,
    Lower = 4,
    BiggerEqual = 5,
    LowerEqual = 6
  }
}
