namespace MochaDB.mhql.engine.value {
  using System.Text.RegularExpressions;

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

      val = val.Substring(1,val.Length-2);
      for(int dex = 0; dex < MhqlEngVal_LEXER.Escapes.Length/2; ++dex) {
        Regex pattern = new Regex(MhqlEngVal_LEXER.Escapes[dex,1],
            RegexOptions.Multiline);
        val = pattern.Replace(val,MhqlEngVal_LEXER.Escapes[dex,0]);
      }
      if(val.Length > 1)
        throw new MochaException("Char can be at most one character!");
    }
  }
}
