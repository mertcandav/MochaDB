using MochaDB.Mhql;

namespace MochaDB.mhql.functions {
    /// <summary>
    /// MHQL BETWEEN function.
    /// </summary>
    internal class MhqlFunc_BETWEEN {
        /// <summary>
        /// Pass command?
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = command.Split(',');
            if(parts.Length < 3 || parts.Length > 3)
                throw new MochaException("BETWEEN function is cannot processed!");

            int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table,from);
            decimal
                range1,
                range2;

            if(!decimal.TryParse(parts[1].Trim(),out range1))
                throw new MochaException("BETWEEN function is cannot processed!");
            if(!decimal.TryParse(parts[2].Trim(),out range2))
                throw new MochaException("BETWEEN function is cannot processed!");

            decimal value;
            if(!decimal.TryParse(row.Datas[dex].Data.ToString(),out value))
                throw new MochaException("BETWEEN function is cannot processed!");

            return
                    range1 <= range2 ?
                    range1 <= value && value <= range2 :
                    range2 <= value && value <= range1;
        }
    }
}
