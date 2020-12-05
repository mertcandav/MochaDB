namespace MochaDB.mhql {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  using MochaDB.Querying;

  /// <summary>
  /// Grammar of MHQL.
  /// </summary>
  internal static class Mhql_GRAMMAR {
    #region Members

    /// <summary>
    /// Returns column index.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="columns">Columns.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static int GetIndexOfColumn(string value,MochaColumn[] columns,bool from) {
      int returndex() {
        int columndex;
        if(!int.TryParse(value,out columndex))
          throw new MochaException("Column index or name is cannot processed!");
        return columndex;
      }
      value = value.Trim();
      if(!from)
        return returndex();
      IEnumerable<MochaColumn> result = columns.Where(x => x.Name == value);
      if(result.Count() == 0)
        return returndex();
      return Array.IndexOf(columns,result.First());
    }

    /// <summary>
    /// Returns column index.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="cols">Columns.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static int GetIndexOfColumn(string value,MochaCollectionResult<MochaColumn> cols,bool from) {
      int returndex() {
        int columndex;
        if(!int.TryParse(value,out columndex))
          throw new MochaException("Column index or name is cannot processed!");
        return columndex;
      }
      value = value.Trim();
      if(!from)
        return returndex();
      IEnumerable<MochaColumn> result = cols.Where(x => x.Name == value);
      if(result.Count() == 0)
        return returndex();
      return cols.IndexOf(result.First());
    }

    #endregion Members

    #region Properties

    /// <summary>
    /// Functions of must.
    /// </summary>
    public static string[] MustFunctions =>
      new[] {
        "BETWEEN",
        "BIGGER",
        "LOWER",
        "EQUAL",
        "NOTEQUAL",
        "STARTW",
        "ENDW",
        "CONTAINS",
        "NOTCONTAINS",
        "NOTSTARTW",
        "NOTENDW"
      };

    /// <summary>
    /// Functions of use.
    /// </summary>
    public static Dictionary<string/* Pattern */,string/* Tag */> UseFunctions =>
      new Dictionary<string,string>() {
        { "COUNT(\\s*)\\((\\s*)\\)", "COUNT" },
        { "SUM(\\s*)\\((\\s*).*(\\s*)\\)", "SUM" },
        { "MAX(\\s*)\\((\\s*).*(\\s*)\\)", "MAX" },
        { "MIN(\\s*)\\((\\s*).*(\\s*)\\)", "MIN" },
        { "AVG(\\s*)\\((\\s*).*(\\s*)\\)", "AVG" }
      };

    /// <summary>
    /// Main keywords.
    /// </summary>
    public static string MainKeywords =>
      "USE|ORDERBY|MUST|GROUPBY|SELECT|SUBROW|SUBCOL|DELROW|DELCOL|ADDROW|CORDERBY";

    /// <summary>
    /// All words.
    /// </summary>
    public static Regex FullRegex => new Regex(
$@"\b({MainKeywords}|ASC|DESC|AND|FROM|AS|TRUE|FALSE|IN|INEQ|\$BETWEEN|\$BIGGER|\$LOWER|\$EQUAL|\$STARTW|\$ENDW|" +
$@"\$NOTEQUAL|\$CONTAINS|\$NOTCONTAINS|\$NOTSTARTW|\$NOTENDW)\b",
RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

    /// <summary>
    /// Main keywrods.
    /// </summary>
    public static Regex MainRegex => new Regex(
$@"\b({MainKeywords})\b|\s*$",
RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

    #endregion Properties
  }
}
