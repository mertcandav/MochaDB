using System.Text.RegularExpressions;

namespace MochaDB.mhql.engine.value {
  /// <summary>
  /// Char value engine of MHQL.
  /// </summary>
  internal static class MhqlEngVal_STRING {
    /// <summary>
    /// Process value.
    /// </summary>
    /// <param name="val">Value.</param>
    public static void Process(ref string val) {
      val = val.Trim();

      if(val == string.Empty)
        throw new MochaException("String is not defined!");
      if(!val.StartsWith("\""))
        throw new MochaException("String is not declare!");
      if(!val.EndsWith("\""))
        throw new MochaException("String end is not declared!");

      val = val.Substring(1,val.Length-2);
      for(int index = 0; index < MhqlEngVal_LEXER.Escapes.Length/2; ++index) {
        Regex pattern = new Regex(MhqlEngVal_LEXER.Escapes[index,1],
            RegexOptions.Multiline);
        val = pattern.Replace(val,MhqlEngVal_LEXER.Escapes[index,0]);
      }
    }
  }
}
