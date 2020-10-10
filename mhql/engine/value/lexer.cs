namespace MochaDB.mhql.engine.value {
  /// <summary>
  /// Lexer of MHQL values.
  /// </summary>
  internal static class MhqlEngVal_LEXER {
    /// <summary>
    /// Escape characters of char.
    /// </summary>
    public static string[,] Escapes =>
        new[,] {
                { "\"", "\\\\\"" },
                { "\'", "\\\\\'" },
                { "\n", "\\\\n" },
                { "\r", "\\\\r" },
                { "\t", "\\\\t" },
                { "\b", "\\\\b" },
                { "\f", "\\\\f" },
                { "\a", "\\\\a" },
                { "\v", "\\\\v" }
    };

    /// <summary>
    /// Check escape characters for escape character processor(s).
    /// </summary>
    public static string[] EscapeCheck =>
        new[] {
                "\\",
                "\""
    };
  }
}
