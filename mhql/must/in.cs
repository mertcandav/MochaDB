using System;
using MochaDB.Mhql;
using MochaDB.Querying;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL IN keyword.
    /// </summary>
    internal class Mhql_IN {
        #region Members

        /// <summary>
        /// Returns true if command is IN command, returns if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        public static bool IsIN(string command) =>
            command.StartsWith("IN",StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Process in keyword.
        /// </summary>
        /// <param name="tdb">Target database.</param>
        /// <param name="command">Command.</param>
        /// <param name="table">Destination table.</param>
        /// <param name="from">use state of FROM keyword.</param>
        /// <returns>True if subquery is success, false if not.</returns>
        public static bool Process(MochaDatabase tdb,string command,MochaTableResult table,bool from) {
            command = command.Substring(2).TrimStart();
            int obrace = command.IndexOf(Mhql_LEXER.LBRACE);
            if(obrace == -1)
                throw new MochaException($"{Mhql_LEXER.LBRACE} is not found!");
            MochaColumn column = table.Columns[Mhql_GRAMMAR.GetIndexOfColumn(
                command.Substring(0,obrace).Trim(),table.Columns,from)];
            MochaTableResult result = tdb.ExecuteScalarTable(Mhql_LEXER.RangeBrace(
                command.Substring(obrace).Trim(),Mhql_LEXER.LBRACE,Mhql_LEXER.RBRACE));
            if(result.Columns.Length != 1)
                throw new MochaException("Subqueries should only return one column!");
            else if(column.DataType != result.Columns[0].DataType)
                throw new MochaException("Column data type is not same of subquery result!");
            for(int index = 0; index < column.Datas.Count; index++) {
                if(result.Columns[0].Datas.ContainsData(column.Datas[index].Data))
                    return true;
            }
            return false;
        }

        #endregion
    }
}
