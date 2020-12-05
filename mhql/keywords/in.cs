namespace MochaDB.mhql.keywords {
  using System;

  using MochaDB.Mhql;
  using MochaDB.Querying;

  /// <summary>
  /// MHQL IN keyword.
  /// </summary>
  internal class Mhql_IN {
    #region Members

    /// <summary>
    /// Returns true if command is IN command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public static bool IsIN(string command) =>
      command.StartsWith("IN",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Process in keyword.
    /// </summary>
    /// <param name="tdb">Target database.</param>
    /// <param name="command">Command.</param>
    /// <param name="table">Destination table.</param>
    /// <param name="row">Destination row.</param>
    /// <param name="from">Use state of FROM keyword.</param>
    /// <returns>True if subquery is success, false if not.</returns>
    public static bool Process(MochaDatabase tdb,string command,MochaTableResult table,MochaRow row,bool from) {
      command = command.Substring(2).TrimStart();
      int obrace = command.IndexOf(Mhql_LEXER.LBRACE);
      if(obrace == -1)
        throw new MochaException($"{Mhql_LEXER.LBRACE} is not found!");
      MochaColumn column = table.Columns[Mhql_GRAMMAR.GetIndexOfColumn(
          command.Substring(0,obrace).Trim(),table.Columns,from)];
      MochaTableResult result = tdb.ExecuteScalarTable(Mhql_LEXER.RangeBrace(
          command.Substring(obrace).Trim(),Mhql_LEXER.LBRACE,Mhql_LEXER.RBRACE));
      if(result.Columns.Length != 1)
        throw new MochaException("Subqueries should only return one column!");
      else if(MochaData.IsNumericType(column.DataType) != MochaData.IsNumericType(result.Columns[0].DataType)
        && column.DataType != result.Columns[0].DataType)
        throw new MochaException("Column data type is not same of subquery result!");
      for(int index = 0; index < row.Datas.Count; ++index)
        for(int rindex = 0; rindex < result.Columns[0].Datas.Count; ++rindex)
          if(row.Datas[index].Data.ToString() == result.Columns[0].Datas[rindex].Data.ToString())
            return true;
      return false;
    }

    #endregion Members
  }
}
