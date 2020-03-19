using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Querying {
    /// <summary>
    /// Query extension for MochaArrays.
    /// </summary>
    public static class QueryingMochaArray {
        /// <summary>
        /// Select items by query.
        /// </summary>
        /// <param name="query">Query to use in filtering.</param>
        public static IEnumerable<T> Select<T>(this MochaArray<T> mc,Func<T,T> query) =>
            mc.array.Select(query);

        /// <summary>
        /// Select items by query.
        /// </summary>
        /// <param name="query">Query to use in filtering.</param>
        public static IEnumerable<T> Select<T>(this MochaArray<T> mc,Func<T,int,T> query) =>
            mc.array.Select(query);

        /// <summary>
        /// Select items by condition.
        /// </summary>
        /// <param name="query">Query to use in conditioning.</param>
        public static IEnumerable<T> Where<T>(this MochaArray<T> mc,Func<T,bool> query) =>
            mc.array.Where(query);

        /// <summary>
        /// Select items by condition.
        /// </summary>
        /// <param name="query">Query to use in conditioning.</param>
        public static IEnumerable<T> Where<T>(this MochaArray<T> mc,Func<T,int,bool> query) =>
            mc.array.Where(query);

        /// <summary>
        /// Order items descending by query.
        /// </summary>
        /// <param name="query">Query to use in ordering.</param>
        public static IEnumerable<T> OrderByDescending<T>(this MochaArray<T> mc,Func<T,T> query) =>
            mc.array.OrderByDescending(query);

        /// <summary>
        /// Order items ascending by query.
        /// </summary>
        /// <param name="query">Query to use in ordering.</param>
        public static IEnumerable<T> OrderBy<T>(this MochaArray<T> mc,Func<T,T> query) =>
            mc.array.OrderBy(query);

        /// <summary>
        /// Group items by query.
        /// </summary>
        /// <param name="query">Query to use in grouping.</param>
        public static IEnumerable<IGrouping<T,T>> GroupBy<T>(this MochaArray<T> mc,Func<T,T> query) =>
            mc.array.GroupBy(query);
    }
}
