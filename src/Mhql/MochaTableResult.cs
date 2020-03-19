namespace MochaDB.Mhql {
    /// <summary>
    /// Table result for Mhql query results.
    /// </summary>
    public class MochaTableResult {
        #region Constructors

        /// <summary>
        /// Create a new MochaTableResult.
        /// </summary>
        internal MochaTableResult() {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Columns.
        /// </summary>
        public MochaArray<MochaColumn> Columns { get; internal set; }

        /// <summary>
        /// Rows.
        /// </summary>
        public MochaArray<MochaRow> Rows { get; internal set; }

        #endregion
    }
}
