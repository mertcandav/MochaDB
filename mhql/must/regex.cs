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
        /// <param name="row">Reference row.</param>
        public static string GetCommand(string command,MochaRow row) {
            int dex;
            command = command.TrimStart().TrimEnd().Substring(2);
            command = command.Remove(command.Length-1,1);
            while(
                (dex = command.IndexOf('\\')) != -1 &&
                command.Length-1 < dex+1 &&
                char.IsNumber(command[dex+1])) {
                var number = int.Parse(command[dex+1].ToString());
                command =command.Replace($"\\{number}",$"{row.Datas[number]}");
            }
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
