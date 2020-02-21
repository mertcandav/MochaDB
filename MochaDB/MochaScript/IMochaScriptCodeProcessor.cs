using System.Collections.Generic;

namespace MochaDB.MochaScript {
    /// <summary>
    /// Interface for MochaScript code processors.
    /// </summary>
    public interface IMochaScriptCodeProcessor {
        #region Methods

        bool CheckBrackets(int startIndex,char openBracket,char closeBracket);
        int GetCloseBracketIndex(int startIndex,char openBracket,char closeBracket);

        #endregion

        #region Properties

        IEnumerable<string> Source { get; set; }

        #endregion
    }
}
