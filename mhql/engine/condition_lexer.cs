using System.Collections.Generic;

namespace MochaDB.mhql.engine {
    /// <summary>
    /// Lexer of MHQL conditions.
    /// </summary>
    internal static class MhqlEng_CONDITION_LEXER {
        /// <summary>
        /// Operators.
        /// </summary>
        public static Dictionary<string,string> __OPERATORS__ =>
            new Dictionary<string,string> {
                { "EQUAL", "==" },
                { "NOTEQUAL", "!=" },
                { "BIGGER", ">" },
                { "LOWER", "<" }
            };
    }

    /// <summary>
    /// Condition type.
    /// </summary>
    internal enum ConditionType {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
        /// <summary>
        /// Euqal operator.
        /// </summary>
        EQUAL = 1,
        /// <summary>
        /// Not equal operator.
        /// </summary>
        NOTEQUAL = 2,
        /// <summary>
        /// Bigger operator.
        /// </summary>
        BIGGER = 3,
        /// <summary>
        /// Lower operator.
        /// </summary>
        LOWER = 4
    }
}
