using System;
using System.Linq;
using MochaDB.mhql.engine;
using MochaDB.Mhql;

namespace MochaDB.mhql {
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

        #endregion

        #region Methods

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
                throw new MochaException("ORDERBY command is cannot processed!");
            var match = MochaDbCommand.mainkeywordRegex.Match(command,orderbydex+7);
            int finaldex = match.Index;
            if(finaldex==0)
                throw new MochaException("ORDERBY command is cannot processed!");
            var orderbycommand = command.Substring(orderbydex+7,finaldex-(orderbydex+7));

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
            command = command.Trim();
            int dex =
                command.StartsWith("ASC",StringComparison.OrdinalIgnoreCase) ?
                3 :
                command.StartsWith("DESC",StringComparison.OrdinalIgnoreCase) ?
                4 : 0;
            
            string[] parts = command.Substring(dex).Split(',');
            int columndex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table,from);
            IOrderedEnumerable<MochaRow> rows =
                dex == 0 || dex == 3 ?
                    table.Rows.OrderBy(x => x.Datas[columndex].ToString(),new ORDERBYComparer()) :
                    table.Rows.OrderByDescending(x => x.Datas[columndex].ToString(),new ORDERBYComparer());
            for(int index = 1; index < parts.Length; index++) {
                int coldex = Mhql_GRAMMAR.GetIndexOfColumn(parts[index].Trim(),table,from);
                if(dex == 0 || dex == 3)
                    rows = rows.ThenBy(x => x.Datas[coldex].ToString(),new ORDERBYComparer());
                else
                    rows = rows.ThenByDescending(x => x.Datas[coldex].ToString(),new ORDERBYComparer());
            }
            table.Rows = rows.ToArray();
            table.SetDatasByRows();
        }

        #endregion
    }
}
