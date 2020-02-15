using System;
using System.Collections.Generic;

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

    /// <summary>
    /// Interface for MochaScript code processor classes.
    /// </summary>
    public interface IMochaScriptCodeProcessor {
        #region Methods

        public bool CheckBrackets(int startIndex,char openBracket,char closeBracket);
        public int GetCloseBracketIndex(int startIndex,char openBracket,char closeBracket);

        #endregion

        #region Properties

        public IEnumerable<string> Source { get; set; }

        #endregion
    }
}