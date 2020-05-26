using System.Collections.Generic;

namespace MochaDB.framework {
    /// <summary>
    /// Dictionary for MochaDB.
    /// </summary>
    internal static class Framework_DICTIONARY {
        /// <summary>
        /// Dictionary from array.
        /// </summary>
        /// <param name="array">Source array.</param>
        public static Dictionary<string,string> FromArray(string[,] array) {
            var dict = new Dictionary<string,string>();
            for(int index = 0; index < array.Length; index++) {
                dict.Add(array[index,0],array[index,1]);
            }
            return dict;
        }
    }
}
