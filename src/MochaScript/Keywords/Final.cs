namespace MochaDB.MochaScript.Keywords {
    /// <summary>
    /// The "Final" keyword.
    /// </summary>
    public static class Keyword_Final {
        /// <summary>
        /// Return index if find but return -1 if not find.
        /// </summary>
        /// <param name="source">MochaScript code as lines.</param>
        public static int GetIndex(string[] source) {
            for(int index = 0; index < source.Length; index++) {
                if(source[index] == "Final")
                    return index;
            }

            return -1;
        }
    }
}