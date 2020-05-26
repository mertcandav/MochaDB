using System.Text.RegularExpressions;

namespace MochaDB.mhql.engine.value {
    /// <summary>
    /// Char value engine of MHQL.
    /// </summary>
    internal static class MhqlEngVal_CHAR {
        /// <summary>
        /// Process value.
        /// </summary>
        /// <param name="val">Value.</param>
        public static void Process(ref string val) {
            val = val.Trim();
            
            if(val == string.Empty)
                throw new MochaException("Char is not defined!");
            if(!val.StartsWith("'"))
                throw new MochaException("Char is not declare!");

            void control(ref string cval) {
                for(int dex = 0; dex < MhqlEngVal_LEXER.Escapes.Length; dex++) {
                    var pattern = new Regex(MhqlEngVal_LEXER.Escapes[dex,0],
                        RegexOptions.Multiline);
                    cval = pattern.Replace(cval,MhqlEngVal_LEXER.Escapes[dex,1]);
                }

                if(!cval.EndsWith("'"))
                    throw new MochaException("Char end is not declared!");

                cval = cval.Substring(0,cval.Length-1);

                if(cval.Length > 1)
                    throw new MochaException("Char can be at most one character!");

                for(int dex = 0; dex < MhqlEngVal_LEXER.EscapeCheck.Length; dex++) {
                    var key = MhqlEngVal_LEXER.EscapeCheck[dex,0];

                    if(key == "\"")
                        continue;

                    if(cval.IndexOf(key) != -1)
                        throw new MochaException($"'{key}' invalid value!");
                }
            }

            control(ref val);
        }
    }
}
