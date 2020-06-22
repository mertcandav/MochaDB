using System.Collections.Generic;
using System.Text;
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
        public static List<string> GetParts(string command) {
            var pattern = new Regex("AND",
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var parts = new List<string>();
            var value = new StringBuilder();
            var count = 0;
            for(int index = 0; index < command.Length; index++) {
                var currentChar = command[index];
                if(currentChar == 'A' || currentChar == 'a') {
                    if(command.Length - 1 - index >= 3) {
                        if(count == 0 && pattern.IsMatch(command.Substring(index,3))) {
                            parts.Add(value.ToString().Trim());
                            value.Clear();
                            index+=2;
                            continue;
                        }
                    }
                } else if(currentChar == '(')
                    count++;
                else if(currentChar == ')')
                    count--;

                value.Append(currentChar);
            }
            parts.Add(value.ToString().Trim());
            return parts;
        }
    }
}
