using System;
using System.Collections.Generic;
using System.Linq;

using MochaDB.Mhql;

namespace MochaDB.mhql.keywords {
  /// <summary>
  /// MHQL GROUPBY keyword.
  /// </summary>
  internal class Mhql_GROUPBY:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Create a new Mhql_GROUPBY.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_GROUPBY(MochaDatabase db) =>
      Tdb=db;

    #endregion Constructors

    #region Members

    /// <summary>
    /// Returns true if command is GROUPBY command, returns if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    public bool IsGROUPBY(string command) =>
        command.StartsWith("GROUPBY",StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns groupby command.
    /// </summary>
    /// <param name="command">MHQL Command.</param>
    /// <param name="final">Command of removed groupby commands.</param>
    public string GetGROUPBY(string command,out string final) {
      int groupbydex = command.IndexOf("GROUPBY",StringComparison.OrdinalIgnoreCase);
      if(groupbydex==-1)
        throw new MochaException("GROUPBY command is cannot processed!");
      System.Text.RegularExpressions.Match match = Mhql_GRAMMAR.MainRegex.Match(command,groupbydex+7);
      int finaldex = match.Index;
      if(finaldex==0)
        throw new MochaException("GROUPBY command is cannot processed!");
      string groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));
      final = command.Substring(finaldex);
      return groupbycommand;
    }

    /// <summary>
    /// Groupby by command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="table">Table to grouping.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public void GroupBy(string command,ref MochaTableResult table,bool from) {
      command = command.Trim();
      int columndex = Mhql_GRAMMAR.GetIndexOfColumn(command,table.Columns,from);

      MochaColumn column = table.Columns[columndex];
      IEnumerable<MochaColumn> columns =
          table.Columns.Where(x => Mhql_GRAMMAR.UseFunctions.Values.Contains(x.Tag));
      Dictionary<object,MochaRow> rows = new Dictionary<object,MochaRow>();
      for(int index = 0; index < table.Rows.Length; ++index) {
        object data = column.Datas[index].Data;
        if(rows.ContainsKey(data)) {
          MochaRow _row;
          rows.TryGetValue(data,out _row);
          for(int dex = 0; dex < columns.Count(); ++dex) {
            MochaColumn col = columns.ElementAt(dex);
            MochaData _data = _row.Datas[Array.IndexOf(table.Columns,col)];
            if(col.Tag == "COUNT")
              _data.Data = int.Parse(_data.ToString())+1;
            else if(col.Tag == "SUM")
              _data.Data =
                  decimal.Parse(_data.ToString()) +
                  decimal.Parse(table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].ToString());
            else if(col.Tag  == "AVG") {
              string[] parts = _data.ToString().Split(';');
              string colval = parts[0];
              int count = int.Parse(parts[1]) + 1;
              _data.Data =
                  decimal.Parse(colval) +
                  decimal.Parse(table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].ToString()) +
                  ";" + count;
            } else {
              decimal
                  currentValue = decimal.Parse(_data.ToString()),
                  value = decimal.Parse(table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].ToString());
              if(col.Tag == "MAX" && currentValue < value)
                _data.Data = value;
              else if(col.Tag == "MIN" && currentValue > value)
                _data.Data = value;
            }
          }
          continue;
        }
        MochaRow row = table.Rows[index];
        for(int dex = 0; dex < columns.Count(); ++dex) {
          MochaColumn col = columns.ElementAt(dex);
          MochaData _data = row.Datas[Array.IndexOf(table.Columns,col)];
          if(col.Tag == "COUNT")
            _data.Data = 1;
          else {
            if(col.Tag == "AVG")
              _data.Data = table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].Data + ";1";
            else
              _data.Data = table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].Data;
          }
        }
        rows.Add(data,row);
      }
      IEnumerable<MochaColumn> avgcols = table.Columns.Where(x => x.Tag == "AVG");
      for(int index = 0; index < avgcols.Count(); ++index) {
        MochaColumn col = avgcols.ElementAt(index);
        for(int rindex = 0; rindex < rows.Keys.Count; ++rindex) {
          MochaData data = rows[rows.Keys.ElementAt(rindex)].Datas[Array.IndexOf(table.Columns,col)];
          string[] parts = data.ToString().Split(';');
          string colval = parts[0];
          data.Data = decimal.Parse(colval) / int.Parse(parts[1]);
        }

      }
      table.Rows = rows.Values.ToArray();
      table.SetDatasByRows();
    }

    #endregion Members
  }
}
