namespace MochaDB.mhql.must.functions {
  using System;

  using MochaDB.Mhql;

  /// <summary>
  /// MHQL BIGGER function of MUST.
  /// </summary>
  internal class MhqlMustFunc_BIGGER {
    /// <summary>
    /// Pass command?
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
      string[] parts = Mhql_LEXER.SplitFunctionParameters(command);
      if(parts.Length != 2)
        throw new ArgumentOutOfRangeException("The BIGGER function can only take 2 parameters!");

      int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table.Columns,from);
      decimal
          range,
          value;

      if(!decimal.TryParse(parts[1].Trim(),out range) ||
          !decimal.TryParse(row.Datas[dex].Data.ToString(),out value))
        throw new ArithmeticException("The parameter of the BIGGER command was not a number!");

      return value >= range;
    }
  }
}
