using System;
using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL FROM keyword.
    /// </summary>
    internal class Mhql_FROM:MhqlKeyword {
        #region Methods

        /// <summary>
        /// Returns true if command is FROM style, returns if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        public static bool IsFROM(string command) =>
            GetIndex(ref command) != -1;

        /// <summary>
        /// Returns index of FROM keyword.
        /// </summary>
        /// <param name="command">Command.</param>
        public static int GetIndex(ref string command) =>
            command.LastIndexOf(" FROM ",StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns subrow command.
        /// </summary>
        /// <param name="command">MHQL Command.</param>
        /// <param name="final">Command of removed subrow commands.</param>
        public static string GetSUBROW(string command,out string final) {
            int groupbydex = command.IndexOf("SUBROW",StringComparison.OrdinalIgnoreCase);
            if(groupbydex==-1)
                throw new MochaException("SUBROW command is cannot processed!");
            var match = MochaDbCommand.mainkeywordRegex.Match(command,groupbydex+7);
            int finaldex = match.Index;
            if(finaldex==0)
                throw new MochaException("SUBROW command is cannot processed!");
            var groupbycommand = command.Substring(groupbydex+7,finaldex-(groupbydex+7));

            final = command.Substring(finaldex);
            return groupbycommand;
        }

        #endregion
    }
}
