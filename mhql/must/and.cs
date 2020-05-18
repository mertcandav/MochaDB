using System.Collections.Generic;
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
            var pattern = new Regex("AND",
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var parts = new List<string>();
            var value = string.Empty;
            var count = 0;
            for(int index = 0; index < command.Length; index++) {
                var currentChar = command[index];
                if(currentChar == 'A' || currentChar == 'a') {
                    if(command.Length - 1 - index >= 3) {
                        if(count == 0 && pattern.IsMatch(command.Substring(index,3))) {
                            parts.Add(value.Trim());
                            value = string.Empty;
                            index+=2;
                            continue;
                        }
                    }
                } else if(currentChar == '(')
                    count++;
                else if(currentChar == ')')
                    count--;

                value += currentChar;
            }
            parts.Add(value.Trim());
            return new MochaArray<string>(parts);
        }
    }
}
