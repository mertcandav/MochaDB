using System;
using MochaDB.mhql.engine;

namespace MochaDB.Mhql {
    /// <summary>
    /// Mhql command for MochaDB Mhql queries.
    /// </summary>
    public struct MhqlCommand:IMhqlCommand {
        #region Fields

        private string command;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MhqlCommand.
        /// </summary>
        /// <param name="command">Mhql command.</param>
        public MhqlCommand(string command) :
            this() {
            Command = command;
        }

        #endregion

        #region Operators

        public static implicit operator string(MhqlCommand value) =>
            value.Command;

        public static implicit operator MhqlCommand(string value) =>
            new MhqlCommand(value);

        #endregion

        #region Static

        /// <summary>
        /// Returns true if command ise execute compatible command, returns false if not.
        /// </summary
        /// <param name="command">Command to check.</param>
        public static bool IsExecuteCompatible(string command) {
            return command.TrimEnd().EndsWith("REMOVE",StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns true if command ise reader compatible command, returns false if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        public static bool IsReaderCompatible(string command) {
            return !IsExecuteCompatible(command);
        }

        /// <summary>
        /// Returns true if command ise scalar compatible command, returns false if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        public static bool IsScalarCompatible(string command) {
            return IsReaderCompatible(command);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if command ise execute compatible command, returns false if not.
        /// </summary
        public bool IsExecuteCompatible() {
            return IsExecuteCompatible(Command);
        }

        /// <summary>
        /// Returns true if command ise reader compatible command, returns false if not.
        /// </summary>
        public bool IsReaderCompatible() {
            return !IsExecuteCompatible();
        }

        /// <summary>
        /// Returns true if command ise scalar compatible command, returns false if not.
        /// </summary>
        public bool IsScalarCompatible() {
            return IsReaderCompatible();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Command">.
        /// </summary>
        public override string ToString() {
            return Command;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mhql command.
        /// </summary>
        public string Command {
            get =>
                command;
            set {
                value=value.Trim();
                MhqlEng_EDITOR.RemoveComments(ref value);
                if(value==command)
                    return;

                command =value;
            }
        }

        #endregion
    }
}
