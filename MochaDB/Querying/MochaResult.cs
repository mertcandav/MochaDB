namespace MochaDB.Querying {
    /// <summary>
    /// Base for MochaDB value results.
    /// </summary>
    /// <typeparam name="T">Type of result value.</typeparam>
    public struct MochaResult<T>:IMochaResult {
        #region Constructors

        /// <summary>
        /// Create new MochaResult.
        /// </summary>
        /// <param name="value">Result value.</param>
        public MochaResult(T value) {
            Value=value;
        }

        #endregion

        #region Implicit & Explicit

        public static implicit operator MochaResult<T>(T value) =>
            new MochaResult<T>(value);

        public static implicit operator T(MochaResult<T> value) =>
            value.Value;

        #endregion

        #region Override

        public override string ToString() {
            return Value.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Result value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// This is collection result.
        /// </summary>
        public bool IsCollectionResult =>
            false;

        #endregion
    }
}