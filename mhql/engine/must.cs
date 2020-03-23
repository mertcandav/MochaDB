using System;
using MochaDB.mhql.must;
using MochaDB.mhql.functions;
using MochaDB.Querying;

namespace MochaDB.mhql.engine {
    /// <summary>
    /// MHQL MUST core.
    /// </summary>
    internal class MhqlEng_MUST {
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

        /// <summary>
        /// Returns command must result.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="row">Row.</param>
        public static bool IsPassTable(ref string command,MochaRow row) {
            command=command.TrimStart().TrimEnd();
            if(char.IsNumber(command.FirstChar())) {
                return MhqlMust_REGEX.Match(
                                MhqlMust_REGEX.GetCommand(command,row),
                                (string)GetDataFromCommand(command,row));
            } else if(command.StartsWith("BETWEEN(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
                return MhqlFunc_BETWEEN.Pass(command.Substring(8,command.Length-9),row);
            } else if(command.StartsWith("BIGGER(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
                return MhqlFunc_BIGGER.Pass(command.Substring(7,command.Length-8),row);
            } else if(command.StartsWith("LOWER(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
                return MhqlFunc_LOWER.Pass(command.Substring(6,command.Length-7),row);
            } else if(command.StartsWith("EQUAL(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
                return MhqlFunc_EQUAL.Pass(command.Substring(6,command.Length-7),row);
            } else if(command.StartsWith("STARTW(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
                return MhqlFunc_STARTW.Pass(command.Substring(7,command.Length-8),row);
            } else
                throw new MochaException($"'{command}' is cannot processed!");
        }
    }
}
