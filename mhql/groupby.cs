using System;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL GROUPBY keyword.
    /// </summary>
    internal class Mhql_GROUPBY:MhqlKeyword {
        #region Constructors

        /// <summary>
        /// Create a new Mhql_GROUPBY.
        /// </summary>
        /// <param name="db">Target database.</param>
        public Mhql_GROUPBY(MochaDatabase db) {
            Tdb=db;
        }

        #endregion

        #region Methods

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
            var match = MochaDbCommand.mainkeywordRegex.Match(command,groupbydex+7);
            int finaldex = match.Index;
            if(finaldex==0)
                throw new MochaException("GROUPBY command is cannot processed!");
            var groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));

            final = command.Substring(finaldex);
            return groupbycommand;
        }

        /// <summary>
        /// Groupby by command.
        /// </summary>
        /// <param name="command">Orderby command.</param>
        /// <param name="table">Table to ordering.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public void GroupBy(string command,ref MochaTableResult table,bool from) {
            command = command.Trim();
            int columndex = Mhql_GRAMMAR.GetIndexOfColumn(command,table,from);

            var column = table.Columns[columndex];
            IEnumerable<MochaColumn> columns =
                table.Columns.Where(x => Mhql_GRAMMAR.UseFunctions.Values.Contains(x.Tag));
            Dictionary<object,MochaRow> rows = new Dictionary<object,MochaRow>();
            for(int index = 0; index < table.Rows.Length; index++) {
                object data = column.Datas[index].Data;
                if(rows.ContainsKey(data)) {
                    MochaRow _row;
                    rows.TryGetValue(data,out _row);
                    for(int dex = 0; dex < columns.Count(); dex++) {
                        MochaColumn col = columns.ElementAt(dex);
                        MochaData _data = _row.Datas[table.Columns.IndexOf(col)];
                        if(col.Tag == "COUNT")
                            _data.Data = int.Parse(_data.ToString())+1;
                        else
                            _data.Data =
                                int.Parse(_data.ToString()) +
                                int.Parse(table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].ToString());
                    }
                    continue;
                }
                MochaRow row = table.Rows[index];
                for(int dex = 0; dex < columns.Count(); dex++) {
                    MochaColumn col = columns.ElementAt(dex);
                    MochaData _data = row.Datas[table.Columns.IndexOf(col)];
                    if(col.Tag == "COUNT")
                        _data.Data = 1;
                    else
                        _data.Data = table.Columns.ElementAt(int.Parse(col.Description)).Datas[index].Data;
                }
                rows.Add(data,row);
            }
            table.Rows = rows.Values.ToArray();
            table.SetDatasByRows();
        }

        #endregion
    }
}
