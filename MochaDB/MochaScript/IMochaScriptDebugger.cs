using System;

namespace MochaDB.MochaScript {
    /// <summary>
    /// MochaScript code debugger interface for MochaDB MochaScript.
    /// </summary>
    public interface IMochaScriptDebugger:IDisposable {
        #region Events

        public event EventHandler<EventArgs> StartDebug;
        public event EventHandler<EventArgs> SuccessFinishDebug;
        public event EventHandler<MochaScriptEchoEventArgs> Echo;
        public event EventHandler<EventArgs> FunctionInvoking;
        public event EventHandler<EventArgs> FunctionInvoked;

        #endregion

        #region Methods

        public void DebugRun();

        #endregion
    }
}
