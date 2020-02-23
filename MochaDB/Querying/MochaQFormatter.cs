using System.Text;
using System.Text.RegularExpressions;

namespace MochaDB.Querying {
    /// <summary>
    /// Formatter for MochaQ commands.
    /// </summary>
    public static class MochaQFormatter {
        #region Fields

        private static string specialKeywords =
@"BREAKQUERY";

        private static string runKeywords =
@"RESETMOCHA|RESETTABLES|RESETTABLE|CREATEMOCHA|SETPASSWORD|SETDESCRIPTION|REMOVETABLE|REMOVESECTOR|
REMOVECOLUMN|REMOVEROW|RENAMETABLE|RENAMECOLUMN|RENAMESECTOR|RENAMESTACK|RENAMESTACKITEM|SETCOLUMNDATATYPE|CREATETABLE|
SETSECTORDATA|SETSECTORDESCRIPTION|ADDSECTOR|SETTABLEDESCRIPTION|UPDATEFIRSTDATA|UPDATELASTDATA|ADDDATA|UPDATEDATA|CREATESTACK|
SETSTACKDESCRIPTION|REMOVESTACK|REMOVESTACKITEM|SETSTACKITEMVALUE|SETSTACKITEMDESCRIPTION|CLEARSECTORS|CLEARSTACKS|CLEARTABLES|CLEARALL|
FILESYSTEM_CLEARDISKS|FILESYSTEM_REMOVEDISK|FILESYSTEM_REMOVEDIRECTORY|FILESYSTEM_REMOVEFILE|FILESYSTEM_UPLOADFILE";

        private static string getRunKeywords =
@"GETPASSWORD|GETDESCRIPTION|GETTABLES|GETDATAS|TABLECOUNT|GETFIRSTCOLUMN_NAME|GETTABLE|GETCOLUMNS|GETDATAS|GETROWS|COLUMNCOUNT|
DATACOUNT|ROWCOUNT|EXISTSTABLE|EXISTSSECTOR|GETSECTORDATA|GETSECTORDESCRIPTION|DATACOUNT|GETCOLUMN|EXISTSDATA|GETDATAS|GETCOLUMNDESCRIPTION|
GETSECTOR|EXISTSSTACK|EXISTSSTACKITEM|GETSTACKITEMVALUE|GETSTACKDESCRIPTION|GETSTACKITEMDESCRIPTION|GETDATA|GETCOLUMNDATATYPE|
FILESYSTEM_EXISTSDIRECTORY|FILESYSTEM_EXISTSDISK|FILESYSTEM_EXISTSFILE|";

        private static string dynamicKeywords =
@"SELECT|FROM";

        private static Regex specialKeywordsRegex = new Regex(
$@"^({specialKeywords})$",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        private static Regex runKeywordsRegex = new Regex(
$@"^({runKeywords})$",
RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        private static Regex getRunKeywordsRegex = new Regex(
$@"^({getRunKeywords})$",
RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        private static Regex dynamicKeywordsRegex = new Regex(
$@"^({dynamicKeywords})$",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        private static Regex specialKeywordsUnlimitedRegex = new Regex(specialKeywords,
            RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
        private static Regex runKeywordsUnlimitedRegex = new Regex(runKeywords,
            RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
        private static Regex getRunKeywordsUnlimitedRegex = new Regex(getRunKeywords,
            RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
        private static Regex dynamicKeywordsUnlimitedRegex = new Regex(dynamicKeywords,
            RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

        #endregion

        #region Static

        /// <summary>
        /// Return true if value is MochaQ keyword but return false if not.
        /// </summary>
        /// <param name="value">Value to check.</param>
        public static bool IsKeyword(string value) =>
            specialKeywordsRegex.IsMatch(value) ||
            runKeywordsRegex.IsMatch(value) ||
            getRunKeywordsRegex.IsMatch(value) ||
            dynamicKeywordsRegex.IsMatch(value);

        /// <summary>
        /// Replace MochaQ keywords to upper case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static void UpperCaseKeywords(ref string value) {
            MatchCollection specialMatches = specialKeywordsUnlimitedRegex.Matches(value);
            MatchCollection runMatches = runKeywordsUnlimitedRegex.Matches(value);
            MatchCollection getRunMatches = getRunKeywordsUnlimitedRegex.Matches(value);
            MatchCollection dynamicMatches = dynamicKeywordsUnlimitedRegex.Matches(value);

            StringBuilder valueSB = new StringBuilder(value);

            for(int index = 0; index < specialMatches.Count; index++) {
                Match match = specialMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToUpperInvariant(),match.Index,match.Length);
            }

            for(int index = 0; index < runMatches.Count; index++) {
                Match match = runMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToUpperInvariant(),match.Index,match.Length);
            }

            for(int index = 0; index < getRunMatches.Count; index++) {
                Match match = getRunMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToUpperInvariant(),match.Index,match.Length);
            }

            for(int index = 0; index < dynamicMatches.Count; index++) {
                Match match = dynamicMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToUpperInvariant(),match.Index,match.Length);
            }

            value = valueSB.ToString();
        }

        /// <summary>
        /// Replace MochaQ keywords to lower case.
        /// </summary>
        /// <param name="value">The value to targeting.</param>
        public static void LowerCaseKeywords(ref string value) {
            MatchCollection specialMatches = specialKeywordsUnlimitedRegex.Matches(value);
            MatchCollection runMatches = runKeywordsUnlimitedRegex.Matches(value);
            MatchCollection getRunMatches = getRunKeywordsUnlimitedRegex.Matches(value);
            MatchCollection dynamicMatches = dynamicKeywordsUnlimitedRegex.Matches(value);

            StringBuilder valueSB = new StringBuilder(value);

            for(int index = 0; index < specialMatches.Count; index++) {
                Match match = specialMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToLowerInvariant(),match.Index,match.Length);
            }

            for(int index = 0; index < runMatches.Count; index++) {
                Match match = runMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToLowerInvariant(),match.Index,match.Length);
            }

            for(int index = 0; index < getRunMatches.Count; index++) {
                Match match = getRunMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToLowerInvariant(),match.Index,match.Length);
            }

            for(int index = 0; index < dynamicMatches.Count; index++) {
                Match match = dynamicMatches[index];
                if(!match.Success)
                    continue;

                valueSB.Replace(match.Value,match.Value.ToLowerInvariant(),match.Index,match.Length);
            }

            value = valueSB.ToString();
        }

        #endregion
    }
}