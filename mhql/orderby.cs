using System;
using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhqlcore {
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
            var match = MochaDbCommand.keywordRegex.Match(command,orderbydex+7);
            if(match.Value.Equals("ASC",StringComparison.OrdinalIgnoreCase) ||
                match.Value.Equals("DESC",StringComparison.OrdinalIgnoreCase))
                match = MochaDbCommand.keywordRegex.Match(command,match.Index+match.Length);
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
        /// <param name="final">Command of removed use commands.</param>
        public void OrderBy(string command,ref MochaTableResult table) {
            command = command.TrimStart().TrimEnd();
            int dex =
                command.StartsWith("ASC",StringComparison.OrdinalIgnoreCase) ?
                3 :
                command.StartsWith("DESC",StringComparison.OrdinalIgnoreCase) ?
                4 : 0;

            int columndex;
            if(!int.TryParse(command.Substring(dex),out columndex))
                throw new MochaException(command);//"Item index is cannot processed!");

            table.Rows.Clone();
            table.Rows.array = (
                dex == 0 || dex == 3 ?
                table.Rows.OrderBy(x => x.Datas[columndex].ToString()) :
                table.Rows.OrderByDescending(x => x.Datas[columndex].ToString())).ToArray();
        }

        #endregion
    }
}
