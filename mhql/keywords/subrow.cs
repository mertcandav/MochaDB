using System;
using System.Linq;

using MochaDB.Mhql;

namespace MochaDB.mhql.keywords {
  /// <summary>
  /// MHQL SUBROW keyword.
  /// </summary>
  internal class Mhql_SUBROW:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_SUBROW(MochaDatabase db) =>
      Tdb = db;

    #endregion Constructors

    #region Members

    /// <summary>
    /// Returns true if command is SUBROW command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsSUBROW(string command) =>
        command.StartsWith("SUBROW",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns subrow command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed subrow commands.</param>
    public string GetSUBROW(string command,out string final) {
      int groupbydex = command.IndexOf("SUBROW",StringComparison.OrdinalIgnoreCase);
      if(groupbydex==-1)
        throw new MochaException("SUBROW command is cannot processed!");
      System.Text.RegularExpressions.Match match = Mhql_GRAMMAR.MainRegex.Match(command,groupbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new MochaException("SUBROW command is cannot processed!");
      string groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));
      final = command.Substring(finaldex);
      return groupbycommand;
    }

    /// <summary>
    /// Subrow by command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table to subrowing.</param>
    public void Subrow(string command,ref MochaTableResult table) {
      command = command.Trim();
      string[] parts = command.Split(',');
      if(parts.Length > 2)
        throw new MochaException("The SUBROW command can take up to 2 parameters!");
      if(parts.Length == 1) {
        int count;
        if(!int.TryParse(command,out count))
          throw new MochaException("The parameter of the SUBROW command was not a number!");
        if(count < 1)
          throw new MochaException("The parameters of the SUBROW command cannot be less than 1!");
        table.Rows = table.Rows.Take(count).ToArray();
      } else {
        int start, count;
        if(!int.TryParse(parts[0],out start) || !int.TryParse(parts[1],out count))
          throw new MochaException("The parameter of the SUBROW command was not a number!");
        if(start < 1 || count < 1)
          throw new MochaException("The parameters of the SUBROW command cannot be less than 1!");
        table.Rows = table.Rows.Skip(start-1).Take(count).ToArray();
      }
    }

    #endregion Members
  }
}
