using System.Text;

namespace MochaDB.Mhql {
    /// <summary>
    /// Formatter for Mhql commands.
    /// </summary>
    public static class MhqlFormatter {
        #region Static

        /// <summary>
        /// Return true if value is Mhql keyword but return false if not.
        /// </summary>
        /// <param name="value">Value to check.</param>
        public static bool IsKeyword(string value) =>
            MochaDbCommand.keywordRegex.IsMatch(value);

        /// <summary>
        /// Replace Mhql keywords to upper case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static void UpperCaseKeywords(ref string value) {
            var matches = MochaDbCommand.keywordRegex.Matches(value);

            var valueSB = new StringBuilder(value);
            for(int index = 0; index < matches.Count; index++) {
                var match = matches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToUpperInvariant(),match.Index,match.Length);
            }
            value = valueSB.ToString();
        }

        /// <summary>
        /// Replace Mhql keywords to upper case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static string UpperCaseKeywords(string value) {
            UpperCaseKeywords(ref value);
            return value;
        }

        /// <summary>
        /// Replace Mhql keywords to lower case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static void LowerCaseKeywords(ref string value) {
            var matches = MochaDbCommand.keywordRegex.Matches(value);

            var valueSB = new StringBuilder(value);
            for(int index = 0; index < matches.Count; index++) {
                var match = matches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToLowerInvariant(),match.Index,match.Length);
            }
            value = valueSB.ToString();
        }

        /// <summary>
        /// Replace Mhql keywords to lower case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static string LowerCaseKeywords(string value) {
            LowerCaseKeywords(ref value);
            return value;
        }

        #endregion
    }
}
