using System;

namespace MochaDB {
    /// <summary>
    /// Exception for MochaDB.
    /// </summary>
    public class MochaException:Exception {
        #region Constructors

        /// <summary>
        /// Create a new MochaException.
        /// </summary>
        public MochaException() {
            Message=string.Empty;
        }

        /// <summary>
        /// Create a new MochaException.
        /// </summary>
        /// <param name="msg">Message of exception.</param>
        public MochaException(string msg) {
            Message=msg;
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaException value) =>
            value.ToString();

        #endregion

        #region Methods

        /// <summary>
        /// Set exception message.
        /// </summary>
        /// <param name="msg">Message to set.</param>
        public void SetMessage(string msg) =>
            Message = msg;

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Message"/>
        /// </summary>
        public override string ToString() {
            return Message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Message of exception.
        /// </summary>
        public new string Message { get; set; }

        #endregion
    }
}
