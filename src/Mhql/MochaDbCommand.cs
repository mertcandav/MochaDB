using System;
using System.Collections.Generic;
using System.Linq;
using MochaDB.Connection;
using MochaDB.mhql;
using MochaDB.Streams;

namespace MochaDB.Mhql {
    /// <summary>
    /// MHQL Command processor for MochaDB.
    /// </summary>
    public class MochaDbCommand {
        #region Fields

        private string command;
        private MochaDatabase db;
        internal MhqlKeyword[] keywords;
        internal Mhql_USE USE;
        internal Mhql_SELECT SELECT;
        internal Mhql_REMOVE REMOVE;
        internal Mhql_ORDERBY ORDERBY;
        internal Mhql_MUST MUST;
        internal Mhql_GROUPBY GROUPBY;
        internal Mhql_SUBROW SUBROW;
        internal Mhql_SUBCOL SUBCOL;
        internal Mhql_DELROW DELROW;
        internal Mhql_DELCOL DELCOL;
        internal Mhql_ADDROW ADDROW;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaDbCommand.
        /// </summary>
        /// <param name="db">Target MochaDatabase.</param>
        public MochaDbCommand(MochaDatabase db) {
            //Load mhql core.
            USE = new Mhql_USE(Database);
            SELECT = new Mhql_SELECT(Database);
            ORDERBY = new Mhql_ORDERBY(Database);
            GROUPBY = new Mhql_GROUPBY(Database);
            MUST = new Mhql_MUST(Database);
            REMOVE = new Mhql_REMOVE(Database);
            SUBROW = new Mhql_SUBROW(Database);
            SUBCOL = new Mhql_SUBCOL(Database);
            DELROW = new Mhql_DELROW(Database);
            DELCOL = new Mhql_DELCOL(Database);
            ADDROW = new Mhql_ADDROW(Database);
            keywords = new MhqlKeyword[] {
                USE,
                SELECT,
                REMOVE,
                ORDERBY,
                GROUPBY,
                MUST,
                SUBROW,
                SUBCOL,
                DELROW,
                DELCOL,
                ADDROW
            };

            Database=db;
            Command=string.Empty;
        }

