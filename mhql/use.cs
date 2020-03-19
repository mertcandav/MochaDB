using System;
using System.Linq;
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
        /// <param name="final">Final index of use command.</param>
        public string GetUSE(out int final) {
            int usedex = Command.IndexOf("USE",StringComparison.OrdinalIgnoreCase);
            if(usedex==-1)
                throw new MochaException("USE command is cannot processed!");
            int finaldex = MochaDbCommand.keywordRegex.Match(Command,usedex+3).Index;
            if(finaldex==-1)
                throw new MochaException("USE command is cannot processed!");
            var usecommand = Command.Substring(usedex+3,finaldex-(usedex+3));

            final = finaldex;
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
                var table = Tdb.GetTable(callparts[0]).Value;
                if(callparts.Length==1) {
                    columns.AddRange(table.Columns);
                } else {
                    columns.Add(table.Columns[callparts[1]]);
                }
            }

            if(columns.Count > 0 && columns[0].Datas.Count > 0) {
                var firstcolumn = columns[0];
                MochaArray<MochaRow> rows = new MochaRow[firstcolumn.Datas.Count];
                //Process rows.
                for(var dataindex = 0; dataindex < firstcolumn.Datas.Count; dataindex++) {
                    MochaArray<MochaData> datas = new MochaArray<MochaData>(columns.Count);
                    for(var columnindex = 0; columnindex < columns.Count; columnindex++) {
                        var column = columns[columnindex];
                        datas[columnindex] =
                            column.Datas.Count < dataindex+1 ?
                            new MochaData { data =string.Empty,dataType=MochaDataType.String } :
                            column.Datas[dataindex];
                    }
                    rows[dataindex] = new MochaRow(datas);
                }
                resulttable.Rows = rows;
            }

            resulttable.Columns = columns.ToArray();
            return resulttable;
        }

        #endregion
    }
}
