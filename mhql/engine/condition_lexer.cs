namespace MochaDB.mhql.engine {
  using System.Collections.Generic;

  /// <summary>
  /// Lexer of MHQL conditions.
  /// </summary>
  internal static class MhqlEng_CONDITION_LEXER {
    /// <summary>
    /// Operators.
    /// </summary>
    public static Dictionary<string,string> __OPERATORS__ =>
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
    /// <summary>
    /// None.
    /// </summary>
    None = 0,
    /// <summary>
    /// Euqal operator.
    /// </summary>
    EQUAL = 1,
    /// <summary>
    /// Not equal operator.
    /// </summary>
    NOTEQUAL = 2,
    /// <summary>
    /// Bigger operator.
    /// </summary>
    BIGGER = 3,
    /// <summary>
    /// Lower operator.
    /// </summary>
    LOWER = 4,
    /// <summary>
    /// Bigger or equal operator.
    /// </summary>
    BIGGEREQ = 5,
    /// <summary>
    /// Lower or equal operator.
    /// </summary>
    LOWEREQ = 6
  }
}
