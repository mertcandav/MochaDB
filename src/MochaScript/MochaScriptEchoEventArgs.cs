using System;

namespace MochaDB.MochaScript {
    /// <summary>
    /// Echo event arguments for echo events.
    /// </summary>
    public class MochaScriptEchoEventArgs:EventArgs {
        #region Constructors

        /// <summary>
        /// Create new MochaScriptEchoEventArgs.
        /// </summary>
        /// <param name="message">Echo message.</param>
        public MochaScriptEchoEventArgs(object message) =>
            Message = message;

        #endregion

        #region Properties

        /// <summary>
        /// Echo message.
        /// </summary>
        public object Message { get; private set; }

        #endregion
    }
}
