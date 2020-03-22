using System;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL AS keyword.
    /// </summary>
    internal class Mhql_AS {
        /// <summary>
        /// Returns as name.
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="final">As removed command.</param>
        public static string GetAS(ref string command) {
            var dex = command.IndexOf("AS",StringComparison.OrdinalIgnoreCase);
            if(dex==-1)
                return command;

            var name = command.Substring(dex+2).TrimStart().TrimEnd();
            command = command.Substring(0,dex).TrimStart().TrimEnd();
            return name;
        }
    }
}
