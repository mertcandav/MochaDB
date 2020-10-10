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
      if(!val.EndsWith("'"))
        throw new MochaException("Char end is not declared!");

      void control(ref string cval) {
        cval = cval.Substring(1,cval.Length-2);
        for(int dex = 0; dex < MhqlEngVal_LEXER.Escapes.Length/2; dex++) {
          var pattern = new Regex(MhqlEngVal_LEXER.Escapes[dex,1],
              RegexOptions.Multiline);
          cval = pattern.Replace(cval,MhqlEngVal_LEXER.Escapes[dex,0]);
        }

        if(cval.Length > 1)
          throw new MochaException("Char can be at most one character!");
      }

      control(ref val);
    }
  }
}
