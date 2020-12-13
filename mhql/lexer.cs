namespace MochaDB.mhql {
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Lexer of MHQL.
  /// </summary>
  internal static class Mhql_LEXER {
    #region Members

    /// <summary>
    /// Get range by brackets.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="open">Open bracket.</param>
    /// <param name="close">Close bracket.</param>
    /// <returns>Range of brackets.</returns>
    public static string RangeBrace(string value,char open,char close) {
      if(open == close)
        throw new InvalidOperationException("Open and close brackets are same!");

      value = value.TrimStart();
      if(value.Length < 2 && value[0] == open)
        return string.Empty;

      int openCount = 1;
      for(int index = 1; index < value.Length; ++index) {
        if(openCount == 0)
          return value.Substring(1,index);

        char current = value[index];
        if(current == open)
          ++openCount;
        else if(current == close)
          --openCount;
      }
      if(openCount == 0)
        return value.Substring(1,value.Length-2).Trim();
      else
        throw new MochaException("Brackets is opened but not close!");
    }

    /// <summary>
    /// Get range by subquery standards.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Subquery.</returns>
    public static string RangeSubqueryBrace(string value) =>
      value.Substring(1,
        RangeBrace($"{LBRACE}{value.Substring(1,value.Length - 1).Replace(LBRACE,' ').Replace(RBRACE,' ')}{RBRACE}",
          LBRACE,RBRACE).Length + 3);

    /// <summary>
    /// Split function parameters.
    /// </summary>
    /// <param name="statement">Statement of function.</param>
    /// <returns>Params.</returns>
    public static string[] SplitFunctionParameters(string statement) =>
      statement.Split(FUNC_PARAM_DELIMITER);

    /// <summary>
    /// Split parameters.
    /// </summary>
    /// <param name="statement">Statement.</param>
    /// <returns>Params.</returns>
    public static string[] SplitParameters(string statement) =>
      statement.Split(PARAM_DELIMITER);

    /// <summary>
    /// Split use parameters.
    /// </summary>
    /// <param name="statement">Statement.</param>
    /// <returns>Params.</returns>
    public static List<string> SplitUseParameters(string statement) {
      List<string> parts = new List<string>();
      int count = 0, last = 0;
      statement = statement.TrimStart();
      for(int index = 0; index < statement.Length; ++index) {
        char current = statement[index];
        if(current == LBRACE)
          ++count;
        else if(current == RBRACE)
          --count;

        if(count != 0)
          continue;

        if(current != PARAM_DELIMITER)
          continue;

        parts.Add(statement.Substring(last,index - last));
        last = index + 1;
      }
      if(last < statement.Length)
        parts.Add(statement.Substring(last));
      return parts;
    }

    /// <summary>
    /// Split sub calls.
    /// </summary>
    /// <param name="statement">Statement.</param>
    /// <returns>Params.</returns>
    public static string[] SplitSubCalls(string statement) =>
      statement.Split(SUBCALL_DELIMITER);

    #endregion Members

    #region Properties

    /// <summary>
    /// Left curly bracket.
    /// </summary>
    public static char LBRACE => '{';

    /// <summary>
    /// Right curly bracket.
    /// </summary>
    public static char RBRACE => '}';

    /// <summary>
    /// Left parentheses.
    /// </summary>
    public static char LPARANT => '(';

    /// <summary>
    /// Right parentheses.
    /// </summary>
    public static char RPARANT => ')';

    /// <summary>
    /// Delimiter of function parameters.
    /// </summary>
    public static char FUNC_PARAM_DELIMITER => ',';

    /// <summary>
    /// Delimiter of parameters.
    /// </summary>
    public static char PARAM_DELIMITER => ',';

    /// <summary>
    /// Delimiter of sub object call.
    /// </summary>
    public static char SUBCALL_DELIMITER => '.';

    /// <summary>
    /// Match with all operator.
    /// </summary>
    public static char ALL_OPERATOR => '*';

    #endregion Properties
  }
}
