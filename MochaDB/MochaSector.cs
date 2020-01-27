namespace MochaDB {
    /// <summary>
    /// This is sector object for MochaDB.
    /// </summary>
    public class MochaSector {
        #region Constructors

        /// <summary>
        /// Create new MochaSector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public MochaSector(string name) {
            Name = name;
            Description = string.Empty;
            Data = string.Empty;
        }

        /// <summary>
        /// Create new MochaSector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data value.</param>
        public MochaSector(string name,string data)
            : this(name) {
            Data = data;
        }

        /// <summary>
        /// Create new MochaSector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data value.</param>
        /// <param name="description">Description of sector.</param>
        public MochaSector(string name,string data,string description)
            : this(name,data) {
            Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
