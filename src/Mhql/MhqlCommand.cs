using MochaDB.mhql.engine;

namespace MochaDB.Mhql {
    /// <summary>
    /// Mhql command for MochaDB Mhql querys.
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

        #region Methods

        /// <summary>
        /// Returns true if command ise execute compatible command, returns false if not.
        /// </summary
        public bool IsExecuteCompatible() {
            return !Command.TrimEnd().EndsWith("RETURN");
        }

        /// <summary>
        /// Returns true if command ise reader compatible command, returns false if not.
        /// </summary>
        public bool IsReaderCompatible() {
            return Command.TrimEnd().EndsWith("RETURN");
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
                value=value.TrimStart().TrimEnd();
                MhqlEng_EDITOR.RemoveComments(ref value);
                if(value==command)
                    return;

                command =value;
            }
        }

        #endregion
    }
}
