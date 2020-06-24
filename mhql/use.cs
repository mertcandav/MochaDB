using System;
using System.Collections.Generic;
using MochaDB.Mhql;

namespace MochaDB.mhql {
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
            int finaldex = MochaDbCommand.mainkeywordRegex.Match(Command,usedex+3).Index;
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
        public MochaTableResult GetTable(string usecommand,bool from) {
            MochaColumn GetColumn(string cmd,MochaTable tbl) {
                var name = Mhql_AS.GetAS(ref cmd);
                string uppercmd = cmd.ToUpperInvariant();
                if(Mhql_GRAMMAR.UseFunctions.ContainsKey(uppercmd)) {
                    MochaColumn column = new MochaColumn();
                    column.MHQLAsText = name;
                    string tag;
                    Mhql_GRAMMAR.UseFunctions.TryGetValue(uppercmd,out tag);
                    column.Tag = tag;
                    return column;
                } else {
                    MochaColumn column = tbl.Columns[cmd];
                    column.MHQLAsText = name;
                    return column;
                }
            }

            var columns = new List<MochaColumn>();
            var resulttable = new MochaTableResult();

            if(from) {
                var dex = usecommand.IndexOf("FROM",StringComparison.OrdinalIgnoreCase);
                var tablename = usecommand.Substring(dex+4).Trim();
                var parts = usecommand.Substring(0,dex).Split(',');

                var table = Tdb.GetTable(tablename);

                if(parts.Length == 1 && parts[0].Trim() == "*")
                    columns.AddRange(table.Columns);
                else
                    for(var index = 0; index < parts.Length; index++)
                        columns.Add(GetColumn(parts[index].Trim(),table));
            } else {
                var parts = usecommand.Split(',');
                for(var index = 0; index < parts.Length; index++) {
                    var callcmd = parts[index].Trim();
                    if(callcmd == "*") {
                        var tables = Tdb.GetTables();
                        for(int tindex = 0; tindex < tables.Count; tindex++)
                            columns.AddRange(tables[tindex].Columns);
                        continue;
                    }

                    var callparts = callcmd.Split('.');
                    if(callparts.Length>2)
                        throw new MochaException($"'{callcmd}' command is cannot processed!");
                    for(byte partindex = 0; partindex < callparts.Length; partindex++)
                        callparts[partindex] = callparts[partindex].Trim();
                    var table = Tdb.GetTable(callparts[0]);
                    if(callparts.Length==1)
                        columns.AddRange(table.Columns);
                    else
                        columns.Add(GetColumn(callparts[1],table));
                }
            }

            resulttable.Columns = columns.ToArray();
            resulttable.SetRowsByDatas();

            return resulttable;
        }

        /// <summary>
        /// Returns sector table by use command.
        /// </summary>
        /// <param name="usecommand">Use command.</param>
        public MochaTableResult GetSector(string usecommand,bool from) {
            if(from)
                throw new MochaException("FROM keyword is cannot used with @SECTORS marked commands!");

            var resulttable = new MochaTableResult {
                Columns = new[] {
                    new MochaColumn("Name"),
                    new MochaColumn("Description"),
                    new MochaColumn("Data")
                }
            };
            var rows = new List<MochaRow>();

            var parts = usecommand.Split(',');
            for(var index = 0; index < parts.Length; index++) {
                var callcmd = parts[index].Trim();
                if(callcmd == "*") {
                    var sectors = Tdb.GetSectors();
                    for(int sindex = 0; sindex < sectors.Count; sindex++) {
                        var currentsector = sectors.ElementAt(sindex);
                        rows.Add(new MochaRow(currentsector.Name,currentsector.Description,currentsector.Data));
                    }
                } else {
                    var sector = Tdb.GetSector(callcmd);
                    rows.Add(new MochaRow(sector.Name,sector.Description,sector.Data));
                }
            }
            resulttable.Rows = rows.ToArray();

            return resulttable;
        }

        #endregion
    }
}
