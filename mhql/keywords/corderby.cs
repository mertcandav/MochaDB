namespace MochaDB.mhql.keywords {
  using System;
  using System.Linq;

  using MochaDB.mhql.engine;
  using MochaDB.Mhql;

  /// <summary>
  /// MHQL CORDERBY keyword.
  /// </summary>
  internal class Mhql_CORDERBY:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Create a new Mhql_CORDERBY.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_CORDERBY(MochaDatabase db) {
      Command = string.Empty;
      Tdb = db;
    }

    #endregion Constructors

    #region Members

    /// <summary>
    /// Returns true if command is CORDERBY command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsCORDERBY(string command) =>
        command.StartsWith("CORDERBY",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns corderby command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed corderby commands.</param>
    public string GetCORDERBY(string command,out string final) {
      int orderbydex = command.IndexOf("CORDERBY",StringComparison.OrdinalIgnoreCase);
      if(orderbydex==-1)
        throw new InvalidOperationException("CORDERBY command is cannot processed!");
      System.Text.RegularExpressions.Match match = Mhql_GRAMMAR.MainRegex.Match(command,orderbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new InvalidOperationException("CORDERBY command is cannot processed!");
      string orderbycommand = command.Substring(orderbydex+7,finaldex-(orderbydex+7));
      final = command.Substring(finaldex);
      return orderbycommand;
    }

    /// <summary>
    /// COrderby by command.
    /// </summary>
    /// <param name="command">COrderby command.</param>
    /// <param name="table">Table to ordering.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public void COrderBy(string command,ref MochaTableResult table) {
      if(Mhql_LEXER.SplitParameters(command).Length > 1)
        throw new ArgumentOutOfRangeException("CODERBY keyword are can take only one parameter!");
      command = command.Trim();
      string[] parts = command.Split(new[] { ' ' },2,StringSplitOptions.RemoveEmptyEntries);
      if(parts.Length == 1)
        throw new ArgumentException("CORDERBY order type is not defined!");
      IOrderedEnumerable<MochaColumn> columns =
        parts[1].Equals("ASC",StringComparison.OrdinalIgnoreCase) ?
          table.Columns.OrderBy(x => x.Name,new ORDERBYComparer()) :
          parts[1].Equals("DESC",StringComparison.OrdinalIgnoreCase) ?
            table.Columns.OrderByDescending(x => x.Name,new ORDERBYComparer()) :
            throw new Exception("CORDERBY could not understand this sort type!");
      table.Columns = columns.ToArray();
      table.SetRowsByDatas();
    }

    #endregion Members
  }
}