using System;
using System.Collections.Generic;
using MochaDB.Mhql;

namespace MochaDB.mhqlcore {
    /// <summary>
    /// MHQL USE keyword.
    /// </summary>
    internal class Mhql_USE:MhqlKeyword {
        #region Constructors

        /// <summary>
        /// Create a new Mhql_USE.
        /// </summary>
        /// <param name="db">Target database.</param>
        public Mhql_USE(MochaDatabase db) {
            Command = string.Empty;
            Tdb = db;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns use command.
        /// </summary>
        /// <param name="final">Command of removed use commands.</param>
        public string GetUSE(out string final) {
            int usedex = Command.IndexOf("USE",StringComparison.OrdinalIgnoreCase);
            if(usedex==-1)
                throw new MochaException("USE command is cannot processed!");
            int finaldex = MochaDbCommand.keywordRegex.Match(Command,usedex+3).Index;
            if(finaldex==0)
                throw new MochaException("USE command is cannot processed!");
            var usecommand = Command.Substring(usedex+3,finaldex-(usedex+3));

            final = Command.Substring(finaldex);
            return usecommand;
        }

        /// <summary>
        /// Returns table by use command.
        /// </summary>
        /// <param name="usecommand">Use command.</param>
        public MochaTableResult GetTable(string usecommand) {
            var columns = new List<MochaColumn>();
            var resulttable = new MochaTableResult();
            var parts = usecommand.Split(',');

            for(var index = 0; index < parts.Length; index++) {
                var callcmd = parts[index];
                var callparts = callcmd.Split('.');
                if(callparts.Length>2)
                    throw new MochaException($"'{callcmd}' command is cannot processed!");
                for(byte partindex = 0; partindex < callparts.Length; partindex++)
                    callparts[partindex] = callparts[partindex].TrimStart().TrimEnd();
                var table = Tdb.GetTable(callparts[0]);
                if(callparts.Length==1) {
                    columns.AddRange(table.Columns);
                } else {
                    columns.Add(table.Columns[callparts[1]]);
                }
            }

            resulttable.Columns = columns.ToArray();
            resulttable.SetRowsByDatas();

            return resulttable;
        }

        #endregion
    }
}
