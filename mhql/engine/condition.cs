using System.Linq;
using System.Text.RegularExpressions;
using MochaDB.mhql.engine.value;
using MochaDB.Mhql;
using MochaDB.Querying;

namespace MochaDB.mhql.engine {
    /// <summary>
    /// Condition engine for MHQL.
    /// </summary>
    internal static class MhqlEng_CONDITION {
        /// <summary>
        /// Process condition and returns condition result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process(string command,MochaTableResult table,MochaRow row,bool from) {
            ConditionType type = ConditionType.None;
            if(!IsCondition(command,out type))
                return false;

            if(type == ConditionType.EQUAL)
                return Process_EQUAL(command,table,row,from);
            else if(type == ConditionType.NOTEQUAL)
                return Process_NOTEQUAL(command,table,row,from);
            return false;
        }

        /// <summary>
        /// Returns true if command is condition but returns false if not.
        /// </summary>
        /// <param name="command">Command to check.</param>
        /// <param name="type">Type of condition.</param>
        public static bool IsCondition(string command,out ConditionType type) {
            if(new Regex(".*\\(").IsMatch(command)) {
                type = ConditionType.None;
                return false;
            }

            if(command.Contains("==")) {
                type = ConditionType.EQUAL;
                return true;
            } else if(command.Contains("!=")) {
                type = ConditionType.NOTEQUAL;
                return true;
            }

            type = ConditionType.None;
            return false;
        }

        /// <summary>
        /// Process equal condition and returns result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process_EQUAL(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = GetConditionParts(command,"==");
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            return value0 == value1;
        }

        /// <summary>
        /// Process not equal condition and returns result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process_NOTEQUAL(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = GetConditionParts(command,"!=");
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            return value0 != value1;
        }

        /// <summary>
        /// Returns condition parts.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="operator">Operator.</param>
        public static string[] GetConditionParts(string command,string @operator) {
            var parts = command.Split(new[] { @operator },0);
            if(parts.Length != 2)
                throw new MochaException("Condition is cannot processed!");
            parts[0] = parts[0].Trim();
            parts[1] = parts[1].Trim();
            return parts;
        }

        /// <summary>
        /// Returns value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static string GetValue(string value,MochaTableResult table,MochaRow row,bool from) {
            if(value.StartsWith("'")) {
                MhqlEngVal_CHAR.Process(ref value);
                return value;
            } else if(value.StartsWith("\"")) {
                MhqlEngVal_STRING.Process(ref value);
                return value;
            }

            if(from) {
                var result = table.Columns.Where(x => x.Name == value);

                if(result.Count() == 0) {
                    goto index;
                }

                return row.Datas[table.Columns.IndexOf(result.First())].ToString();
            }

        index:

            if(!char.IsNumber(value.FirstChar()))
                throw new MochaException("Column is not defined!");
            var dex = int.Parse(value);
            if(dex < 0)
                throw new MochaException("Index is cannot lower than zero!");
            else if(dex > row.Datas.MaxIndex())
                throw new MochaException("The specified index is more than the number of columns!");
            return row.Datas[dex].ToString();
        }
    }
}
