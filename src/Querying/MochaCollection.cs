using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Querying {
  /// <summary>
  /// Query extension for MochaCollections.
  /// </summary>
  public static class QueryingMochaCollection {
    /// <summary>
    /// Select items by query.
    /// </summary>
    /// <param name="query">Query to use in filtering.</param>
    public static IEnumerable<T> Select<T>(this MochaCollection<T> mc,Func<T,T> query) =>
        mc.collection.Select(query);

    /// <summary>
    /// Select items by query.
    /// </summary>
    /// <param name="query">Query to use in filtering.</param>
    public static IEnumerable<T> Select<T>(this MochaCollection<T> mc,Func<T,int,T> query) =>
        mc.collection.Select(query);

    /// <summary>
    /// Select items by condition.
    /// </summary>
    /// <param name="query">Query to use in conditioning.</param>
    public static IEnumerable<T> Where<T>(this MochaCollection<T> mc,Func<T,bool> query) =>
        mc.collection.Where(query);

    /// <summary>
    /// Select items by condition.
    /// </summary>
    /// <param name="query">Query to use in conditioning.</param>
    public static IEnumerable<T> Where<T>(this MochaCollection<T> mc,Func<T,int,bool> query) =>
        mc.collection.Where(query);

    /// <summary>
    /// Order items descending by query.
    /// </summary>
    /// <param name="query">Query to use in ordering.</param>
    public static IEnumerable<T> OrderByDescending<T>(this MochaCollection<T> mc,Func<T,T> query) =>
        mc.collection.OrderByDescending(query);

    /// <summary>
    /// Order items ascending by query.
    /// </summary>
    /// <param name="query">Query to use in ordering.</param>
    public static IEnumerable<T> OrderBy<T>(this MochaCollection<T> mc,Func<T,T> query) =>
        mc.collection.OrderBy(query);

    /// <summary>
    /// Group items by query.
    /// </summary>
    /// <param name="query">Query to use in grouping.</param>
    public static IEnumerable<IGrouping<T,T>> GroupBy<T>(this MochaCollection<T> mc,Func<T,T> query) =>
        mc.collection.GroupBy(query);
  }
}
