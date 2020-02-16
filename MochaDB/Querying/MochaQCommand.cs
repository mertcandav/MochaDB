using System;

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
                command.StartsWith("GET",StringComparison.InvariantCultureIgnoreCase) ||
                command.StartsWith("TABLECOUNT",StringComparison.InvariantCulture) ||
                command.StartsWith("COLUMNCOUNT",StringComparison.InvariantCulture) ||
                command.StartsWith("ROWCOUNT",StringComparison.InvariantCulture) ||
                command.StartsWith("DATACOUNT",StringComparison.InvariantCulture) ||
                command.StartsWith("EXISTS",StringComparison.InvariantCulture))
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
                command.StartsWith("RESET",StringComparison.InvariantCultureIgnoreCase) ||
                command.StartsWith("SET",StringComparison.InvariantCulture) ||
                command.StartsWith("ADD",StringComparison.InvariantCulture) ||
                command.StartsWith("CREATE",StringComparison.InvariantCulture) ||
                command.StartsWith("CLEAR",StringComparison.InvariantCulture) ||
                command.StartsWith("REMOVE",StringComparison.InvariantCulture) ||
                command.StartsWith("RENAME",StringComparison.InvariantCulture) ||
                command.StartsWith("UPDATE",StringComparison.InvariantCulture) ||
                command.StartsWith("EXISTS",StringComparison.InvariantCulture))
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