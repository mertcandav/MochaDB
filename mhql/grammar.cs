namespace MochaDB.mhql {
    /// <summary>
    /// Grammar of MHQL.
    /// </summary>
    internal static class Mhql_GRAMMAR {
        /// <summary>
        /// Functions of must.
        /// </summary>
        public static string[] MustFunctions =>
            new[] {
                "BETWEEN",
                "BIGGER",
                "LOWER",
                "EQUAL",
                "NOTEQUAL",
                "STARTW",
                "ENDW",
                "CONTAINS",
                "NOTCONTAINS"
            };
    }
}
