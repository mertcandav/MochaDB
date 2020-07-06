using System;
using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL DELROW keyword.
    /// </summary>
    internal class Mhql_DELROW:MhqlKeyword {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="db">Target database.</param>
        public Mhql_DELROW(MochaDatabase db) {
            Tdb = db;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if command is DELROW command, returns if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        public bool IsDELROW(string command) =>
            command.StartsWith("DELROW",StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns delrow command.
        /// </summary>
        /// <param name="command">MHQL Command.</param>
        /// <param name="final">Command of removed delrow commands.</param>
        public string GetDELROW(string command,out string final) {
            int groupbydex = command.IndexOf("DELROW",StringComparison.OrdinalIgnoreCase);
            if(groupbydex==-1)
                throw new MochaException("DELROW command is cannot processed!");
            var match = Mhql_GRAMMAR.MainRegex.Match(command,groupbydex+7);
            int finaldex = match.Index;
            if(finaldex==0)
                throw new MochaException("DELROW command is cannot processed!");
            var groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));

            final = command.Substring(finaldex);
            return groupbycommand;
        }

        /// <summary>
        /// Delrow by command.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="table">Table to delrowing.</param>
        public void Delrow(string command,ref MochaTableResult table) {
            command = command.Trim();
            string[] parts = command.Split(',');
            if(parts.Length > 2)
                throw new MochaException("The DELROW command can take up to 2 parameters!");
            if(parts.Length == 1) {
                int count;
                if(!int.TryParse(command,out count))
                    throw new MochaException("The parameter of the DELROW command was not a number!");
                if(count < 1)
                    throw new MochaException("The parameters of the DELROW command cannot be less than 1!");
                table.Rows = table.Rows.Skip(count).ToArray();
            } else {
                int start, count;
                if(!int.TryParse(parts[0],out start) || !int.TryParse(parts[1],out count))
                    throw new MochaException("The parameter of the DELROW command was not a number!");
                if(start < 1 || count < 1)
                    throw new MochaException("The parameters of the DELROW command cannot be less than 1!");
                var deleted = table.Rows.Skip(start-1).Take(count);
                table.Rows = table.Rows.Where(x => !deleted.Contains(x)).ToArray();
            }
        }

        #endregion
    }
}
