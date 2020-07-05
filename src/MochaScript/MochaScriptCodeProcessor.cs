using System.Collections.Generic;
using System.Linq;

namespace MochaDB.MochaScript {
    /// <summary>
    /// MochaScript code processor.
    /// </summary>
    public class MochaScriptCodeProcessor {
        #region Constructors

        /// <summary>
        /// Create new MochaScriptCodeProcessor.
        /// </summary>
        public MochaScriptCodeProcessor() {
            Source=null;
        }

        /// <summary>
        /// Create new MochaScriptCodeProcessor.
        /// </summary>
        /// <param name="source">MochaScript code as lines.</param>
        public MochaScriptCodeProcessor(IEnumerable<string> source) {
            Source=source;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check brackets.
        /// </summary>
        /// <param name="startIndex">Index to start finding.</param>
        /// <param name="openBracket">Open bracket char.</param>
        /// <param name="closeBracket">Close bracket char.</param>
        public bool CheckBrackets(int startIndex,char openBracket,char closeBracket) {
            if(GetCloseBracketIndex(startIndex,openBracket,closeBracket) != -1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Find and get close bracket index.
        /// </summary>
        /// <param name="startIndex">Index to start finding.</param>
        /// <param name="openBracket">Open bracket char.</param>
        /// <param name="closeBracket">Close bracket char.</param>
        public int GetCloseBracketIndex(int startIndex,char openBracket,char closeBracket) {
            int openCount = 0;
            string openBracketString = openBracket.ToString();
            string closeBracketString = closeBracket.ToString();

            for(int index = startIndex; index < Source.Count(); index++) {
                string lineJayScriptArray = Source.ElementAt(index).Trim();

                if(lineJayScriptArray == openBracketString)
                    openCount++;
                else if(lineJayScriptArray == closeBracketString && openCount > 0)
                    openCount--;
                else if(lineJayScriptArray == closeBracketString && openCount == 0)
                    return index;
            }
            return -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// MochaScript code as lines.
        /// </summary>
        public IEnumerable<string> Source { get; set; }

        #endregion
    }
}
