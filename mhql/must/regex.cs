using System.Text.RegularExpressions;

namespace MochaDB.mhql.must {
    /// <summary>
    /// REGEX functions of MUST.
    /// </summary>
    internal class MhqlMust_REGEX {
        /// <summary>
        /// Returns Regex command by row.
        /// </summary>
        /// <param name="command">Command.</param>
        public static string GetCommand(string command) {
            command = command.Substring(2);
            command = command.Remove(command.Length-1,1);
            return command;
        }

        /// <summary>
        /// Execute regex and returns result.
        /// </summary>
        /// <param name="pattern">Regex pattern.</param>
        /// <param name="value">Value.</param>
        public static bool Match(string pattern,string value) {
            var regex = new Regex(pattern);
            var result = regex.IsMatch(value);
            return result;
        }
    }
}
