using System;
using System.Linq;
using MochaDB.mhql.engine;
using MochaDB.mhql.must;
using MochaDB.Mhql;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL ORDERBY must.
    /// </summary>
    internal class Mhql_MUST:MhqlKeyword {
        #region Constructors

        /// <summary>
        /// Create a new Mhql_MUST.
        /// </summary>
        /// <param name="db">Target database.</param>
        public Mhql_MUST(MochaDatabase db) {
            Command = string.Empty;
            Tdb = db;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if command is MUST command, returns if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        public bool IsMUST(string command) =>
            command.StartsWith("MUST",StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns must command.
        /// </summary>
        /// <param name="command">MHQL Command.</param>
        /// <param name="final">Command of removed orderby commands.</param>
        public string GetMUST(string command,out string final) {
            int mustdex = command.IndexOf("MUST",StringComparison.OrdinalIgnoreCase);
            if(mustdex==-1)
                throw new MochaException("MUST command is cannot processed!");
            var match = MochaDbCommand.mainkeywordRegex.Match(command,mustdex+4);
            int finaldex = match.Index;
            string mustcommand;
            if(finaldex!=0)
                mustcommand = command.Substring(mustdex+4,finaldex-(mustdex+4));
            else
                mustcommand = command.Substring(mustdex+4);

            final = command.Substring(finaldex);
            return mustcommand;
        }

        /// <summary>
        /// Must by tables.
        /// </summary>
        /// <param name="command">Must command.</param>
        /// <param name="table">Table.</param>
        public void MustTable(string command,ref MochaTableResult table) {
            command = command.TrimStart().TrimEnd();
            var parts = MhqlMust_AND.GetParts(command);
            for(int index = 0; index < parts.Length; index++) {
                var partcmd = parts[index].TrimStart().TrimEnd();
                table.Rows = (
                        from value in table.Rows
                        where MhqlMust_REGEX.Match(
                            MhqlMust_REGEX.GetCommand(partcmd,value),
                            (string)MhqlEng_MUST.GetDataFromCommand(partcmd,value)
                            )
                        select value
                        ).ToArray();
            }
        }

        #endregion
    }
}
