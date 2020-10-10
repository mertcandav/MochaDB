using System;
using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhql {
  /// <summary>
  /// MHQL DELCOL keyword.
  /// </summary>
  internal class Mhql_DELCOL:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_DELCOL(MochaDatabase db) {
      Tdb = db;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns true if command is DELCOL command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsDELCOL(string command) =>
        command.StartsWith("DELCOL",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns delcol command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed delcol commands.</param>
    public string GetDELCOL(string command,out string final) {
      int groupbydex = command.IndexOf("DELCOL",StringComparison.OrdinalIgnoreCase);
      if(groupbydex==-1)
        throw new MochaException("DELCOL command is cannot processed!");
      var match = Mhql_GRAMMAR.MainRegex.Match(command,groupbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new MochaException("DELCOL command is cannot processed!");
      var groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));

      final = command.Substring(finaldex);
      return groupbycommand;
    }

    /// <summary>
    /// Delcol by command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table to delcoluming.</param>
    public void Delcol(string command,ref MochaTableResult table) {
      command = command.Trim();
      string[] parts = command.Split(',');
      if(parts.Length > 2)
        throw new MochaException("The DELCOL command can take up to 2 parameters!");
      if(parts.Length == 1) {
        int count;
        if(!int.TryParse(command,out count))
          throw new MochaException("The parameter of the DELCOL command was not a number!");
        if(count < 1)
          throw new MochaException("The parameters of the DELCOL command cannot be less than 1!");
        table.Columns = table.Columns.Skip(count).ToArray();
      } else {
        int start, count;
        if(!int.TryParse(parts[0],out start) || !int.TryParse(parts[1],out count))
          throw new MochaException("The parameter of the DELCOL command was not a number!");
        if(start < 1 || count < 1)
          throw new MochaException("The parameters of the DELCOL command cannot be less than 1!");
        var deleted = table.Columns.Skip(start-1).Take(count);
        table.Columns = table.Columns.Where(x => !deleted.Contains(x)).ToArray();
      }
      table.SetRowsByDatas();
    }

    #endregion
  }
}
