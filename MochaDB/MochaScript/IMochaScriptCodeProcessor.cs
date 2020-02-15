using System.Collections.Generic;

namespace MochaDB.MochaScript {
    /// <summary>
    /// Interface for MochaScript code processors.
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
