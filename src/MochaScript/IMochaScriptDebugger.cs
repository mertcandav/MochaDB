using System;

namespace MochaDB.MochaScript {
    /// <summary>
    /// MochaScript code debugger interface for MochaDB MochaScript.
    /// </summary>
    public interface IMochaScriptDebugger:IDisposable {
        #region Events

        event EventHandler<EventArgs> StartDebug;
        event EventHandler<EventArgs> SuccessFinishDebug;
        event EventHandler<MochaScriptEchoEventArgs> Echo;
        event EventHandler<EventArgs> FunctionInvoking;
        event EventHandler<EventArgs> FunctionInvoked;

        #endregion

        #region Methods

        void DebugRun();

        #endregion
    }
}
