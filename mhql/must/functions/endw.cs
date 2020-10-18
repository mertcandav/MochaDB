using MochaDB.Mhql;

namespace MochaDB.mhql.must.functions {
  /// <summary>
  /// MHQL ENDW function of MUST.
  /// </summary>
  internal class MhqlMustFunc_ENDW {
    /// <summary>
    /// Pass command?
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
      var parts = command.Split(',');
      if(parts.Length != 2)
        throw new MochaException("The ENDW function can only take 2 parameters!");

      int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table.Columns,from);

      return
          row.Datas[dex].Data.ToString().EndsWith(parts[1]);
    }
  }
}
