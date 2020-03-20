using System.Collections.Generic;
using MochaDB.Querying;

namespace MochaDB.mhql.engine {
    /// <summary>
    /// MHQL MUST core.
    /// </summary>
    internal class MhqlEng_MUST {
        /// <summary>
        /// Returns seperated commands by brackets.
        /// </summary>
        /// <param name="command">Command.</param>
        public static MochaArray<string> GetMusts(string command) {
            var parts = new List<string>();
            var dex = 0;
            while(dex != -1) {
                var finaldex = command.IndexOf('&',dex);
                finaldex =
                    finaldex == -1 ?
                    command.IndexOf('|',dex) : dex;

                if(finaldex != -1)
                    parts.Add(command.Substring(dex,finaldex-dex));
                else
                    parts.Add(command.Substring(dex));
            }

            return parts.ToArray();
        }

        /// <summary>
        /// Returns data by command.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="row">Base row.</param>
        public static MochaData GetDataFromCommand(string command,MochaRow row) {
            command = command.TrimStart().TrimEnd();
            if(!char.IsNumber(command.FirstChar()))
                throw new MochaException("Column is not defined!");
            var dex = int.Parse(command.FirstChar().ToString());

            if(dex < 0)
                throw new MochaException("Index is cannot lower than zero!");
            else if(dex > row.Datas.MaxIndex())
                throw new MochaException("The specified index is more than the number of columns!");

            return row.Datas[dex];
        }
    }
}
