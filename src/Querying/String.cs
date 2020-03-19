namespace MochaDB.Querying {
    /// <summary>
    /// Strings.
    /// </summary>
    public static class QueryingString {
        /// <summary>
        /// Returns first char.
        /// </summary>
        /// <param name="value">String.</param>
        public static char FirstChar(this string value) =>
            value.Length == 0 ? '\0' : value[0];

        /// <summary>
        /// Returns last char.
        /// </summary>
        /// <param name="value">String.</param>
        public static char LastChar(this string value) =>
            value.Length == 0 ? '\0' : value[value.Length-1];
    }
}
