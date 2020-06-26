using System;
using System.Collections.Generic;
using System.Linq;
using MochaDB.framework;
using MochaDB.mhql.engine;
using MochaDB.Mhql;
using MochaDB.Querying;

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
            MochaColumn GetColumn(string cmd,MochaCollectionResult<MochaColumn> cols) {
                var name = Mhql_AS.GetAS(ref cmd);
                if(Mhql_GRAMMAR.UseFunctions.MatchKey(cmd)) {
                    MochaColumn column = new MochaColumn();
                    column.MHQLAsText = name;
                    column.Tag =
                        Mhql_GRAMMAR.UseFunctions.GetValueByMatchKey(cmd);
                    if(column.Tag != "COUNT")
                        column.Description =
                            Mhql_GRAMMAR.GetIndexOfColumn(MhqlEng_EDITOR.DecomposeBrackets(cmd),cols,from).ToString();
                    return column;
                } else {
                    string colname = cmd.StartsWith("$") ? cmd.Substring(1).Trim(): cmd;
                    IEnumerable<MochaColumn> result = cols.Where(x => x.Name == colname);
                    if(result.Count() == 0)
                        throw new MochaException($"Could not find a column with the name '{cmd}'!");
                    MochaColumn column = result.First();
                    column.Tag = colname != cmd ? "$" : null;
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

                var _columns = Tdb.GetColumns(tablename);

                if(parts.Length == 1 && parts[0].Trim() == "*")
                    columns.AddRange(_columns);
                else
                    for(var index = 0; index < parts.Length; index++)
                        columns.Add(GetColumn(parts[index].Trim(),_columns));
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
                    var _columns = Tdb.GetColumns(callparts[0]);
                    if(callparts.Length==1)
                        columns.AddRange(_columns);
                    else
                        columns.Add(GetColumn(callparts[1],_columns));
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

            MochaColumn GetColumn(string cmd) {
                var name = Mhql_AS.GetAS(ref cmd);
                if(Mhql_GRAMMAR.UseFunctions.MatchKey(cmd)) {
                    MochaColumn column = new MochaColumn();
                    column.MHQLAsText = name;
                    column.Tag = Mhql_GRAMMAR.UseFunctions.GetValueByMatchKey(cmd);
                    if(column.Tag != "COUNT") {
                        string colname = MhqlEng_EDITOR.DecomposeBrackets(cmd);
                        column.Description =
                            colname == "Name" ?
                                "0" : colname == "Data" ?
                                    "1" : "2";
                    }
                    return column;
                }
                return null;
            }

            var resulttable = new MochaTableResult();
            var columns = new List<MochaColumn>() {
                new MochaColumn("Name") {
                    MHQLAsText = "Name"
                },
                new MochaColumn("Data") {
                    MHQLAsText = "Data"
                },
                new MochaColumn("Description") {
                    MHQLAsText = "Description"
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
                        rows.Add(new MochaRow(currentsector.Name,currentsector.Data,currentsector.Description));
                    }
                } else {
                    MochaColumn col = GetColumn(callcmd);
                    if(col != null) {
                        columns.Add(col);
                        continue;
                    }
                    var sector = Tdb.GetSector(callcmd);
                    rows.Add(new MochaRow(sector.Name,sector.Data,sector.Description));
                }
            }
            resulttable.Columns = columns.ToArray();
            resulttable.Rows = rows.ToArray();
            resulttable.SetDatasByRows();

            return resulttable;
        }

        #endregion
    }
}
