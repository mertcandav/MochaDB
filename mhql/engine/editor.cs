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
    }
}
