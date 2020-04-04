using MochaDB.Querying;

namespace MochaDB.Mochaq {
    /// <summary>
    /// MochaQ query for MochaDB.
    /// </summary>
    public struct MochaQCommand:IMochaQCommand {
        #region Fields

        private string command;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaQCommand.
        /// </summary>
        /// <param name="command">MochaQ command.</param>
        public MochaQCommand(string command) :
            this() {
            Command=command;
        }

        #endregion

        #region Operators

        public static implicit operator string(MochaQCommand value) =>
            value.Command;

        public static implicit operator MochaQCommand(string value) =>
            new MochaQCommand(value);

        #endregion

        #region Methods

        /// <summary>
        /// Set command.
        /// </summary>
        /// <param name="command">Command to set.</param>
        public void SetCommand(string command) =>
            Command = command;

        /// <summary>
        /// Return true if this MochaQ command ise Dynamic command but return false if not.
        /// </summary>
        public bool IsDynamicQuery() {
            string command = Command.ToUpperInvariant();
            if(
                !string.IsNullOrEmpty(command) &&
                command.StartsWith("SELECT"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Return true if this MochaQ command ise GetRun command but return false if not.
        /// </summary>
        public bool IsGetRunQuery() {
            string command = Command.ToUpperInvariant();
            if(
                !string.IsNullOrEmpty(command) && (
                command.StartsWith("GET") ||
                command.StartsWith("TABLECOUNT") ||
                command.StartsWith("COLUMNCOUNT") ||
                command.StartsWith("ROWCOUNT") ||
                command.StartsWith("DATACOUNT") ||
                command.StartsWith("EXISTS") ||
                command.StartsWith("FILESYSTEM_EXISTS") ||
                command.FirstChar() == '#'))
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
                !string.IsNullOrEmpty(command) && (
                command.StartsWith("RESET") ||
                command.StartsWith("SET") ||
                command.StartsWith("ADD") ||
                command.StartsWith("CREATE") ||
                command.StartsWith("CLEAR") ||
                command.StartsWith("REMOVE") ||
                command.StartsWith("RENAME") ||
                command.StartsWith("UPDATE") ||
                command.StartsWith("FILESYSTEM_CLEAR") ||
                command.StartsWith("FILESYSTEM_REMOVE") ||
                command.StartsWith("FILESYSTEM_UPLOAD") ||
                command.StartsWith("FILESYSTEM_CREATE") ||
                command.StartsWith("RESTORE")))
                return true;
            else
                return false;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Command"/>.
        /// </summary>
        public override string ToString() {
            return Command;
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
