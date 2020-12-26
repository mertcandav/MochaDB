namespace MochaDB.mhql.keywords {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using MochaDB.Mhql;

  /// <summary>
  /// MHQL ADDROW keyword.
  /// </summary>
  internal class Mhql_ADDROW:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Initialize a new instance.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_ADDROW(MochaDatabase db) =>
      Tdb = db;

    #endregion Constructors

    #region Members

    /// <summary>
    /// Returns true if command is ADDROW command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsADDROW(string command) =>
        command.StartsWith("ADDROW",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns addrow command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed addrow commands.</param>
    public string GetADDROW(string command,out string final) {
      int groupbydex = command.IndexOf("ADDROW",StringComparison.OrdinalIgnoreCase);
      if(groupbydex==-1)
        throw new InvalidOperationException("ADDROW command is cannot processed!");
      System.Text.RegularExpressions.Match match = Mhql_GRAMMAR.MainRegex.Match(command,groupbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new InvalidOperationException("ADDROW command is cannot processed!");
      string groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));
      final = command.Substring(finaldex);
      return groupbycommand;
    }

    /// <summary>
    /// Addrow by command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table to add rowing.</param>
    public void Addrow(string command,ref MochaTableResult table) {
      command = command.Trim();
      int count;
      if(!int.TryParse(command,out count))
        throw new ArgumentException("The parameter of the ADDROW command was not a number!");
      if(count < 1)
        throw new ArgumentOutOfRangeException("The parameters of the ADDROW command cannot be less than 1!");
      IList<MochaRow> rows = new List<MochaRow>(table.Rows);
      for(int counter = 0; counter < count; ++counter) {
        MochaRow row = new MochaRow();
        for(int index = 0; index < table.Columns.Length; ++index)
          row.Datas.Add(new MochaData() {
            dataType = table.Columns[index].DataType,
            data = MochaData.TryGetData(table.Columns[index].DataType,null)
          });
        rows.Add(row);
      }
      table.Rows = rows.ToArray();
    }

    #endregion Members
  }
}
