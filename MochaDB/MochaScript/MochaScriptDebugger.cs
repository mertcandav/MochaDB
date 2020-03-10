using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MochaDB.MochaScript.Keywords;
using MochaDB.Querying;

namespace MochaDB.MochaScript {
    /// <summary>
    /// Process, debug and run MochaScript codes.
    /// </summary>
    public class MochaScriptDebugger:IMochaScriptDebugger {
        #region Fields

        //Regexes.
        internal Regex compilerEventsRegex = new Regex(
@"\b(OnStartDebug|OnSuccessFinishDebug|OnFunctionInvoking|OnFunctionInvoked|OnEcho)\b");

        internal Regex bannedSyntaxesRegex = new Regex(
@"\b(String|String\[*\]|Boolean|Boolean\[*\]|Integer|Integer\[*\]|Double|ouble\[*\]|nil|
True|Float|Float\[*\]|if|else|False|Short|Short\[*\]|Long|Long\[*\]|Decimal|Decimal\[*\]|MochaData|MochaData\[*\]|
MochaTable|MochaTable\[*\]|MochaSector|MochaSector\[*\]|MochaRow|MochaRow\[*\]|Char|Char\[*\]|MochaQuery|MochaQuery\[*\]|
MochaDatabase|MochaDatabase\[*\]|MochaColumn|MochaColumn\[*\]|MochaScriptDebugger|MochaScriptDebugger\[*\]|MochaDataType|
MochaDataType\[*\]|MochaScriptCompareMark|MochaScriptCompareMark\[*\]|delete|SByte|SByte\[*\]|UShort|UShort\[*\]|ULong|ULong\[*\]|
UInteger|UInteger\[*\]|DateTime|DateTime\[*\])\b");

