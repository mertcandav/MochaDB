  using System;

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
            return command.Split(new[] {
                "AND","and","And","ANd","anD","aND","AnD"
            },StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
