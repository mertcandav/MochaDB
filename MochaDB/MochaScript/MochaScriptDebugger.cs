using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MochaDB.MochaScript.Keywords;
using System.Linq;

namespace MochaDB.MochaScript {
    /// <summary>
    /// Process, debug and run MochaScript codes.
    /// </summary>
    public sealed class MochaScriptDebugger:IDisposable {
        #region Fields

        //Regexes.
        internal Regex compilerEventsRegex = new Regex(@"\bOnStartDebug\b|\bOnSuccessFinishDebug\b|\bOnFunctionInvoking\b|\bOnFunctionInvoked\b|\bOnEcho\b");
        internal Regex bannedSyntaxesRegex = new Regex(@"\bString\b|\bString\[*\]\b|\bBoolean\b|\bBoolean\[*\]\b|\bInteger\b|\bInteger\[*\]\b|\bDouble\b|\bouble\[*\]\b|\bnil\b|
\bTrue\b|\bFloat\b|\bFloat\[*\]\b|\bif\b|\belse\b|\bFalse\b|\bShort\b|\bShort\[*\]\b|\bLong\b|\bLong\[*\]\b|\bDecimal\b|\bDecimal\[*\]\b|\bMochaData\b|\bMochaData\[*\]\b|
\bMochaTable\b|\bMochaTable\[*\]\b|\bMochaSector\b|\bMochaSector\[*\]\b|\bMochaRow\b|\bMochaRow\[*\]\b|\bChar\b|\bChar\[*\]\b|\bMochaQuery\b|\bMochaQuery\[*\]\b|
\bMochaDatabase\b|\bMochaDatabase\[*\]\b|\bMochaColumn\b|\bMochaColumn\[*\]\b|\bMochaScriptDebugger\b|\bMochaScriptDebugger\[*\]\b|\bMochaDataType\b|
\bMochaDataType\[*\]\b|\bMochaScriptCompareMark\b|\bMochaScriptCompareMark\[*\]\b|\bdelete\b");
        internal Regex numberRegex = new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");
        internal Regex variableTypesRegex = new Regex(@"\bString\b|\bChar\b|\bLong\b|\bInteger\b|\bShort\b|
\bDecimal\b|\bDouble\b|\bFloat\b|\bBoolean\b");

        //-----

        private MochaDatabase db;
        private MochaScriptFunctionCollection functions;
        private MochaScriptFunctionCollection compilerEvents;
        private MochaScriptVariableCollection variables;
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

        /// <summary>
        /// Throw exception.
        /// </summary>
        /// <param name="line">Line.</param>
        /// <param name="message">Error message.</param>
        private Exception Throw(int line,string message) {
            if(db!=null)
                db.Dispose();

            return new Exception(line + message);
        }

        /// <summary>
        /// Debug code and run.
        /// </summary>
        public void DebugRun() {
            OnStartDebug(new EventArgs());

            //Use MochaDatabase object if success provider.
            db = null;

            int dex=0;
            //Find Provider and Debugger.
            for(int Index = 0; Index < MochaScriptArray.Length; Index++) {
                try {
                    string line = MochaScriptArray[Index].Trim();

                    if(line.StartsWith("Provider ")) {
                        string[] Parts = line.Split(' ');
                        if(Parts.Length == 3)
                            db = new MochaDatabase(Parts[1],Parts[2]);
                        else if(Parts.Length == 2)
                            db = new MochaDatabase(Parts[1]);

                        break;
                    }
                } catch { dex = Index; break; }
            }

            //Check Provider.
            if(db == null)
                Throw(dex+1,"|| Provider could not be processed!");

            //Find Begin and Final tag index.
            beginIndex = Keyword_Begin.GetIndex(MochaScriptArray);
            finalIndex = Keyword_Final.GetIndex(MochaScriptArray);

            //Check indexes.
            if(beginIndex == -1)
                Throw(-1,"|| Begin keyword not found!");
            else if(finalIndex == -1)
                Throw(-1,"|| Final keyword not found!");
            else if(beginIndex > finalIndex)
                Throw(beginIndex,"|| Start keyword cannot come after Last keyword!");

            //Functions.
            functions.Clear();
            functions.AddRange(GetFunctions());

            //Check Main function.
            if(functions[0].Name != "Main")
                Throw(functions[0].Index,"|| First function is not Main function.");
            else if(!functions.Contains("Main"))
                Throw(-1,"|| Not defined Main function.");

            //Compiler Events.
            compilerEvents.Clear();
            compilerEvents.AddRange(GetCompilerEvents());

            //Variables.
            variables.Clear();

            //Process Commands.
            functions.Invoke("Main");

            OnSuccessFinishDebug(new EventArgs());
        }

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
                        OnEcho(new MochaScriptEchoEventArgs(GetArgumentValue("Undefined",line[5..^0],index)));
                        continue;
                    } else if(line.StartsWith("delete ")) {
                        string[] parts = line.Split(' ');
                        if(parts.Length == 2) {
                            variables.Remove(parts[1]);
                            continue;
                        } else {
                            Throw(index + 1,"|| The entry was not in the correct format!");
                        }
                    } else if(line[^2] == '(' && line[^1] == ')') {
                        functions.Invoke(line[0..^2]);
                        continue;
                    } else if(TryVariable(index)) {
                        continue;
                    }

                    try {
                        db.Query.Run(line);
                    } catch {
                        try {
                            db.Query.GetRun(line);
                        } catch(Exception Excep) {
                            Throw(index + 1,"|| " + Excep.Message);
                        }
                    }
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

                if(line.StartsWith("if ")) {
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

                    closeIndex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');

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

                    closeIndex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');

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
                    closeIndex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');

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
                    db.Query.GetRun(parts.ElementAt(1))));
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
                    throw Throw(dex + 1, "|| ComparisonMark failed, no such ComparisonMark!");
                } else if(mark == MochaScriptComparisonMark.Equal) {
                    return arg1.Equals(arg2);
                } else if(mark == MochaScriptComparisonMark.NotEqual) {
                    return !arg1.Equals(arg2);
                } else if(mark == MochaScriptComparisonMark.EqualBigger) {
                    return ((int)arg1) >= ((int)arg2);
                } else if(mark == MochaScriptComparisonMark.EqualSmaller) {
                    return ((int)arg1) <= ((int)arg2);
                } else if(mark == MochaScriptComparisonMark.Bigger) {
                    return ((int)arg1) > ((int)arg2);
                } else {
                    return ((int)arg1) < ((int)arg2);
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
                if(arg[0] == '"' && arg[^1] == '"') {
                    return arg[1..^1];
                } else if(arg[0] == '\'' && arg[^1] == '\'' && arg[1..^1].Length == 1) {
                    return char.Parse(arg[1..^1]);
                } else if(arg == bool.TrueString || arg == bool.FalseString) {
                    return bool.Parse(arg);
                } else if(numberRegex.IsMatch(arg)) {
                    double DoubleOut;
                    float FloatOut;
                    decimal DecimalOut;
                    long LongOut;
                    int IntOut;
                    short ShortOut;
                    if(short.TryParse(arg,out ShortOut))
                        return ShortOut;
                    else if(int.TryParse(arg,out IntOut))
                        return IntOut;
                    else if(long.TryParse(arg,out LongOut))
                        return LongOut;
                    else if(decimal.TryParse(arg,out DecimalOut))
                        return DecimalOut;
                    else if(float.TryParse(arg,out FloatOut))
                        return FloatOut;
                    else if(double.TryParse(arg,out DoubleOut))
                        return DoubleOut;
                    else
                        throw Throw(index + 1,"|| Error in value conversion!");
                }  else {
                    int dex = variables.IndexOf(arg);
                    if(dex != -1)
                        return variables[dex].Value;

                    //Check Query.
                    try {
                        return db.Query.GetRun(arg);
                    } catch { }

                    throw Throw(index + 1,"|| Error in value conversion!");
                }
            } else {
                if(type == "String") {
                    if(arg[0] == '"' && arg[^1] == '"') {
                        return arg[1..^1];
                    }
                    return string.Empty;
                } else if(type == "Char") {
                    if(arg[0] == '\'' && arg[^1] == '\'' && arg[1..^1].Length == 1) {
                        return char.Parse(arg[1..^1]);
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
            string
                line = string.Empty,
                name = string.Empty;
            string[] parts = null;
            int dex;
            MochaScriptFunction func;

            for(int index = beginIndex + 1; index < finalIndex; index++) {
                line = MochaScriptArray[index].Trim();

                if(line.StartsWith("func ")) {
                    parts = line.Split(' ');
                    name = parts[1][0..^2];

                    if(parts.Length != 2 || MochaScriptArray[index + 1].Trim() != "{")
                        throw Throw(index + 1,"|| Any function is not processed!");
                    else if(parts[1][^2] != '(' || parts[1][^1] != ')')
                        throw Throw(index + 1,"|| Any function is not processed!");

                    if(functions.Contains(name))
                        throw Throw(index + 1,"|| Not added function. Debugger already in defined this name.");

                    dex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');
                    if(dex == -1)
                        throw Throw(index + 1,"|| Any function is not processed!");

                    func = new MochaScriptFunction(name);
                    func.Source = MochaScriptArray[(index + 2)..dex];
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
            string
                line = string.Empty,
                name = string.Empty;
            string[] parts = null;
            int dex;
            MochaScriptFunction _event;

            for(int index = beginIndex + 1; index < finalIndex; index++) {
                line = MochaScriptArray[index].Trim();

                if(line.StartsWith("compilerevent ")) {

                    parts = line.Split(' ');
                    name = parts[1][0..^2];

                    if(parts.Length != 2 || MochaScriptArray[index + 1].Trim() != "{")
                        throw Throw(index + 1,"|| Any function is not processed!");
                    else if(parts[1][^2] != '(' || parts[1][^1] != ')')
                        throw Throw(index + 1,"|| Any function is not processed!");

                    if(!compilerEventsRegex.IsMatch(name))
                        throw Throw(index + 1,"|| Not added compiler event. Not exists compiler event this name.");

                    if(compilerEvents.Contains(name))
                        throw Throw(index + 1,"|| Not added compiler event. Debugger already in defined this name.");

                    dex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');
                    if(dex == -1)
                        throw Throw(index + 1,"|| Any function is not processed!");

                    _event = new MochaScriptFunction(name);
                    _event.Source = MochaScriptArray[(index + 2)..dex];
                    _event.Index = index;
                    _compilerEvents.Add(_event);

                    index = dex;
                    continue;
                }
            }

            return _compilerEvents;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
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
                    throw new Exception("ScriptPath is can not null!");

                if(value == scriptPath)
                    return;

                FileInfo fInfo = new FileInfo(value);

                if(!fInfo.Exists)
                    throw new Exception("There is no such MochaScript file!");
                if(fInfo.Extension != ".mochascript")
                    throw new Exception("The file shown is not a MochaScript file!");

                if(scriptStream!=null)
                    scriptStream.Dispose();

                MochaScript = File.ReadAllText(value);
                MochaScriptArray = File.ReadAllLines(value);
                
                scriptStream=File.OpenRead(value);
                
                scriptPath = value;
            }
        }

        #endregion
    }
}
