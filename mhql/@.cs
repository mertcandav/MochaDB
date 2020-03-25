using MochaDB.Querying;
using MochaDB.Mhql;

namespace MochaDB.mhql {
    /// <summary>
    /// MHQL @ mark.
    /// </summary>
    internal class Mhql_AT {
        /// <summary>
        /// Returns use command.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="final">Command of removed use commands.</param>
        public static string GetAT(string command,out string final) {
            if(command.FirstChar() != '@') {
                final = command;
                return null;
            }

            int finaldex = MochaDbCommand.mainkeywordRegex.Match(command).Index;
            if(finaldex==0)
                throw new MochaException("@ mark is cannot processed!");
            
            var atcommand = command.Substring(0,finaldex).TrimStart().TrimEnd();
            final = command.Substring(finaldex);
            return atcommand;
        }
    }
}
