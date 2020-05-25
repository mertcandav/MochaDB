using System.Collections.Generic;

namespace MochaDB.MochaScript.keywords {
    /// <summary>
    /// Collectioner for MochaScript variables.
    /// </summary>
    internal sealed class MochaScriptVariableCollection {
        #region Fields

        private List<MochaScriptVariable> variables;

        #endregion

        #region Constructors

        /// <summary> 
        /// Create new MochaScriptVariableCollection.
        /// </summary>
        /// <param name="debugger">Based debugger.</param>
        public MochaScriptVariableCollection() {
            variables = new List<MochaScriptVariable>();
        }

        /// <summary> 
        /// Create new MochaScriptVariableCollection.
        /// </summary>
        /// <param name="variables">To be variables.</param>
        public MochaScriptVariableCollection(IEnumerable<MochaScriptVariable> variables)
            : this() {
            this.variables.AddRange(variables);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Remove all variables.
        /// </summary>
        public void Clear() =>
            variables.Clear();

        /// <summary>
        /// Add variable.
        /// </summary>
        /// <param name="variable">To be added variable.</param>
        public void Add(MochaScriptVariable variable) =>
            variables.Add(variable);

        /// <summary>
        /// Add varaible from collection.
        /// </summary>
        /// <param name="variables">To be added variables.</param>
        public void AddRange(IEnumerable<MochaScriptVariable> variables) =>
            this.variables.AddRange(variables);

        /// <summary>
        /// Remove variable by name.
        /// </summary>
        /// <param name="name">Name of variable to remove.</param>
        public void Remove(string name) {
            variables.RemoveAt(IndexOf(name));
        }

        /// <summary>
        /// Remove variable by index.
        /// </summary>
        /// <param name="name">Index of variable to remove.</param>
        public void Remove(int index) {
            variables.RemoveAt(index);
        }

        /// <summary>
        /// Return index from name. Return index if defined name but return -1 if not defined name.
        /// </summary>
        /// <param name="name">Name of variable.</param>
        public int IndexOf(string name) {
            for(int index = 0; index < Count; index++) {
                if(this[index].Name == name) {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Exists variable.
        /// </summary>
        /// <param name="name">Name of variable.</param>
        public bool Contains(string name) {
            return IndexOf(name) == -1 ? false : true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get variable in index.
        /// </summary>
        /// <param name="index">Index.</param>
        public MochaScriptVariable this[int index] =>
            variables[index];

        /// <summary>
        /// All functions as array.
        /// </summary>
        public IList<MochaScriptVariable> Variables =>
            variables.ToArray();

        /// <summary>
        /// Get function count.
        /// </summary>
        public int Count =>
            variables.Count;

        #endregion
    }

    /// <summary>
    /// This is the variable delegate object for MochaScript.
    /// </summary>
    internal struct MochaScriptVariable {
        #region Constructors

        /// <summary>
        /// Create new MochaScriptVariable.
        /// </summary>
        /// <param name="name">Name of variable.</param>
        /// <param name="type">Value type of variable.</param>
        public MochaScriptVariable(string name,string type) {
            Name = name;
            ValueType = type;
            Value = null;
        }

        /// <summary>
        /// Create new MochaScriptFunction.
        /// </summary>
        /// <param name="name">Name of variable.</param>
        /// <param name="type">Value type of variable.</param>
        /// <param name="value">Value of varaible.</param>
        public MochaScriptVariable(string name,string type,object value)
            : this(name,type) {
            Value=value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of variable.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value type of variable.
        /// </summary>
        public string ValueType { get; private set; }

        /// <summary>
        /// Value of variable.
        /// </summary>
        public object Value { get; set; }

        #endregion
    }
}
