using System.Text;

namespace MochaDB.Mhql {
    /// <summary>
    /// Formatter for Mhql commands.
    /// </summary>
    public static class MhqlFormatter {
        /// <summary>
        /// Returns true if value is Mhql keyword or function but return false if not.
        /// </summary>
        /// <param name="value">Value to check.</param>
        public static bool IsObject(string value) =>
            MochaDbCommand.fullRegex.IsMatch(value);

        /// <summary>
        /// Replace Mhql keywords and functions to upper case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static void UpperCaseObjects(ref string value) {
            var matches = MochaDbCommand.fullRegex.Matches(value);

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
        /// Replace Mhql keywords and functions to upper case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static string UpperCaseObjects(string value) {
            UpperCaseObjects(ref value);
            return value;
        }

        /// <summary>
        /// Replace Mhql keywords and functions to lower case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static void LowerCaseObjects(ref string value) {
            var matches = MochaDbCommand.fullRegex.Matches(value);

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
        /// Replace Mhql keywords and functions to lower case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static string LowerCaseObjects(string value) {
            LowerCaseObjects(ref value);
            return value;
        }
    }
}
