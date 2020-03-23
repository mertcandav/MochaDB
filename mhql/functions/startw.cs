namespace MochaDB.mhql.functions {
    /// <summary>
    /// STARTW function.
    /// </summary>
    internal class MhqlFunc_STARTW {
        /// <summary>
        /// Pass command?
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="row">Row.</param>
        public static bool Pass(string command,MochaRow row) {
            var parts = command.Split(',');
            if(parts.Length < 2 || parts.Length > 2)
                throw new MochaException("STARTW function is cannot processed!");

            int dex;
            if(!int.TryParse(parts[0].TrimStart().TrimEnd(),out dex))
                throw new MochaException("STARTW function is cannot processed!");

            return
                row.Datas[dex].Data.ToString().StartsWith(parts[1]);
        }
    }
}
