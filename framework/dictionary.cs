namespace MochaDB.framework {
  using System.Collections.Generic;
  using System.Text.RegularExpressions;

  /// <summary>
  /// Dictionary framework of MochaDB.
  /// </summary>
  internal static class Framework_DICTIONARY {
    /// <summary>
    /// Return true if contains matched key, returns false if not.
    /// </summary>
    /// <typeparam name="T1">Type 1.</typeparam>
    /// <typeparam name="T2">Type 2.</typeparam>
    /// <param name="dict">Dictionary to find.</param>
    /// <param name="key">Key.</param>
    public static bool MatchKey<T1, T2>(this Dictionary<T1,T2> dict,string key) {
      foreach(T1 k in dict.Keys)
        if(
            new Regex(k.ToString(),
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant).IsMatch(key)
            )
          return true;
      return false;
    }

    /// <summary>
    /// Return value if contains matched key, returns null if not.
    /// </summary>
    /// <typeparam name="T1">Type 1.</typeparam>
    /// <typeparam name="T2">Type 2.</typeparam>
    /// <param name="dict">Dictionary to find.</param>
    /// <param name="key">Key.</param>
    public static object GetValueByMatchKey<T1, T2>(this Dictionary<T1,T2> dict,string key) {
      foreach(T1 k in dict.Keys)
        if(
            new Regex(k.ToString(),
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant).IsMatch(key)
            )
          return dict[k];
      return null;
    }
  }
}