        internal Regex numberRegex = new Regex(
@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");

        internal Regex variableTypesRegex = new Regex(
@"\b(String|Char|Long|Integer|Short|Decimal|Double|Float|Boolean|Byte|SByte|UInteger|ULong|UShort|DateTime)\b");

        //-----

        private MochaDatabase db;
        private MochaScriptFunctionCollection functions;
        private MochaScriptFunctionCollection compilerEvents;
        private MochaScriptVariableCollection variables;
        private MochaScriptCodeProcessor codeProcessor;
        private int beginIndex;
        private int finalIndex;

        private FileStream scriptStream;
        private string scriptPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaScriptDebugger.
        /// </summary>
        /// <param name="path">Path of MochaScript file.</param>
        public MochaScriptDebugger(string path) {
            codeProcessor=new MochaScriptCodeProcessor();
            ScriptPath = path;

            db = null;
            functions = new MochaScriptFunctionCollection(this);
            compilerEvents = new MochaScriptFunctionCollection(this);
            variables = new MochaScriptVariableCollection();
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> StartDebug;
        internal void OnStartDebug(EventArgs e) {
            //Invoke.
            StartDebug?.Invoke(this,e);

            if(compilerEvents.Contains("OnStartDebug")) {
                compilerEvents.Invoke("OnStartDebug");
            }
        }

        public event EventHandler<EventArgs> SuccessFinishDebug;
        internal void OnSuccessFinishDebug(EventArgs e) {
            //Invoke.
            SuccessFinishDebug?.Invoke(this,e);

            if(compilerEvents.Contains("OnSuccessFinishDebug")) {
                compilerEvents.Invoke("OnSuccessFinishDebug");
            }
        }

        public event EventHandler<MochaScriptEchoEventArgs> Echo;
        internal void OnEcho(MochaScriptEchoEventArgs e) {
            //Invoke.
            Echo?.Invoke(this,e);

            if(compilerEvents.Contains("OnEcho")) {
                compilerEvents.Invoke("OnEcho");
            }
        }

        public event EventHandler<EventArgs> FunctionInvoking;
        internal void OnFunctionInvoking(EventArgs e) {
            //Invoke.
            FunctionInvoking?.Invoke(this,e);

            if(compilerEvents.Contains("OnFunctionInvoking")) {
                compilerEvents.Invoke("OnFunctionInvoking");
            }
        }

        public event EventHandler<EventArgs> FunctionInvoked;
        internal void OnFunctionInvoked(EventArgs e) {
            //Invoke.
            FunctionInvoked?.Invoke(this,e);

            if(compilerEvents.Contains("OnFunctionInvoked")) {
                compilerEvents.Invoke("OnFunctionInvoked");
            }
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// Throw exception.
        /// </summary>
        /// <param name="line">Line.</param>
        /// <param name="message">Error message.</param>
        private Exception Throw(int line,string message) {
            if(db!=null) {
                db.Dispose();
            }

            return new Exception($"{line}{message}");
        }

        #endregion

        #region Internal

        /// <summary>
        /// Process range.
        /// </summary>
        /// <param name="startIndex">Process start index.</param>
        /// <param name="endIndex">Process end index.</param>
        internal void ProcessRange(int startIndex,int endIndex) {
            for(int index = startIndex; index < endIndex; index++) {
                string line = MochaScriptArray[index].Trim();

                if(line.Length >= 2) {
                    if(line.StartsWith("//")) {
                        continue;
                    } else if(line.StartsWith("if ")) {
                        index = Process_if(index);
                        continue;
                    } else if(line.StartsWith("echo ")) {
                        OnEcho(new MochaScriptEchoEventArgs(GetArgumentValue("Undefined",line.Substring(5),index)));
                        continue;
                    } else if(line.StartsWith("delete ")) {
                        string[] parts = line.Split(' ');
                        if(parts.Length == 2) {
                            variables.Remove(parts[1]);
                            continue;
                        } else {
                            throw Throw(index + 1,"|| The entry was not in the correct format!");
                        }
                    } else if(line.EndsWith("()")) {
                        try {
                            functions.Invoke(line.Substring(0,line.Length-2));
                        } catch {
                            throw Throw(index+1,$"|| {MochaScriptArray[index+1]}");
                        }
                        continue;
                    } else if(TryVariable(index)) {
                        continue;
                    }

                    db.Query.MochaQ.Command=line;
                    if(db.Query.MochaQ.IsRunQuery()) {
                        try {
                            db.Query.Run(line);
                            continue;
                        } catch(Exception excep) {
                            throw Throw(index+1,$"|| {excep.Message}");
                        }
                    }

                    throw Throw(index+1,"|| The compiler doesn't know what to do with this code!");
                }
            }
        }

        /// <summary>
        /// Process if.
        /// </summary>
        /// <param name="startIndex">Process start index.</param>
        internal int Process_if(int startIndex) {
            bool ok = false;
            int closeIndex;

            for(int index = startIndex; index < MochaScriptArray.Length; index++) {
                string line = MochaScriptArray[index].Trim();

                if(index == startIndex && line.StartsWith("if ")) {
                    //Check argument.
                    string[] elifArg = MochaScriptArray[index].Trim().Split(' ');

                    if(elifArg.Length != 4)
                        throw Throw(index + 1,"|| The if comparison was wrong! There are too many or missing parameters!");

                    //Get compare mark.
                    MochaScriptComparisonMark elifMark = GetCompareMark(elifArg[2]);

                    if(elifMark == MochaScriptComparisonMark.Undefined)
                        throw Throw(index + 1,"|| The comparison parameter is not recognized!");

                    //Get Value Arguments.
                    object ElifArg1 = GetArgumentValue("Undefined",elifArg[1],index);
                    object ElifArg2 = GetArgumentValue("Undefined",elifArg[3],index);

                    //Check arguments.
                    if(ElifArg1 == null || ElifArg2 == null)
                        throw Throw(index + 1,"|| One of his arguments could not be processed!");

                    closeIndex = codeProcessor.GetCloseBracketIndex(index + 2,'{','}');

                    if(!ok && CompareArguments(elifMark,ElifArg1,ElifArg2,index)) {
                        ok = true;
                        ProcessRange(index + 2,closeIndex);
                        index = closeIndex;
                        continue;
                    } else {
                        index = closeIndex;
                        continue;
                    }
                } else if(line.StartsWith("elif ")) {
                    //Check argument.
                    string[] ElifArg = MochaScriptArray[index].Trim().Split(' ');

                    if(ElifArg.Length != 4)
                        throw Throw(index + 1,"|| The if comparison was wrong! There are too many or missing parameters!");

                    //Get compare mark.
                    MochaScriptComparisonMark ElifMark = GetCompareMark(ElifArg[2]);

                    if(ElifMark == MochaScriptComparisonMark.Undefined)
                        throw Throw(index + 1,"The comparison parameter is not recognized!");

                    //Get Value Arguments.
                    object ElifArg1 = GetArgumentValue("Undefined",ElifArg[1],index);
                    object ElifArg2 = GetArgumentValue("Undefined",ElifArg[3],index);

                    //Check arguments.
                    if(ElifArg1 == null || ElifArg2 == null)
                        throw Throw(index + 1,"|| One of his arguments could not be processed!");

                    closeIndex = codeProcessor.GetCloseBracketIndex(index + 2,'{','}');

                    if(!ok && CompareArguments(ElifMark,ElifArg1,ElifArg2,index)) {
                        ok = true;
                        ProcessRange(index + 2,closeIndex);
                        index = closeIndex;
                        continue;
                    } else {
                        index = closeIndex;
                        continue;
                    }
                } else if(line == "else") {
                    closeIndex = codeProcessor.GetCloseBracketIndex(index + 2,'{','}');

                    if(!ok) {
                        ProcessRange(index + 2,closeIndex);
                    }

                    return closeIndex;
                } else {
                    return index - 1;
                }
            }

            throw Throw(startIndex + 1,"|| Could not process if-elif-else structures.");
        }

        /// <summary>
        /// Try define variable.
        /// </summary>
        /// <param name="index">Index of line.</param>
        /// <returns>True if variable is defined successfully but false if not.</returns>
        internal bool TryVariable(int index) {
            string line = MochaScriptArray[index].TrimStart().TrimEnd();
            IEnumerable<string> parts = line.Split('=');
            IEnumerable<string> varParts = parts.ElementAt(0).Split(' ');

            parts =
                from value in parts
                where !string.IsNullOrWhiteSpace(value)
                select value.TrimStart().TrimEnd();
            varParts =
                from value in varParts
                where !string.IsNullOrWhiteSpace(value)
                select value.TrimStart().TrimEnd();


            if(parts.Count()==1 && varParts.Count() == 2) {
                if(IsBannedSyntax(varParts.ElementAt(1)))
                    throw Throw(index + 1,"|| This variable name cannot be used!");

                if(!variableTypesRegex.IsMatch(varParts.ElementAt(0)))
                    return false;

                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),null));
                return true;
            }

            if(parts.Count() != 2)
                return false;

            if(varParts.Count() == 1) {
                int dex = variables.IndexOf(varParts.ElementAt(0));
                if(dex != -1) {
                    object value = GetArgumentValue(variables[dex].ValueType,parts.ElementAt(1),index);
                    variables.Add(new MochaScriptVariable(variables[dex].Name,
                        variables[dex].ValueType,value));
                    variables.Remove(dex);
                    return true;
                }
                return false;
            }

            if(IsBannedSyntax(varParts.ElementAt(1)))
                throw Throw(index + 1,"|| This variable name cannot be used!");

            if(parts.ElementAt(1) == "nil") {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),null));
                return true;
            }

            if(variables.Contains(varParts.ElementAt(1))) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Variable",parts.ElementAt(1),index)));
                return true;
            }

            //Check Query.
            try {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Undefined",db.Query.GetRun(parts.ElementAt(1)).ToString(),index)));
                return true;
            } catch { }

            if(line.StartsWith("String ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("String",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Char ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Char",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Decimal ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Decimal",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Long ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Long",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Integer ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Integer",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Short ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Short",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Double ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Double",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Float ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Float",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("Boolean ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("Boolean",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("UShort ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("UShort",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("UInteger ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("UInteger",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("ULong ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("ULong",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("SByte ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("SByte",parts.ElementAt(1),index)));
                return true;
            } else if(line.StartsWith("DateTime ")) {
                variables.Add(new MochaScriptVariable(varParts.ElementAt(1),varParts.ElementAt(0),
                    GetArgumentValue("DateTime",parts.ElementAt(1),index)));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get syntax ban state.
        /// </summary>
        /// <param name="syntax">Check syntax.</param>
        /// <returns>true if banned but false if no.</returns>
        internal bool IsBannedSyntax(string syntax) {
            return bannedSyntaxesRegex.IsMatch(syntax);
        }

        /// <summary>
        /// Compare arguments by compare mark.
        /// </summary>
        /// <param name="mark">Compare mark.</param>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        /// <param name="dex">Index of line.</param>
        internal bool CompareArguments(MochaScriptComparisonMark mark,object arg1,object arg2,int dex) {
            try {
                if(mark == MochaScriptComparisonMark.Undefined) {
                    throw Throw(dex + 1,"|| ComparisonMark failed, no such ComparisonMark!");
                }
                if(mark == MochaScriptComparisonMark.Equal) {
                    return arg1.Equals(arg2);
                }
                if(mark == MochaScriptComparisonMark.NotEqual) {
                    return !arg1.Equals(arg2);
                }

                var a1 = new MochaResult<decimal>((decimal)MochaData.GetDataFromString(MochaDataType.Decimal,arg1.ToString()));
                var a2 = new MochaResult<decimal>((decimal)MochaData.GetDataFromString(MochaDataType.Decimal,arg2.ToString()));
                if(mark == MochaScriptComparisonMark.EqualBigger) {
                    return a1 >= a2;
                }
                if(mark == MochaScriptComparisonMark.EqualSmaller) {
                    return a1 <= a2;
                }
                if(mark == MochaScriptComparisonMark.Bigger) {
                    return a1 > a2;
                } else {
                    return a1 < a2;
                }
            } catch { return false; }
        }

        /// <summary>
        /// Get value by argument type.
        /// </summary>
        /// <param name="type">Variable data type.</param>
        /// <param name="arg">Argument.</param>
        /// <param name="index">Index of line.</param>
        internal object GetArgumentValue(string type,string arg,int index) {
            if(arg == "nil")
                return null;

            //Check variable.
            if(type == "Variable") {
                int dex = variables.IndexOf(arg);
                if(dex != -1) {
                    return variables[dex].Value;
                }
                throw Throw(index + 1,"|| There is no such variable!");
            }

            if(type == "Undefined") {
                if(arg.StartsWith("\"") && arg.EndsWith("\"")) {
                    return arg.Substring(1,arg.Length-2);
                } else if(arg.StartsWith("'") && arg.EndsWith("'") && arg.Substring(1,arg.Length-2).Length == 1) {
                    return char.Parse(arg.Substring(1,arg.Length-2));
                } else if(arg == bool.TrueString || arg == bool.FalseString) {
                    return bool.Parse(arg);
                } else if(numberRegex.IsMatch(arg)) {
                    double doubleOut;
                    float floatOut;
                    decimal decimalOut;
                    long longOut;
                    int intOut;
                    short shortOut;
                    ushort uShortOut;
                    uint uIntOut;
                    ulong uLongOut;
                    sbyte sByteOut;
                    DateTime dateTimeOut;
                    if(short.TryParse(arg,out shortOut))
                        return shortOut;
                    if(int.TryParse(arg,out intOut))
                        return intOut;
                    if(long.TryParse(arg,out longOut))
                        return longOut;
                    if(decimal.TryParse(arg,out decimalOut))
                        return decimalOut;
                    if(float.TryParse(arg,out floatOut))
                        return floatOut;
                    if(double.TryParse(arg,out doubleOut))
                        return doubleOut;
                    if(ushort.TryParse(arg,out uShortOut))
                        return uShortOut;
                    if(uint.TryParse(arg,out uIntOut))
                        return uIntOut;
                    if(ulong.TryParse(arg,out uLongOut))
                        return uLongOut;
                    if(sbyte.TryParse(arg,out sByteOut))
                        return sByteOut;
                    if(DateTime.TryParse(arg,out dateTimeOut))
                        return dateTimeOut;
                    else
                        throw Throw(index + 1,"|| Error in value conversion!");
                } else {
                    int dex = variables.IndexOf(arg);
                    if(dex != -1)
                        return variables[dex].Value;

                    //Check Query.
                    try {
                        return db.Query.GetRun(arg).ToString();
                    } catch { }

                    throw Throw(index + 1,"|| Error in value conversion!");
                }
            } else {
                if(type == "String") {
                    if(arg.StartsWith("\"") && arg.EndsWith("\"")) {
                        return arg.Substring(1,arg.Length-2);
                    }
                    return string.Empty;
                } else if(type == "Char") {
                    if(arg.StartsWith("'") && arg.EndsWith("'") && arg.Substring(1,arg.Length-2).Length == 1) {
                        return char.Parse(arg.Substring(1,arg.Length-2));
                    }
                    return new char();
                } else if(type == "Decimal") {
                    if(numberRegex.IsMatch(arg)) {
                        return decimal.Parse(arg);
                    }
                    return 0;
                } else if(arg == "Long") {
                    if(numberRegex.IsMatch(arg)) {
                        return long.Parse(arg);
                    }
                    return 0;
                } else if(type == "Integer") {
                    if(numberRegex.IsMatch(arg)) {
                        return int.Parse(arg);
                    }
                    return 0;
                } else if(type == "Short") {
                    if(numberRegex.IsMatch(arg)) {
                        return short.Parse(arg);
                    }
                    return 0;
                } else if(type == "UShort") {
                    if(numberRegex.IsMatch(arg)) {
                        return ushort.Parse(arg);
                    }
                    return 0;
                } else if(type == "ULong") {
                    if(numberRegex.IsMatch(arg)) {
                        return ulong.Parse(arg);
                    }
                    return 0;
                } else if(type == "UInteger") {
                    if(numberRegex.IsMatch(arg)) {
                        return uint.Parse(arg);
                    }
                    return 0;
                } else if(type == "SByte") {
                    if(numberRegex.IsMatch(arg)) {
                        return sbyte.Parse(arg);
                    }
                    return 0;
                } else if(type == "Double") {
                    if(numberRegex.IsMatch(arg)) {
                        return double.Parse(arg);
                    }
                    return 0;
                } else if(type == "Float") {
                    if(numberRegex.IsMatch(arg)) {
                        return float.Parse(arg);
                    }
                    return 0;
                } else if(type == "Boolean") {
                    if(arg == bool.TrueString || arg == bool.FalseString) {
                        return bool.Parse(arg);
                    }
                    return false;
                } else if(type == "DateTime") {
                    DateTime _out;
                    if(DateTime.TryParse(arg,out _out)) {
                        return _out;
                    }
                    return DateTime.Now;
                } else {
                    throw Throw(index + 1,"|| Error in value conversion!");
                }
            }
        }

        /// <summary>
        /// Find mark task.
        /// </summary>
        /// <param name="mark">Compare mark.</param>
        internal MochaScriptComparisonMark GetCompareMark(string mark) {
            if(mark == ">")
                return MochaScriptComparisonMark.Bigger;
            else if(mark == "<")
                return MochaScriptComparisonMark.Smaller;
            else if(mark == ">=")
                return MochaScriptComparisonMark.EqualBigger;
            else if(mark == "<=")
                return MochaScriptComparisonMark.EqualSmaller;
            else if(mark == "==")
                return MochaScriptComparisonMark.Equal;
            else if(mark == "!=")
                return MochaScriptComparisonMark.NotEqual;
            else
                return MochaScriptComparisonMark.Undefined;
        }

        /// <summary>
        /// Get all functions in code.
        /// </summary>
        internal IList<MochaScriptFunction> GetFunctions() {
            List<MochaScriptFunction> _functions = new List<MochaScriptFunction>();
            string line, name;
            string[] parts;
            int dex;
            MochaScriptFunction func;

            for(int index = beginIndex + 1; index < finalIndex; index++) {
                line = MochaScriptArray[index].Trim();

                if(line.StartsWith("func ")) {
                    parts = line.Split(' ');
                    name = parts[1].Substring(0,parts[1].Length-2);

                    if(parts.Length != 2 || MochaScriptArray[index + 1].Trim() != "{")
                        throw Throw(index + 1,"|| Any function is not processed!");
                    else if(!parts[1].EndsWith("()"))
                        throw Throw(index + 1,"|| Any function is not processed!");

                    if(functions.Contains(name))
                        throw Throw(index + 1,"|| Not added function. Debugger already in defined this name.");

                    dex = codeProcessor.GetCloseBracketIndex(index + 2,'{','}');
                    if(dex == -1)
                        throw Throw(index + 1,"|| Any function is not processed!");

                    func = new MochaScriptFunction(name);
                    func.Source= MochaScriptArray.Skip(index +2).Take(dex - (index + 2)).ToArray();
                    func.Index = index;
                    _functions.Add(func);

                    index = dex;
                    continue;
                }
            }

            return _functions;
        }

        /// <summary>
        /// Get all compiler events in code.
        /// </summary>
        internal IList<MochaScriptFunction> GetCompilerEvents() {
            List<MochaScriptFunction> _compilerEvents = new List<MochaScriptFunction>();
            string line, name;
            string[] parts;
            int dex;
            MochaScriptFunction _event;

            for(int index = beginIndex + 1; index < finalIndex; index++) {
                line = MochaScriptArray[index].Trim();

                if(line.StartsWith("compilerevent ")) {

                    parts = line.Split(' ');
                    name = parts[1].Substring(0,parts[1].Length-2);

                    if(parts.Length != 2 || MochaScriptArray[index + 1].Trim() != "{")
                        throw Throw(index + 1,"|| Any function is not processed!");
                    else if(!parts[1].EndsWith("()"))
                        throw Throw(index + 1,"|| Any function is not processed!");

                    if(!compilerEventsRegex.IsMatch(name))
                        throw Throw(index + 1,"|| Not added compiler event. Not exists compiler event this name.");

                    if(compilerEvents.Contains(name))
                        throw Throw(index + 1,"|| Not added compiler event. Debugger already in defined this name.");

                    dex = codeProcessor.GetCloseBracketIndex(index + 2,'{','}');
                    if(dex == -1)
                        throw Throw(index + 1,"|| Any function is not processed!");

                    _event = new MochaScriptFunction(name);
                    Array.Copy(MochaScriptArray,index +2,_event.Source,0,dex);
                    _event.Source= MochaScriptArray.Skip(index +2).Take(dex - (index + 2)).ToArray();
                    _event.Index = index;
                    _compilerEvents.Add(_event);

                    index = dex;
                    continue;
                }
            }

            return _compilerEvents;
        }

        #endregion

        /// <summary>
        /// Debug code and run.
        /// </summary>
        public void DebugRun() {
            OnStartDebug(new EventArgs());

            int dex = 0;
            //Find Provider and Debugger.
            for(int Index = 0; Index < MochaScriptArray.Length; Index++) {
                try {
                    string line = MochaScriptArray[Index].Trim();

                    if(line.StartsWith("Provider ")) {
                        string[] Parts = line.Split(' ');
                        if(Parts.Length == 3)
                            db = new MochaDatabase($"path={Parts[1]}; password={Parts[2]};");
                        else if(Parts.Length == 2)
                            db = new MochaDatabase($"path={Parts[1]}; password=;");

                        break;
                    }
                } catch { dex = Index; break; }
            }

            //Check Provider.
            if(db == null)
                throw Throw(dex+1,"|| Provider could not be processed!");

            //Find Begin and Final tag index.
            beginIndex = Keyword_Begin.GetIndex(MochaScriptArray);
            finalIndex = Keyword_Final.GetIndex(MochaScriptArray);

            //Check indexes.
            if(beginIndex == -1)
                throw Throw(-1,"|| Begin keyword not found!");
            else if(finalIndex == -1)
                throw Throw(-1,"|| Final keyword not found!");
            else if(beginIndex > finalIndex)
                throw Throw(beginIndex,"|| Start keyword cannot come after Last keyword!");

            //Functions.
            functions.Clear();
            functions.AddRange(GetFunctions());

            //Check Main function.
            if(functions[0].Name != "Main")
                throw Throw(functions[0].Index,"|| First function is not Main function.");
            if(!functions.Contains("Main"))
                throw Throw(-1,"|| Not defined Main function.");

            //Compiler Events.
            compilerEvents.Clear();
            compilerEvents.AddRange(GetCompilerEvents());

            //Variables.
            variables.Clear();
            //Connect to database.
            db.Connect();

            //Process Commands.
            functions.Invoke("Main");

            db.Dispose();
            db=null;

            OnSuccessFinishDebug(new EventArgs());
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            if(scriptStream!=null)
                scriptStream.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// MochaScript codes.
        /// </summary>
        public string MochaScript { get; private set; }

        /// <summary>
        /// MochaScript codes as lines.
        /// </summary>
        public string[] MochaScriptArray { get; private set; }

        /// <summary>
        /// Path of MochaScrip file.
        /// </summary>
        public string ScriptPath {
            get =>
                scriptPath;
            set {
                if(string.IsNullOrEmpty(value))
                    throw new NullReferenceException("ScriptPath is can not null!");

                FileInfo fInfo = new FileInfo(value);

                if(!fInfo.Exists)
                    throw new NullReferenceException("There is no such MochaScript file!");
                if(fInfo.Extension != ".mochascript")
                    throw new NullReferenceException("The file shown is not a MochaScript file!");

                if(scriptStream!=null)
                    scriptStream.Dispose();

                MochaScript = File.ReadAllText(value);
                MochaScriptArray = File.ReadAllLines(value);
                codeProcessor.Source=MochaScriptArray;

                scriptStream=File.OpenRead(value);

                scriptPath = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Comparison mark for MochaScript.
    /// </summary>
    public enum MochaScriptComparisonMark {
        Equal,
        NotEqual,
        Bigger,
        Smaller,
        EqualBigger,
        EqualSmaller,
        Undefined
    }
}