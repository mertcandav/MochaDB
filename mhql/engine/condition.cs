using System;
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
        #region PRIVITE

        /// <summary>
        /// Value output for conditions.
        /// </summary>
        public struct CONDITIONVAL {
            /// <summary>
            /// Returns true if equals, returns false if not.
            /// </summary>
            /// <param name="v">Value to compare.</param>
            public bool __EQUALS__(CONDITIONVAL v) =>
                VALUE.ToString() == v.VALUE.ToString();

            /// <summary>
            /// Returns true if not equals, returns false if not.
            /// </summary>
            /// <param name="v">Value to compare.</param>
            public bool __NEQUALS__(CONDITIONVAL v) =>
                VALUE.ToString() != v.VALUE.ToString();

            /// <summary>
            /// Value.
            /// </summary>
            public object VALUE;
            /// <summary>
            /// Type of value.
            /// </summary>
            public CONDITIONVAL_TYPE TYPE;
        }

        /// <summary>
        /// Types for values of <see cref="CONDITIONVAL"/>.
        /// </summary>
        public enum CONDITIONVAL_TYPE:short {
            /// <summary>
            /// String.
            /// </summary>
            __STRING__ = 0x1,
            /// <summary>
            /// Char.
            /// </summary>
            __CHAR__ = 0x2,
            /// <summary>
            /// Boolean.
            /// </summary>
            __BOOLEAN__ = 0x3,
            /// <summary>
            /// Integer.
            /// </summary>
            __ARITHMETIC__ = 0x4,
        }

        #endregion PRIVITE

        /// <summary>
        /// Process condition and returns condition result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process(string command,MochaTableResult table,MochaRow row,bool from) {
            ConditionType type;
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

            for(int index = 0; index < MhqlEng_CONDITION_LEXER.Operators.Length/2; index++) {
                string
                    key,
                    value;
                value = MhqlEng_CONDITION_LEXER.Operators[index,1];
                if(!command.Contains(value))
                    continue;

                key = MhqlEng_CONDITION_LEXER.Operators[index,0];
                type = (ConditionType)Enum.Parse(typeof(ConditionType),key);
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
            CHKVAL(value0,value1);
            return value0.__EQUALS__(value1);
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
            CHKVAL(value0,value1);
            return value0.__NEQUALS__(value1);
        }

        /// <summary>
        /// Returns condition parts.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="operator">Operator.</param>
        public static string[] GetConditionParts(string command,string @operator) {
            var parts = command.Split(new[] { @operator },2,0);
            if(parts.Length < 2)
                throw new MochaException("Condition is cannot processed!");
            parts[0] = parts[0].Trim();
            parts[1] = parts[1].Trim();
            return parts;
        }

        /// <summary>
        /// Check <see cref="CONDITIONVAL"/>.
        /// </summary>
        /// <param name="v1">Value 1.</param>
        /// <param name="v2">Value 2.</param>
        public static void CHKVAL(CONDITIONVAL v1,CONDITIONVAL v2) {
            Console.WriteLine(v1.VALUE);
            Console.WriteLine(v2.VALUE);
            if(v1.TYPE != v2.TYPE)
                throw new MochaException("Value types is are not compatible!");
        }

        /// <summary>
        /// Returns value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static CONDITIONVAL GetValue(string value,MochaTableResult table,MochaRow row,bool from) {
            if(value.StartsWith("'")) {
                MhqlEngVal_CHAR.Process(ref value);
                return new CONDITIONVAL {
                    TYPE = CONDITIONVAL_TYPE.__CHAR__,
                    VALUE = value
                };
            } else if(value.StartsWith("\"")) {
                MhqlEngVal_STRING.Process(ref value);
                return new CONDITIONVAL {
                    TYPE = CONDITIONVAL_TYPE.__STRING__,
                    VALUE = value
                };
            } else if(value.StartsWith("#")) {
                decimal val;
                if(!decimal.TryParse(value.Substring(1).Replace('.',','),out val))
                    throw new MochaException("Value is not arithmetic value!");
                return new CONDITIONVAL {
                    TYPE = CONDITIONVAL_TYPE.__ARITHMETIC__,
                    VALUE = val
                };
            } else if(value == "TRUE")
                return new CONDITIONVAL {
                    TYPE = CONDITIONVAL_TYPE.__BOOLEAN__,
                    VALUE = true
                };
            else if(value == "FALSE")
                return new CONDITIONVAL {
                    TYPE = CONDITIONVAL_TYPE.__BOOLEAN__,
                    VALUE = false
                };

            if(from) {
                var result = table.Columns.Where(x => x.Name == value);

                if(result.Count() == 0) {
                    goto index;
                }

                var data = row.Datas[Array.IndexOf(table.Columns,result.First())];
                return new CONDITIONVAL {
                    TYPE =
                        data.dataType == MochaDataType.Unique ||
                        data.dataType == MochaDataType.String ||
                        data.dataType == MochaDataType.DateTime ?
                            CONDITIONVAL_TYPE.__STRING__ :
                                data.dataType == MochaDataType.Char ?
                                CONDITIONVAL_TYPE.__CHAR__ :
                                    data.dataType == MochaDataType.Boolean ?
                                    CONDITIONVAL_TYPE.__BOOLEAN__ :
                                        CONDITIONVAL_TYPE.__ARITHMETIC__,
                    VALUE = data.data
                };
            }

        index:

            if(!char.IsNumber(value.FirstChar()))
                throw new MochaException("Column is not defined!");
            var dex = int.Parse(value);
            if(dex < 0)
                throw new MochaException("Index is cannot lower than zero!");
            else if(dex > row.Datas.MaxIndex())
                throw new MochaException("The specified index is more than the number of columns!");
            var _data = row.Datas[dex];
            return new CONDITIONVAL {
                TYPE =
                    _data.dataType == MochaDataType.Unique ||
                    _data.dataType == MochaDataType.String ||
                    _data.dataType == MochaDataType.DateTime ?
                    CONDITIONVAL_TYPE.__STRING__ :
                        _data.dataType == MochaDataType.Char ?
                        CONDITIONVAL_TYPE.__CHAR__ :
                            _data.dataType == MochaDataType.Boolean ?
                            CONDITIONVAL_TYPE.__BOOLEAN__ :
                                CONDITIONVAL_TYPE.__ARITHMETIC__,
                VALUE = _data.data
            };
        }
    }
}
