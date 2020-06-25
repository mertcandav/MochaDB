using System.Text.RegularExpressions;
using MochaDB.Querying;

namespace MochaDB.mhql.engine {
    /// <summary>
    /// MHQL command editor.
    /// </summary>
    internal class MhqlEng_EDITOR {
        /// <summary>
        /// Decompose command ares by brackets.
        /// </summary>
        /// <param name="">Command.</param>
        public static void DecomposeBrackets(ref string command) {
            if(command.FirstChar() == '(') {
                /*var dex = command.IndexOf('&');
                dex =
                    dex == -1 ?
                    command.IndexOf('|') : command.Length;
                if(command[dex] != ')')
                    throw new MochaException("Bracket is open but not closed!");

                command = command.Substring(1,dex-1);*/
                if(command.LastChar() != '(')
                    throw new MochaException("Bracket is open but not closed!");
                command = command.Substring(1,command.Length-1);
            }
        }

        /// <summary>
        /// Returns decompose command ares by brackets.
        /// </summary>
        /// <param name="value">Value to decompose.</param>
        public static string DecomposeBrackets(string value) {
            int dex;
            if((dex = value.IndexOf('(')) != -1) {
                return value.Substring(dex+1,value.Length-dex-2);
            }
            return value;
        }

        /// <summary>
        /// Remove all comments from code.
        /// </summary>
        /// <param name="command">Command.</param>
        public static void RemoveComments(ref string command) {
            var multiline = new Regex(@"/\*.*?\*/",RegexOptions.Singleline);
            command = multiline.Replace(command,string.Empty);
            var singleline = new Regex(@"//.*$",RegexOptions.Multiline);
            command = singleline.Replace(command,string.Empty);
            command = command.Trim();
        }
    }
}