        /// <summary>
        /// Create a new MochaDbCommand.
        /// </summary>
        /// <param name="command">MQL Command.</param>
        /// <param name="db">Target MochaDatabase.</param>
        public MochaDbCommand(string command,MochaDatabase db) :
            this(db) {
            Command=command;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Check connection and database.
        /// </summary>
        internal void CheckConnection() {
            if(Database==null)
                throw new MochaException("Target database is cannot null!");
            if(Database.State!=MochaConnectionState.Connected)
                throw new MochaException("Connection is not open!");
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Returns first data as MochaTableResult.
        /// </summary>
        public MochaTableResult ExecuteScalarTable() {
            return ExecuteScalar() as MochaTableResult;
        }

        /// <summary>
        /// Returns first data as MochaTableResult.
        /// </summary>
        /// <param name="command">MHQL Command to set.</param>
        public MochaTableResult ExecuteScalarTable(string command) {
            return ExecuteScalar(command) as MochaTableResult;
        }

        /// <summary>
        /// Returns first result or null.
        /// </summary>
        /// <param name="command">MQL Command to set.</param>
        public object ExecuteScalar(string command) {
            Command=command;
            return ExecuteScalar();
        }

        /// <summary>
        /// Returns first result or null.
        /// </summary>
        public object ExecuteScalar() {
            var reader = ExecuteReader();
            if(reader.Read())
                return reader.Value;
            return null;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Read returned results.
        /// </summary>
        /// <param name="command">MQL Command to set.</param>
        public MochaReader<object> ExecuteReader(string command) {
            Command=command;
            return ExecuteReader();
        }

        /// <summary>
        /// Read returned results.
        /// </summary>
        public MochaReader<object> ExecuteReader() {
            CheckConnection();
            var reader = new MochaReader<object>();

            bool fromkw;
            string lastcommand;

            if(Command.StartsWith("USE",StringComparison.OrdinalIgnoreCase)) {
                var use = USE.GetUSE(out lastcommand);
                fromkw = Mhql_FROM.IsFROM(use);
                var table = USE.GetTable(use,fromkw);

                do {
                    //Orderby.
                    if(ORDERBY.IsORDERBY(lastcommand)) {
                        ORDERBY.OrderBy(ORDERBY.GetORDERBY(lastcommand,out lastcommand),ref table,fromkw);
                    }
                    //Groupby.
                    else if(GROUPBY.IsGROUPBY(lastcommand)) {
                        GROUPBY.GroupBy(GROUPBY.GetGROUPBY(lastcommand,out lastcommand),ref table,fromkw);
                    }
                    //Must.
                    else if(MUST.IsMUST(lastcommand)) {
                        MUST.MustTable(MUST.GetMUST(lastcommand,out lastcommand),ref table,fromkw);
                    }
                    //Subrow.
                    else if(SUBROW.IsSUBROW(lastcommand)) {
                        SUBROW.Subrow(SUBROW.GetSUBROW(lastcommand,out lastcommand),ref table);
                    }
                    //Subcol.
                    else if(SUBCOL.IsSUBCOL(lastcommand)) {
                        SUBCOL.Subcol(SUBCOL.GetSUBCOL(lastcommand,out lastcommand),ref table);
                    }
                    //Delrow.
                    else if(DELROW.IsDELROW(lastcommand)) {
                        DELROW.Delrow(DELROW.GetDELROW(lastcommand,out lastcommand),ref table);
                    }
                    //Delcol.
                    else if(DELCOL.IsDELCOL(lastcommand)) {
                        DELCOL.Delcol(DELCOL.GetDELCOL(lastcommand,out lastcommand),ref table);
                    }
                    //Addrow
                    else if(ADDROW.IsADDROW(lastcommand)) {
                        ADDROW.Addrow(ADDROW.GetADDROW(lastcommand,out lastcommand),ref table);
                    }
                    //Return.
                    else if(lastcommand == string.Empty) {
                        IEnumerable<MochaColumn> cols = table.Columns.Where(x => x.Tag != "$");
                        if(cols.Count() != table.Columns.Length) {
                            table.Columns = cols.ToArray();
                            table.SetRowsByDatas();
                        }
                        break;
                    } else
                        throw new MochaException($"'{lastcommand}' command is cannot processed!");
                } while(true);
                reader.array = new[] { table };
            } else if(Command.StartsWith("SELECT",StringComparison.OrdinalIgnoreCase)) {
                fromkw = Mhql_FROM.IsFROM(command);
                if(fromkw)
                    throw new MochaException("FROM keyword is cannot use with SELECT keyword!");
                var select = SELECT.GetSELECT(out lastcommand);
                if(!string.IsNullOrWhiteSpace(lastcommand))
                    throw new MochaException($"'{lastcommand}' command is cannot processed!");
                reader.array = SELECT.GetTables(select);
            } else
                throw new MochaException("MHQL is cannot processed!");
            return reader;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns command.
        /// </summary>
        public override string ToString() {
            return Command;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Current MQL command.
        /// </summary>
        public string Command {
            get =>
                command;
            set {
                if(value==command)
                    return;

                command = value.Trim();
                for(int index = 0; index < keywords.Length; index++)
                    keywords[index].Command = command;
            }
        }

        /// <summary>
        /// Target database.
        /// </summary>
        public MochaDatabase Database {
            get =>
                db;
            set {
                if(value==null)
                    throw new MochaException("This MochaDatabase is not affiliated with a database!");
                if(value==db)
                    return;

                db = value;
                for(int index = 0; index < keywords.Length; index++)
                    keywords[index].Tdb = value;
            }
        }

        #endregion
    }
}
