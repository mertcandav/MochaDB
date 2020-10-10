using System;
using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhql {
  /// <summary>
  /// MHQL SUBCOL keyword.
  /// </summary>
  internal class Mhql_SUBCOL:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_SUBCOL(MochaDatabase db) {
      Tdb = db;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns true if command is SUBCOL command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsSUBCOL(string command) =>
        command.StartsWith("SUBCOL",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns subcol command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed subcol commands.</param>
    public string GetSUBCOL(string command,out string final) {
      int groupbydex = command.IndexOf("SUBCOL",StringComparison.OrdinalIgnoreCase);
      if(groupbydex==-1)
        throw new MochaException("SUBCOL command is cannot processed!");
      var match = Mhql_GRAMMAR.MainRegex.Match(command,groupbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new MochaException("SUBCOL command is cannot processed!");
      var groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));

      final = command.Substring(finaldex);
      return groupbycommand;
    }

    /// <summary>
    /// Subcol by command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table to subcoluming.</param>
    public void Subcol(string command,ref MochaTableResult table) {
      command = command.Trim();
      string[] parts = command.Split(',');
      if(parts.Length > 2)
        throw new MochaException("The SUBCOL command can take up to 2 parameters!");
      if(parts.Length == 1) {
        int count;
        if(!int.TryParse(command,out count))
          throw new MochaException("The parameter of the SUBCOL command was not a number!");
        if(count < 1)
          throw new MochaException("The parameters of the SUBCOL command cannot be less than 1!");
        table.Columns = table.Columns.Take(count).ToArray();
      } else {
        int start, count;
        if(!int.TryParse(parts[0],out start) || !int.TryParse(parts[1],out count))
          throw new MochaException("The parameter of the SUBCOL command was not a number!");
        if(start < 1 || count < 1)
          throw new MochaException("The parameters of the SUBCOL command cannot be less than 1!");
        table.Columns = table.Columns.Skip(start-1).Take(count).ToArray();
      }
      table.SetRowsByDatas();
    }

    #endregion
  }
}
