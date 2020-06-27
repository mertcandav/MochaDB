using MochaDB.Mhql;

namespace MochaDB.mhql.must_functions {
    /// <summary>
    /// MHQL BIGGER function of MUST.
    /// </summary>
    internal class MhqlMustFunc_BIGGER {
        /// <summary>
        /// Pass command?
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Pass(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = command.Split(',');
            if(parts.Length != 2)
                throw new MochaException("The BIGGER function can only take 2 parameters!");

            int dex = Mhql_GRAMMAR.GetIndexOfColumn(parts[0],table,from);
            decimal
                range,
                value;

            if(!decimal.TryParse(parts[1].Trim(),out range) ||
                !decimal.TryParse(row.Datas[dex].Data.ToString(),out value))
                throw new MochaException("The parameter of the BIGGER command was not a number!");

            return value >= range;
        }
    }
}
