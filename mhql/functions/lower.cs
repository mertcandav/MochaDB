using MochaDB.Mhql;

namespace MochaDB.mhql.functions {
    /// <summary>
    /// MHQL LOWER function.
    /// </summary>
    internal class MhqlFunc_LOWER {
        /// <summary>
        /// Pass command?
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = command.Split(',');
            if(parts.Length < 2 || parts.Length > 2)
                throw new MochaException("LOWER function is cannot processed!");

            int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table,from);
            decimal
                range,
                value;

            if(!decimal.TryParse(parts[1].Trim(),out range) ||
                !decimal.TryParse(row.Datas[dex].Data.ToString(),out value))
                throw new MochaException("The parameter of the LOWER command was not a number!");

            return value <= range;
        }
    }
}
