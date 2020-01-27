namespace MochaDB.MochaScript {
    /// <summary>
    /// MochaScript code processor.
    /// </summary>
    internal sealed class MochaScriptCodeProcessor {
        /// <summary>
        /// Check brackets.
        /// </summary>
        /// <param name="source">MochaScript codes as lines.</param>
        /// <param name="startIndex">Index to start finding.</param>
        /// <param name="openBracket">Open bracket char.</param>
        /// <param name="closeBracket">Close bracket char.</param>
        internal static bool CheckBrackets(string[] source,int startIndex,char openBracket,char closeBracket) {
            if(GetCloseBracketIndex(source,startIndex,openBracket,closeBracket) != -1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Find and get close bracket index.
        /// </summary>
        /// <param name="source">MochaScript codes as lines.</param>
        /// <param name="startIndex">Index to start finding.</param>
        /// <param name="openBracket">Open bracket char.</param>
        /// <param name="closeBracket">Close bracket char.</param>
        internal static int GetCloseBracketIndex(string[] source,int startIndex,char openBracket,char closeBracket) {
            int openCount = 0;
            string openBracketString = openBracket.ToString();
            string closeBracketString = closeBracket.ToString();

            for(int index = startIndex; index < source.Length; index++) {
                string lineJayScriptArray = source[index].Trim();

                if(lineJayScriptArray == openBracketString)
                    openCount++;
                else if(lineJayScriptArray == closeBracketString && openCount > 0)
                    openCount--;
                else if(lineJayScriptArray == closeBracketString && openCount == 0)
                    return index;
            }
            return -1;
        }
    }
}
