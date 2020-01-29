using System;
using System.Collections.Generic;

namespace MochaDB.MochaScript.Keywords {
    /// <summary>
    /// Collectioner for MochaScript Functions.
    /// </summary>
    [Serializable]
    public sealed class MochaScriptFunctionCollection {
        #region Fields

        private List<MochaScriptFunction> functions;

        #endregion

        #region Constructors

        /// <summary> 
        /// Create new MochaScriptFunctionCollection.
        /// </summary>
        /// <param name="debugger">Based debugger.</param>
        public MochaScriptFunctionCollection(MochaScriptDebugger debugger) {
            Debugger = debugger;
            functions = new List<MochaScriptFunction>();
        }

        /// <summary> 
        /// Create new MochaScriptFunctionCollection.
        /// </summary>
        /// <param name="functions">To be functions.</param>
        public MochaScriptFunctionCollection(MochaScriptDebugger debugger,IEnumerable<MochaScriptFunction> functions)
            : this(debugger) {
            this.functions.AddRange(functions);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Remove all functions.
        /// </summary>
        public void Clear() =>
            functions.Clear();

        /// <summary>
        /// Add function.
        /// </summary>
        /// <param name="Function">To be added function.</param>
        public void Add(MochaScriptFunction Function) =>
            functions.Add(Function);

        /// <summary>
        /// Add functions from collection.
        /// </summary>
        /// <param name="functions">To be added functions.</param>
        public void AddRange(IEnumerable<MochaScriptFunction> functions) =>
            this.functions.AddRange(functions);

        /// <summary>
        /// Get index from name. Return index if defined name but return -1 if not defined name.
        /// </summary>
        /// <param name="name">Name of function.</param>
        public int IndexOf(string name) {
            for(int index = 0; index < Count; index++) {
                if(this[index].Name == name) {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Exists function.
        /// </summary>
        /// <param name="name">Name of function.</param>
        public bool Contains(string name) {
            for(int index = 0; index < Count; index++) {
                if(this[index].Name == name) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Invoke function from name.
        /// </summary>
        /// <param name="name">Name of function.</param>
        public void Invoke(string name) {
            int dex = IndexOf(name);

            if(dex == -1)
                throw new Exception("No function defined this name.");

            if(!Debugger.compilerEventsRegex.IsMatch(name))
                Debugger.OnFunctionInvoking(new EventArgs());

            Debugger.ProcessRange(Functions[dex].Index + 2,Functions[dex].Index + 2 + Functions[dex].Source.Length);

            if(!Debugger.compilerEventsRegex.IsMatch(name))
                Debugger.OnFunctionInvoked(new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Based debugger.
        /// </summary>
        public MochaScriptDebugger Debugger { get; private set; }

        /// <summary>
        /// Get function in index.
        /// </summary>
        /// <param name="index">Index.</param>
        public MochaScriptFunction this[int index] =>
            functions[index];

        /// <summary>
        /// All functions as array.
        /// </summary>
        public IList<MochaScriptFunction> Functions =>
            functions;

        /// <summary>
        /// Get function count.
        /// </summary>
        public int Count =>
            functions.Count;

        #endregion
    }

    /// <summary>
    /// This is the function delegate object for MochaScript.
    /// </summary>
    [Serializable]
    public struct MochaScriptFunction {
        #region Constructors

        /// <summary>
        /// Create new MochaScriptFunction.
        /// </summary>
        /// <param name="name">Name of function.</param>
        public MochaScriptFunction(string name) {
            Name = name;
            Source = null;
            Index = 0;
        }

        /// <summary>
        /// Create new MOchaScriptFunction.
        /// </summary>
        /// <param name="name">Name of function.</param>
        /// <param name="content">MochaScript codes in fuction as lines.</param>
        public MochaScriptFunction(string name,string[] content)
            : this(name) {
            Source = content;
        }

        /// <summary>
        /// Create new MochaScriptFunction.
        /// </summary>
        /// <param name="name">Name of function.</param>
        /// <param name="content">MochaScript codes in fuction as lines.</param>
        /// <param name="index">This function index in code.</param>
        public MochaScriptFunction(string name,string[] content,int index)
            : this(name,content) {
            Index = index;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of function.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Function MochaScript codes as lines.
        /// </summary>
        public string[] Source { get; set; }

        /// <summary>
        /// Function index in code.
        /// </summary>
        public int Index { get; set; }

        #endregion
    }
}
