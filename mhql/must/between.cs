namespace MochaDB.mhql.must {
    /// <summary>
    /// BETWEEN functions of MUST.
    /// </summary>
    internal class MhqlMust_BETWEEN {
        /// <summary>
        /// Pass command?
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="row">Row.</param>
        public static bool Pass(string command,MochaRow row) {
            var parts = command.Split(',');
            if(parts.Length < 3 || parts.Length > 3)
                throw new MochaException("BETWEEN is cannot processed!");

            int dex;
            decimal
                range1,
                range2;

            if(!int.TryParse(parts[0].TrimStart().TrimEnd(),out dex))
                throw new MochaException("BETWEEN is cannot processed!");
            if(!decimal.TryParse(parts[1].TrimStart().TrimEnd(),out range1))
                throw new MochaException("BETWEEN is cannot processed!");
            if(!decimal.TryParse(parts[2].TrimStart().TrimEnd(),out range2))
                throw new MochaException("BETWEEN is cannot processed!");

            decimal value;
            if(!decimal.TryParse(row.Datas[dex].Data.ToString(),out value))
                throw new MochaException("BETWEEN is cannot processed!");

            return
                    range1 <= range2 ?
                    range1 <= value && value <= range2 :
                    range2 <= value && value <= range1;
        }
    }
}
