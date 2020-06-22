using System.Linq;
using MochaDB.Mhql;

namespace MochaDB.mhql {
    /// <summary>
    /// Grammar of MHQL.
    /// </summary>
    internal static class Mhql_GRAMMAR {
        #region Members

        /// <summary>
        /// Returns column index.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="table">Table.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static int GetIndexOfColumn(string value,MochaTableResult table,bool from) {
            int returndex() {
                int columndex;
                if(!int.TryParse(value,out columndex))
                    throw new MochaException("Column index or name is cannot processed!");
                return columndex;
            }

            value = value.Trim();
            if(!from)
                return returndex();

            var result = table.Columns.Where(x => x.Name == value);

            if(result.Count() == 0)
                return returndex();

            return table.Columns.IndexOf(result.First());
        }

        #endregion

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
                "NOTCONTAINS"
            };

        #endregion
    }
}
