using MochaDB.Collections;
using System;

namespace MochaDB {
    /// <summary>
    /// This is stack object for MochaDB.
    /// </summary>
    [Serializable]
    public class MochaStack:IMochaStack {
        #region Constructors

        /// <summary>
        /// Create new MochaStack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        public MochaStack(string name) {
            Items= new MochaStackItemCollection();
            Name=name;
            Description=string.Empty;
        }

        /// <summary>
        /// Create new MochaStack.
        /// </summary>
        /// <param name="name">Name of stack.</param>
        /// <param name="description">Description of stack.</param>
        public MochaStack(string name,string description) :
            this(name) {
            Description=description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of stack.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of stack.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Items of stack.
        /// </summary>
        public MochaStackItemCollection Items { get; }

        #endregion
    }
}