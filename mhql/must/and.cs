using System.Text.RegularExpressions;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL AND keyword.
    /// </summary>
    internal class Mhql_AND {
        /// <summary>
        /// Returns seperated commands by or.
        /// </summary>
        /// <param name="command">Command.</param>
        public static MochaArray<string> GetParts(string command) {
            var regex = new Regex(@"AND",RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            var parts = regex.Split(command);
            for(int index = 0; index < parts.Length-1; index++) {
                parts[index] = parts[index].Trim();
            }
            return parts;
        }
    }
}
