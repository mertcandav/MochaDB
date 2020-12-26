namespace MochaDB.mhql.keywords {
  using System;

  using MochaDB.Mhql;

  /// <summary>
  /// MHQL IN and INEQ keyword.
  /// </summary>
  internal class Mhql_INEQ {
    #region Members

    /// <summary>
    /// Returns true if command is IN command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public static bool IsIN(string command) =>
      command.StartsWith("IN",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true if command is INEQ command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public static bool IsINEQ(string command) =>
        command.StartsWith("INEQ",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Process in keyword.
    /// </summary>
    /// <param name="tdb">Target database.</param>
    /// <param name="command">Command.</param>
    /// <param name="table">Destination table.</param>
    /// <param name="row">Destination row.</param>
    /// <param name="from">Use state of FROM keyword.</param>
    /// <param name="inmode">Set true if command is execute as in mode, set false if ineq mode.</param>
    /// <returns>True if subquery is success, false if not.</returns>
    public static bool Process(MochaDatabase tdb,string command,MochaTableResult table,MochaRow row,
      bool from,bool inmode) {
      command = command.Substring(inmode ? 2 : 4).TrimStart();
      int obrace = command.IndexOf(Mhql_LEXER.LBRACE);
      if(obrace == -1)
        throw new Exception($"{Mhql_LEXER.LBRACE} is not found!");
      MochaColumn column = table.Columns[Mhql_GRAMMAR.GetIndexOfColumn(
          command.Substring(0,obrace).Trim(),table.Columns,from)];
      MochaTableResult result = new MochaDbCommand(tdb).ExecuteScalar(Mhql_LEXER.RangeSubqueryBrace(
          command.Substring(obrace)));
      if(result.Columns.Length != 1)
        throw new Exception("Subqueries should only return one column!");
      else if(MochaData.IsNumericType(column.DataType) != MochaData.IsNumericType(result.Columns[0].DataType)
        && column.DataType != result.Columns[0].DataType)
        throw new Exception("Column data type is not same of subquery result!");
      if(inmode) {
        for(int index = 0; index < row.Datas.Count; ++index)
          for(int rindex = 0; rindex < result.Columns[0].Datas.Count; ++rindex)
            if(row.Datas[index].Data.ToString() == result.Columns[0].Datas[rindex].Data.ToString())
              return true;
        return false;
      } else {
        if(result.Rows.Length != 1)
          return false;
        return row.Datas[0].Data.ToString() == result.Columns[0].Datas[0].Data.ToString();
      }
    }

    #endregion Members
  }
}
