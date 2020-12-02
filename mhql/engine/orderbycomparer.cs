namespace MochaDB.mhql.engine {
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Comparer for mhql orderby.
  /// </summary>
  internal sealed class ORDERBYComparer:IComparer<string> {
    /// <summary>
    /// Method to determine if a string is a number.
    /// </summary>
    /// <param name="value">String to test.</param>
    /// <returns>True if numeric</returns>
    public static bool IsNumeric(string value) =>
        decimal.TryParse(value,out _);

    /// <summary>
    /// Compare values.
    /// </summary>
    /// <param name="s1">Value 1.</param>
    /// <param name="s2">Value 2.</param>
    public int Compare(string s1,string s2) {
      const int S1GreaterThanS2 = 1;
      const int S2GreaterThanS1 = -1;

      bool IsNumeric1 = IsNumeric(s1);
      bool IsNumeric2 = IsNumeric(s2);

      if(IsNumeric1 && IsNumeric2) {
        decimal i1 = Convert.ToDecimal(s1);
        decimal i2 = Convert.ToDecimal(s2);
        if(i1 > i2)
          return S1GreaterThanS2;
        if(i1 < i2)
          return S2GreaterThanS1;
        return 0;
      }
      if(IsNumeric1)
        return S2GreaterThanS1;
      if(IsNumeric2)
        return S1GreaterThanS2;
      return string.Compare(s1,s2,true);
    }
  }
}
