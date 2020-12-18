namespace MochaDB.framework {
  using System.Collections.Generic;
  using System.Linq;
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
      for(int index = 0; index < dict.Keys.Count; ++index)
        if(
            new Regex(dict.Keys.ElementAt(index).ToString(),
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
      for(int index = 0; index < dict.Keys.Count; ++index)
        if(
            new Regex(dict.Keys.ElementAt(index).ToString(),
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant).IsMatch(key)
            )
          return dict.Values.ElementAt(index);
      return null;
    }

    /// <summary>
    /// Returns value by key.
    /// </summary>
    /// <typeparam name="T1">Key type..</typeparam>
    /// <typeparam name="T2">Value type.</typeparam>
    /// <param name="dict">Dictionary.</param>
    /// <param name="key">Key.</param>
    public static T2 GetValue<T1, T2>(this Dictionary<T1,T2> dict,T1 key) {
      T2 @out;
      dict.TryGetValue(key,out @out);
      return @out;
    }
  }
}
