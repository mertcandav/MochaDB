using System.Collections.Generic;
using MochaDB.framework;

namespace MochaDB.mhql.engine {
    /// <summary>
    /// lexer of MHQL conditions.
    /// </summary>
    internal static class MhqlEng_CONDITION_LEXER {
        /// <summary>
        /// Operators.
        /// </summary>
        public static string[,] Operators =>
            new[,] {
                { "EQUAL", "==" },
                { "NOTEQUAL", "!=" }
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
        NOTEQUAL = 2
    }
}
