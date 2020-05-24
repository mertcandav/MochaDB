namespace MochaDB.mhql.functions {
    /// <summary>
    /// MHQL NOTEQUAL function.
    /// </summary>
    internal class MhqlFunc_NOTEQUAL {
        /// <summary>
        /// Pass command?
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="row">Row.</param>
        public static bool Pass(string command,MochaRow row) {
            var parts = command.Split(',');
            if(parts.Length < 2 || parts.Length > 2)
                throw new MochaException("EQUAL function is cannot processed!");

            int dex;

            if(!int.TryParse(parts[0].Trim(),out dex))
                throw new MochaException("EQUAL function is cannot processed!");

            return parts[1] != row.Datas[dex].Data.ToString();
        }
    }
}
