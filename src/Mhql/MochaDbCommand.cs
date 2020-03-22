﻿using System;
using System.Text.RegularExpressions;
using MochaDB.Connection;
using MochaDB.mhql;
using MochaDB.mhql.engine;
using MochaDB.Streams;

namespace MochaDB.Mhql {
    /// <summary>
    /// MHQL Command processor for MochaDB.
    /// </summary>
    public class MochaDbCommand:IMochaDbCommand {
        #region Fields

        private string command;
        private MochaDatabase db;

        internal static Regex keywordRegex = new Regex(
@"\b(USE|RETURN|ORDERBY|ASC|DESC|MUST|AND|GROUPBY|FROM|AS)\b",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        internal static Regex mainkeywordRegex = new Regex(
@"\b(USE|RETURN|ORDERBY|MUST|GROUPBY)\b",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        internal MochaArray<MhqlKeyword> mhqlobjs;

        internal Mhql_USE USE;
        internal Mhql_RETURN RETURN;
        internal Mhql_ORDERBY ORDERBY;
        internal Mhql_MUST MUST;
        internal Mhql_GROUPBY GROUPBY;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaDbCommand.
        /// </summary>
        /// <param name="db">Target MochaDatabase.</param>
        public MochaDbCommand(MochaDatabase db) {
            //Load mhql core.
            USE = new Mhql_USE(Database);
            RETURN = new Mhql_RETURN(Database);
            ORDERBY = new Mhql_ORDERBY(Database);
            GROUPBY = new Mhql_GROUPBY(Database);
            MUST = new Mhql_MUST(Database);
            mhqlobjs = new MochaArray<MhqlKeyword>(USE,RETURN,ORDERBY,GROUPBY,MUST);

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
            if(Database.ConnectionState!=MochaConnectionState.Connected)
                throw new MochaException("Connection is not open!");
        }

        #endregion

        #region ExecuteQuery

        /// <summary>
        /// Run command.
        /// </summary>
        /// <param name="command">MQL Command to set.</param>
        public void ExecuteQuery(string command) {
            Command=command;
            ExecuteCommand();
        }

        /// <summary>
        /// Run command.
        /// </summary>
        public void ExecuteCommand() {
            CheckConnection();
            if(RETURN.IsReturnableCmd())
                return;
        }

        #endregion

        #region ExecuteScalar

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
            if(!RETURN.IsReturnableCmd())
                return reader;

            bool
                fromkw,
                orderby = false,
                groupby = false;

            string lastcommand;
            var use = USE.GetUSE(out lastcommand);
            fromkw = use.IndexOf("FROM",StringComparison.OrdinalIgnoreCase) != -1;
            var table = USE.GetTable(use,fromkw);
            do {
                //Orderby.
                if(ORDERBY.IsORDERBY(lastcommand)) {
                    orderby=true;
                    if(groupby)
                        throw new MochaException("GROUPBY keyword must be specified before ORDERBY!");
                    ORDERBY.OrderBy(ORDERBY.GetORDERBY(lastcommand,out lastcommand),ref table);
                }
                //Groupby.
                else if(GROUPBY.IsGROUPBY(lastcommand)) {
                    groupby=true;
                    GROUPBY.GroupBy(GROUPBY.GetGROUPBY(lastcommand,out lastcommand),ref table);
                }
                //Must.
                else if(MUST.IsMUST(lastcommand)) {
                    if(orderby)
                        throw new MochaException("MUST keyword must be specified before ORDERBY!");
                    else if(groupby)
                        throw new MochaException("MUST keyword must be specified before GROUPBY!");

                    MUST.MustTable(MUST.GetMUST(lastcommand,out lastcommand),ref table);
                }
                //Return.
                else if(!lastcommand.Equals("return",StringComparison.OrdinalIgnoreCase))
                    throw new MochaException($"'{lastcommand}' command is cannot processed!");
                else
                    break;
            } while(true);

            reader.array = new MochaArray<object>(table);
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
                value=value.TrimStart().TrimEnd();
                MhqlEng_EDITOR.RemoveComments(ref value);
                if(value==command)
                    return;

                command = value;
                for(int index = 0; index < mhqlobjs.Length; index++)
                    mhqlobjs[index].Command = value;
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
                for(int index = 0; index < mhqlobjs.Length; index++)
                    mhqlobjs[index].Tdb = value;
            }
        }

        #endregion
    }
}
