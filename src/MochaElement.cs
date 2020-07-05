namespace MochaDB {
    /// <summary>
    /// Element for MochaDB.
    /// </summary>
    public struct MochaElement {
        #region Constructors

        /// <summary>
        /// Create a new MochaElement.
        /// </summary>
        /// <param name="name">Name of element.</param>
        public MochaElement(string name) {
            Name=name;
            Description=string.Empty;
            Value=string.Empty;
        }

        /// <summary>
        /// Create a new MochaElement.
        /// </summary>
        /// <param name="name">Name of element.</param>
        /// <param name="description">Description of element.</param>
        public MochaElement(string name,string description) {
            Name=name;
            Description=description;
            Value=string.Empty;
        }

        /// <summary>
        /// Create a new MochaElement.
        /// </summary>
        /// <param name="name">Name of element.</param>
        public MochaElement(string name,string description,string value) {
            Name=name;
            Description=description;
            Value=value;
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaElement value) =>
            value.ToString();

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Value"/>.
        /// </summary>
        public override string ToString() {
            return Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of element.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Value of element.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}
