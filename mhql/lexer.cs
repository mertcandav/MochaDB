using System;

namespace MochaDB.mhql {
    /// <summary>
    /// Lexer of MHQL.
    /// </summary>
    internal static class Mhql_LEXER {
        #region Members

        /// <summary>
        /// Get range by brackets.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="open">Open bracket.</param>
        /// <param name="close">Close bracket.</param>
        /// <returns>Range of brackets.</returns>
        public static string RangeBrace(string value,char open,char close) {
            if(open == close)
                throw new InvalidOperationException("Open and close brackets are same!");

            value = value.TrimStart();
            if(value.Length < 2 && value[0] == open)
                return string.Empty;

            int openCount = 1;
            for(int index = 1; index < value.Length; index++) {
                if(openCount == 0)
                    return value.Substring(1,index);

                char current = value[index];
                if(current == open)
                    openCount++;
                else if(current == close)
                    openCount--;
            }
            if(openCount == 0)
                return value.Substring(1,value.Length-2).Trim();
            else
                throw new MochaException("Brackets is opened but not close!");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Left curly bracket.
        /// </summary>
        public static char LBRACE => '{';

        /// <summary>
        /// Right curly bracket.
        /// </summary>
        public static char RBRACE => '}';

        /// <summary>
        /// Left parentheses.
        /// </summary>
        public static char LPARANT => '(';

        /// <summary>
        /// Right parentheses.
        /// </summary>
        public static char RPARANT => ')';

        #endregion
    }
}
