using System;
using System.Text.RegularExpressions;
using MochaDB.Connection;
using MochaDB.mhqlcore;
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
@"USE|RETURN",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        internal Mhql_USE USE;
        internal Mhql_RETURN RETURN;

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
                throw new NullReferenceException("Target database is cannot null!");
            if(Database.ConnectionState!=MochaConnectionState.Connected)
                throw new Exception("Connection is not open!");
        }

        #endregion

        #region ExecuteQuery

        /// <summary>
        /// Run command.
        /// </summary>
        /// <param name="command">MQL Command to set.</param>
        public void ExecuteQuery(string command) {
            Command=command;
            ExecuteQuery();
        }

        /// <summary>
        /// Run command.
        /// </summary>
        public void ExecuteQuery() {
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
            int finaldex;
            var table = USE.GetTable(USE.GetUSE(out finaldex));
            var lastcommand = Command.Substring(finaldex);
            if(!lastcommand.Equals("return",StringComparison.OrdinalIgnoreCase))
                throw new Exception($"'{lastcommand}' command is not processed!");

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
                if(value==command)
                    return;

                command = value;
                USE.Command=value;
                RETURN.Command=value;
            }
        }

        /// <summary>
        /// Target database.
        /// </summary>
        public MochaDatabase Database {
            get =>
                db;
            set {
                if(value==db)
                    return;

                db = value;
                USE.Tdb=value;
                RETURN.Tdb=value;
            }
        }

        #endregion
    }
}
