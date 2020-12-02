namespace MochaDB.mhql.must.functions {
  using MochaDB.Mhql;

  /// <summary>
  /// MHQL STARTW function of MUST.
  /// </summary>
  internal class MhqlMustFunc_STARTW {
    /// <summary>
    /// Pass command?
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
      string[] parts = Mhql_LEXER.SplitFunctionParameters(command);
      int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table.Columns,from);
      for(int index = 1; index < parts.Length; ++index)
        if(row.Datas[dex].Data.ToString().StartsWith(parts[index]))
          return true;
      return false;
    }
  }
}
