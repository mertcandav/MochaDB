using System;
using System.Text.RegularExpressions;
using MochaDB.Querying;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL SELECT keyword.
    /// </summary>
    internal class Mhql_SELECT:MhqlKeyword {
        #region Constructors

        /// <summary>
        /// Create a new Mhql_SELECT.
        /// </summary>
        /// <param name="db">Target database.</param>
        public Mhql_SELECT(MochaDatabase db) {
            Command = string.Empty;
            Tdb = db;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns select command.
        /// </summary>
        /// <param name="final">Command of removed select commands.</param>
        public string GetSELECT(out string final) {
            int usedex = Command.IndexOf("SELECT",StringComparison.OrdinalIgnoreCase);
            if(usedex==-1)
                throw new MochaException("SELECT command is cannot processed!");
            int finaldex = Mhql_GRAMMAR.MainRegex.Match(Command,usedex+6).Index;
            if(finaldex==0)
                throw new MochaException("SELECT command is cannot processed!");
            var usecommand = Command.Substring(usedex+6,finaldex-(usedex+6)).Trim();
            if(!usecommand.StartsWith("(") || !usecommand.EndsWith(")"))
                throw new MochaException("Regex query is cannot processed!");

            final = Command.Substring(finaldex);
            return usecommand.Substring(1,usecommand.Length-2);
        }

        /// <summary>
        /// Returns tables by select pattern.
        /// </summary>
        /// <param name="selectcommand">Select pattern.</param>
        public MochaCollectionResult<MochaTable> GetTables(string selectcommand) {
            var regex = new Regex(selectcommand);
            return Tdb.GetTables(x => regex.IsMatch(x.Name));
        }

        #endregion
    }
}
