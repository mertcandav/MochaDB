namespace MochaDB.mhql.keywords {
  using System;
  using System.Linq;

  using MochaDB.mhql.engine;
  using MochaDB.Mhql;

  /// <summary>
  /// MHQL ORDERBY keyword.
  /// </summary>
  internal class Mhql_ORDERBY:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Create a new Mhql_ORDERBY.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_ORDERBY(MochaDatabase db) {
      Command = string.Empty;
      Tdb = db;
    }

    #endregion Constructors

    #region Members

    /// <summary>
    /// Returns true if command is ORDERBY command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsORDERBY(string command) =>
        command.StartsWith("ORDERBY",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns orderby command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed orderby commands.</param>
    public string GetORDERBY(string command,out string final) {
      int orderbydex = command.IndexOf("ORDERBY",StringComparison.OrdinalIgnoreCase);
      if(orderbydex==-1)
        throw new InvalidOperationException("ORDERBY command is cannot processed!");
      System.Text.RegularExpressions.Match match = Mhql_GRAMMAR.MainRegex.Match(command,orderbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new InvalidOperationException("ORDERBY command is cannot processed!");
      string orderbycommand = command.Substring(orderbydex+7,finaldex-(orderbydex+7));
      final = command.Substring(finaldex);
      return orderbycommand;
    }

    /// <summary>
    /// Orderby by command.
    /// </summary>
    /// <param name="command">Orderby command.</param>
    /// <param name="table">Table to ordering.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public void OrderBy(string command,ref MochaTableResult table,bool from) {
      MHQLOrderType DecomposeOrder(string cmd,ref MochaTableResult tbl,out int coldex) {
        string[] orderparts = cmd.Trim().Split(' ');
        if(orderparts.Length > 2)
          throw new ArgumentOutOfRangeException("A single ORDERBY parameter can consist of up to 2 parts!");
        coldex = Mhql_GRAMMAR.GetIndexOfColumn(orderparts[0].Trim(),tbl.Columns,from);

        if(orderparts.Length == 1)
          return 0;

        string order = orderparts[orderparts.Length - 1].Trim();
        return order == string.Empty ||
            order.StartsWith("ASC",StringComparison.OrdinalIgnoreCase) ?
                MHQLOrderType.ASC :
                order.StartsWith("DESC",StringComparison.OrdinalIgnoreCase) ?
                    MHQLOrderType.DESC :
                    throw new Exception("ORDERBY could not understand this '" + order + "' sort type!");
      }
      command = command.Trim();
      string[] parts = Mhql_LEXER.SplitParameters(command);
      int columndex;

      IOrderedEnumerable<MochaRow> rows =
          DecomposeOrder(parts[0],ref table,out columndex) == 0 ?
              table.Rows.OrderBy(x => x.Datas[columndex].ToString(),new ORDERBYComparer()) :
              table.Rows.OrderByDescending(x => x.Datas[columndex].ToString(),new ORDERBYComparer());
      for(int index = 1; index < parts.Length; ++index) {
        int coldex;
        rows =
            DecomposeOrder(parts[index],ref table,out coldex) == 0 ?
                rows.ThenBy(x => x.Datas[coldex].ToString(),new ORDERBYComparer()) :
                rows.ThenByDescending(x => x.Datas[coldex].ToString(),new ORDERBYComparer());
      }
      table.Rows = rows.ToArray();
      table.SetDatasByRows();
    }

    #endregion Members
  }

  /// <summary>
  /// Ordering types of MHQL.
  /// </summary>
  internal enum MHQLOrderType {
    /// <summary>
    /// Ascending.
    /// </summary>
    ASC = 0,
    /// <summary>
    /// Descending.
    /// </summary>
    DESC = 1
  }
}
