using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MochaDB.MochaScript.Keywords;

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

        //-----

        private MochaDatabase db;
        private MochaScriptFunctionCollection functions;
        private MochaScriptFunctionCollection compilerEvents;
        private Dictionary<string,object> variables;
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

            //Load.
            db = null;
            functions = new MochaScriptFunctionCollection(this);
            compilerEvents = new MochaScriptFunctionCollection(this);
            variables = new Dictionary<string,object>();
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
        /// Debug code and run.
        /// </summary>
        public void DebugRun() {
            OnStartDebug(new EventArgs());

            //Use MochaDatabase object if success provider.
            db = null;

            //Find Provider and Debugger.
            for(int Index = 0; Index < MochaScriptArray.Length; Index++) {
                try {
                    string line = MochaScriptArray[Index].Trim();

                    if(line[0..8] == "Provider") {
                        string[] Parts = line.Split(' ');
                        if(Parts.Length == 3)
                            db = new MochaDatabase(Parts[1],Parts[2]);
                        else if(Parts.Length == 2)
                            db = new MochaDatabase(Parts[1]);

                        break;
                    }
                } catch { }
            }

            //Check Provider.
            if(db == null || !db.IsConnected)
                throw new Exception("Provider could not be processed!");

            //Find Begin and Final tag index.
            beginIndex = Keyword_Begin.GetIndex(MochaScriptArray);
            finalIndex = Keyword_Final.GetIndex(MochaScriptArray);

            //Check indexes.
            if(beginIndex == -1)
                throw new Exception("Begin keyword not found!");
            else if(finalIndex == -1)
                throw new Exception("Final keyword not found!");
            else if(beginIndex > finalIndex)
                throw new Exception("Start keyword cannot come after Last keyword!");

            //Functions.
            functions.Clear();
            functions.AddRange(GetFunctions());

            //Check Main function.
            if(functions[0].Name != "Main")
                throw new Exception("First function is not Main function.");
            else if(!functions.Contains("Main"))
                throw new Exception("Not defined Main function.");

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
                        OnEcho(new MochaScriptEchoEventArgs(GetArgumentValue("Undefined",line[5..^0])));
                        continue;
                    } else if(line.StartsWith("delete ")) {
                        string[] parts = line.Split(' ');
                        if(parts.Length == 2) {
                            variables.Remove(parts[1]);
                            continue;
                        } else {
                            throw new Exception("Line: " + (index + 1) + "\n----\nThe entry was not in the correct format!!");
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
                            throw new Exception("Line: " + (index + 1) + "\n----\n" + Excep.Message+"\n" + line);
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
                        throw new Exception("Line: " + (index + 1) + "\n----\nThe if comparison was wrong! There are too many or missing parameters!");

                    //Get compare mark.
                    MochaScriptComparisonMark elifMark = GetCompareMark(elifArg[2]);

                    if(elifMark == MochaScriptComparisonMark.Undefined)
                        throw new Exception("The comparison parameter is not recognized!");

                    //Get Value Arguments.
                    object ElifArg1 = GetArgumentValue("Undefined",elifArg[1]);
                    object ElifArg2 = GetArgumentValue("Undefined",elifArg[3]);

                    //Check arguments.
                    if(ElifArg1 == null || ElifArg2 == null)
                        throw new Exception("Line: " + (index + 1) + "\n----\nOne of his arguments could not be processed!");

                    closeIndex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');

                    if(!ok && CompareArguments(elifMark,ElifArg1,ElifArg2)) {
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
                        throw new Exception("Line: " + (index + 1) + "\n----\nThe if comparison was wrong! There are too many or missing parameters!");

                    //Get compare mark.
                    MochaScriptComparisonMark ElifMark = GetCompareMark(ElifArg[2]);

                    if(ElifMark == MochaScriptComparisonMark.Undefined)
                        throw new Exception("The comparison parameter is not recognized!");

                    //Get Value Arguments.
                    object ElifArg1 = GetArgumentValue("Undefined",ElifArg[1]);
                    object ElifArg2 = GetArgumentValue("Undefined",ElifArg[3]);

                    //Check arguments.
                    if(ElifArg1 == null || ElifArg2 == null)
                        throw new Exception("Line: " + (index + 1) + "\n----\nOne of his arguments could not be processed!");

                    closeIndex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');

                    if(!ok && CompareArguments(ElifMark,ElifArg1,ElifArg2)) {
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
                        ok = true;
                        ProcessRange(index + 2,closeIndex);
                    }

                    return closeIndex;
                } else {
                    return index - 1;
                }
            }

            throw new Exception("Line: " + (startIndex + 1) + "\n----\nCould not process if-elif-else structures.");
        }

        /// <summary>
        /// Try define variable.
        /// </summary>
        /// <param name="index">Index of line.</param>
        /// <returns>True if variable is defined successfully but false if not.</returns>
        internal bool TryVariable(int index) {
            string line = MochaScriptArray[index].Trim();
            string[] parts = line.Split(' ');

            if(parts.Length == 3) {
                if(variables.ContainsKey(parts[0])) {
                    variables.Remove(parts[0]);
                    variables.Add(parts[0],GetArgumentValue("Undefined",parts[2]));
                    return true;
                }
                return false;
            }

            if(parts.Length != 4)
                return false;

            if(IsBannedSyntax(parts[1]))
                throw new Exception("Line: " + (index + 1) + "\n----\nThis variable name cannot be used!");

            if(parts[3] == "nil") {
                variables.Add(parts[1],null);
                return true;
            }

            if(variables.ContainsKey(parts[3])) {
                variables.Add(parts[1],GetArgumentValue("Variable",parts[3]));
                return true;
            }

            //Check Query.
            try {
                variables.Add(parts[1],db.Query.GetRun(parts[3]));
                return true;
            } catch { }

            if(line.StartsWith("String ")) {
                variables.Add(parts[1],GetArgumentValue("String",parts[3]));
                return true;
            } else if(line.StartsWith("Char ")) {
                variables.Add(parts[1],GetArgumentValue("Char",parts[3]));
                return true;
            } else if(line.StartsWith("Decimal ")) {
                variables.Add(parts[1],GetArgumentValue("Decimal",parts[3]));
                return true;
            } else if(line.StartsWith("Long ")) {
                variables.Add(parts[1],GetArgumentValue("Long",parts[3]));
                return true;
            } else if(line.StartsWith("Integer ")) {
                variables.Add(parts[1],GetArgumentValue("Integer",parts[3]));
                return true;
            } else if(line.StartsWith("Short ")) {
                variables.Add(parts[1],GetArgumentValue("Short",parts[3]));
                return true;
            } else if(line.StartsWith("Double ")) {
                variables.Add(parts[1],GetArgumentValue("Double",parts[3]));
                return true;
            } else if(line.StartsWith("Float ")) {
                variables.Add(parts[1],GetArgumentValue("Float",parts[3]));
                return true;
            } else if(line.StartsWith("Boolean ")) {
                variables.Add(parts[1],GetArgumentValue("Boolean",parts[3]));
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
        internal bool CompareArguments(MochaScriptComparisonMark mark,object arg1,object arg2) {
            try {
                if(mark == MochaScriptComparisonMark.Undefined) {
                    throw new Exception("ComparisonMark failed, no such ComparisonMark!");
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
        internal object GetArgumentValue(string type,string arg) {
            if(arg == "nil")
                return null;

            //Check variable.
            if(type == "Variable") {
                object Out;
                if(variables.TryGetValue(arg,out Out)) {
                    return Out;
                }
                return null;
            }

            //Check Query.
            try {
                return db.Query.GetRun(arg);
            } catch { }

            if(type == "Undefined") {
                object Out;

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
                        throw new Exception("Error in value conversion!");
                } else if(variables.TryGetValue(arg,out Out)) {
                    return Out;
                } else {
                    throw new Exception("Error in value conversion!");
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
                    throw new Exception("Error in value conversion!");
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
                        throw new Exception("Any function is not processed!");
                    else if(parts[1][^2] != '(' || parts[1][^1] != ')')
                        throw new Exception("Any function is not processed!");

                    if(functions.Contains(name))
                        throw new Exception("Not added function. Debugger already in defined this name.");

                    dex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');
                    if(dex == -1)
                        throw new Exception("Any function is not processed!");

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

                if(line.StartsWith("compilerevents ")) {

                    parts = line.Split(' ');
                    name = parts[1][0..^2];

                    if(parts.Length != 2 || MochaScriptArray[index + 1].Trim() != "{")
                        throw new Exception("Any function is not processed!");
                    else if(parts[1][^2] != '(' || parts[1][^1] != ')')
                        throw new Exception("Any function is not processed!");

                    if(!compilerEventsRegex.IsMatch(name))
                        throw new Exception("Not added compiler event. Not exists compiler event this name.");

                    if(compilerEvents.Contains(name))
                        throw new Exception("Not added compiler event. Debugger already in defined this name.");

                    dex = MochaScriptCodeProcessor.GetCloseBracketIndex(MochaScriptArray,index + 2,'{','}');
                    if(dex == -1)
                        throw new Exception("Any function is not processed!");

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
