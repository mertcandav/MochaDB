namespace MochaDB.Querying {
    /// <summary>
    /// Base for MochaDB value results.
    /// </summary>
    /// <typeparam name="T">Type of result value.</typeparam>
    public struct MochaResult<T>:IMochaResult<T> {
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

        #region Methods

        /// <summary>
        /// Returns value as object.
        /// </summary>
        public object GetObject() =>
            Value;

        /// <summary>
        /// Return 
        /// </summary>
        public string GetString() =>
            Value.ToString();

        /// <summary>
        /// Return hash code of value.
        /// </summary>
        public int GetHash() =>
            Value.GetHashCode();

        #endregion

        #region Override

        /// <summary>
        /// Returns result of <see cref="GetString()"/>.
        /// </summary>
        public override string ToString() =>
            GetString();

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
