using System;
using System.Linq;
using System.Text.RegularExpressions;
using MochaDB.framework;
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
            /// Returns true if bigger, returns false if not.
            /// </summary>
            /// <param name="v">Value to compare.</param>
            public bool __BIGGER__(CONDITIONVAL v) {
                if(TYPE == CONDITIONVAL_TYPE.__BOOLEAN__)
                    return VALUE.ToString() == "True" && v.VALUE.ToString() == "False";
                else if(TYPE == CONDITIONVAL_TYPE.__CHAR__)
                    return (int)VALUE > (int)v.VALUE;
                else if(TYPE == CONDITIONVAL_TYPE.__ARITHMETIC__) {
                    return decimal.Parse(VALUE.ToString()) > decimal.Parse(v.VALUE.ToString());
                }
                throw new MochaException("BIGGER operator is cannot compatible this data type!");
            }

            /// <summary>
            /// Returns true if lower, returns false if not.
            /// </summary>
            /// <param name="v">Value to compare.</param>
            public bool __LOWER__(CONDITIONVAL v) {
                if(TYPE == CONDITIONVAL_TYPE.__BOOLEAN__)
                    return VALUE.ToString() == "False" && v.VALUE.ToString() == "True";
                else if(TYPE == CONDITIONVAL_TYPE.__CHAR__)
                    return (int)VALUE < (int)v.VALUE;
                else if(TYPE == CONDITIONVAL_TYPE.__ARITHMETIC__) {
                    return decimal.Parse(VALUE.ToString()) < decimal.Parse(v.VALUE.ToString());
                }
                throw new MochaException("LOWER operator is cannot compatible this data type!");
            }

            /// <summary>
            /// Returns true if biggereq, returns false if not.
            /// </summary>
            /// <param name="v">Value to compare.</param>
            public bool __BIGGEREQ__(CONDITIONVAL v) {
                if(TYPE == CONDITIONVAL_TYPE.__BOOLEAN__)
                    return
                        (VALUE.ToString() == "True" && v.VALUE.ToString() == "False") ||
                        (VALUE.ToString() == v.VALUE.ToString());
                else if(TYPE == CONDITIONVAL_TYPE.__CHAR__)
                    return (int)VALUE >= (int)v.VALUE;
                else if(TYPE == CONDITIONVAL_TYPE.__ARITHMETIC__) {
                    return decimal.Parse(VALUE.ToString()) >= decimal.Parse(v.VALUE.ToString());
                }
                throw new MochaException("BIGGEREQ operator is cannot compatible this data type!");
            }

            /// <summary>
            /// Returns true if lowereq, returns false if not.
            /// </summary>
            /// <param name="v">Value to compare.</param>
            public bool __LOWEREQ__(CONDITIONVAL v) {
                if(TYPE == CONDITIONVAL_TYPE.__BOOLEAN__)
                    return
                        (VALUE.ToString() == "False" && v.VALUE.ToString() == "True") ||
                        (VALUE.ToString() == v.VALUE.ToString());
                else if(TYPE == CONDITIONVAL_TYPE.__CHAR__)
                    return (int)VALUE <= (int)v.VALUE;
                else if(TYPE == CONDITIONVAL_TYPE.__ARITHMETIC__) {
                    return decimal.Parse(VALUE.ToString()) <= decimal.Parse(v.VALUE.ToString());
                }
                throw new MochaException("LOWEREQ operator is cannot compatible this data type!");
            }

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
            else if(type == ConditionType.BIGGER)
                return Process_BIGGER(command,table,row,from);
            else if(type == ConditionType.LOWER)
                return Process_LOWER(command,table,row,from);
            else if(type == ConditionType.BIGGEREQ)
                return Process_BIGGEREQ(command,table,row,from);
            else if(type == ConditionType.LOWEREQ)
                return Process_LOWEREQ(command,table,row,from);
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

            for(int index = 0; index < MhqlEng_CONDITION_LEXER.__OPERATORS__.Count; index++) {
                string
                    key,
                    value;
                value = MhqlEng_CONDITION_LEXER.__OPERATORS__.Values.ElementAt(index);
                if(!command.Contains(value))
                    continue;

                key = MhqlEng_CONDITION_LEXER.__OPERATORS__.Keys.ElementAt(index);
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
            var parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.__OPERATORS__.GetValue("EQUAL"));
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
            var parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.__OPERATORS__.GetValue("NOTEQUAL"));
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            CHKVAL(value0,value1);
            return value0.__NEQUALS__(value1);
        }

        /// <summary>
        /// Process bigger condition and returns result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process_BIGGER(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.__OPERATORS__.GetValue("BIGGER"));
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            CHKVAL(value0,value1);
            return value0.__BIGGER__(value1);
        }

        /// <summary>
        /// Process lower condition and returns result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process_LOWER(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.__OPERATORS__.GetValue("LOWER"));
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            CHKVAL(value0,value1);
            return value0.__LOWER__(value1);
        }

        /// <summary>
        /// Process biggereq condition and returns result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process_BIGGEREQ(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.__OPERATORS__.GetValue("BIGGEREQ"));
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            CHKVAL(value0,value1);
            return value0.__BIGGEREQ__(value1);
        }

        /// <summary>
        /// Process lowereq condition and returns result.
        /// </summary>
        /// <param name="command">Condition.</param>
        /// <param name="table">Table.</param>
        /// <param name="row">Row.</param>
        /// <param name="from">Use state FROM keyword.</param>
        public static bool Process_LOWEREQ(string command,MochaTableResult table,MochaRow row,bool from) {
            var parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.__OPERATORS__.GetValue("LOWEREQ"));
            var value0 = GetValue(parts[0],table,row,from);
            var value1 = GetValue(parts[1],table,row,from);
            CHKVAL(value0,value1);
            return value0.__LOWEREQ__(value1);
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
