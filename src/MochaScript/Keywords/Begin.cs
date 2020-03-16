namespace MochaDB.MochaScript.Keywords {
    /// <summary>
    /// The "Begin" keyword.
    /// </summary>
    public static class Keyword_Begin {
        /// <summary>
        /// Return index if find but return -1 if not find.
        /// </summary>
        /// <param name="source">MochaScript code as lines.</param>
        public static int GetIndex(string[] source) {
            for(int index = 0; index < source.Length; index++) {
                if(source[index] == "Begin")
                    return index;
            }

            return -1;
        }
    }
}