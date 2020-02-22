namespace MochaDB.Querying {
    /// <summary>
    /// Base for MochaQ querys.
    /// </summary>
    public class MochaQCommand:IMochaQCommand {
        #region Fields

        private string command;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaQCommand.
        /// </summary>
        public MochaQCommand() {
            Command="BREAKQUERY";
        }

        /// <summary>
        /// Create new MochaQCommand.
        /// </summary>
        /// <param name="command">MochaQ command.</param>
        public MochaQCommand(string command) :
            this() {
            Command=command;
        }

        #endregion

        #region Implicit & Explicit

        public static implicit operator string(MochaQCommand value) =>
            value.Command;

        public static implicit operator MochaQCommand(string value) =>
            new MochaQCommand(value);

        #endregion

        #region Methods

        /// <summary>
        /// Return true if this MochaQ command ise Dynamic command but return false if not.
        /// </summary>
        public bool IsDynamicQuery() =>
            Command.ToUpperInvariant().StartsWith("SELECT");

        /// <summary>
        /// Return true if this MochaQ command ise GetRun command but return false if not.
        /// </summary>
        public bool IsGetRunQuery() {
            string command = Command.ToUpperInvariant();
            if(
                command.StartsWith("GET") ||
                command.StartsWith("TABLECOUNT") ||
                command.StartsWith("COLUMNCOUNT") ||
                command.StartsWith("ROWCOUNT") ||
                command.StartsWith("DATACOUNT") ||
                command.StartsWith("EXISTS"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Return true if this MochaQ command ise Run command but return false if not.
        /// </summary>
        public bool IsRunQuery() {
            string command = Command.ToUpperInvariant();
            if(
                command.StartsWith("RESET") ||
                command.StartsWith("SET") ||
                command.StartsWith("ADD") ||
                command.StartsWith("CREATE") ||
                command.StartsWith("CLEAR") ||
                command.StartsWith("REMOVE") ||
                command.StartsWith("RENAME") ||
                command.StartsWith("UPDATE") ||
                command.StartsWith("EXISTS"))
                return true;
            else
                return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// MochaQ command.
        /// </summary>
        public string Command {
            get =>
                command;
            set {
                value=value.Trim();

                if(value == command)
                    return;

                command=value;
            }
        }

        #endregion
    }
}