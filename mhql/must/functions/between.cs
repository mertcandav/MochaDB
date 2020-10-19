using MochaDB.Mhql;

namespace MochaDB.mhql.must.functions {
  /// <summary>
  /// MHQL BETWEEN function of MUST.
  /// </summary>
  internal class MhqlMustFunc_BETWEEN {
    /// <summary>
    /// Pass command?
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
      string[] parts = Mhql_LEXER.SplitFunctionParameters(command);
      if(parts.Length != 3)
        throw new MochaException("The BETWEEN function can only take 3 parameters!");

      int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table.Columns,from);
      decimal
          range1,
          range2,
          value;

      if(!decimal.TryParse(parts[1].Trim(),out range1) ||
          !decimal.TryParse(parts[2].Trim(),out range2) ||
          !decimal.TryParse(row.Datas[dex].Data.ToString(),out value))
        throw new MochaException("The parameter of the BETWEEN command was not a number!");

      return
              range1 <= range2 ?
              range1 <= value && value <= range2 :
              range2 <= value && value <= range1;
    }
  }
}
