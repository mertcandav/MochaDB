using System.Text.RegularExpressions;

namespace MochaDB.engine {
    /// <summary>
    /// MochaDB attribute engine.
    /// </summary>
    internal static class Engine_ATTRIBUTES {
        /// <summary>
        /// Returns attribute as code.
        /// </summary>
        public static string GetAttributeCode(ref IMochaAttribute attr) {
            string code;
            code = $"{attr.Name}:\"{attr.Value}\"";
            return code;
        }

        /// <summary>
        /// Append attribute code to code.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="attr">Attribute to add code.</param>
        public static void AppendAttribute(ref string code,ref IMochaAttribute attr) {
            code += GetAttributeCode(ref attr);
        }

        /// <summary>
        /// Returns attribute from code by name.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name of attribute.</param>
        public static MochaAttribute GetAttribute(ref string code,string name) {
            var rgx = new Regex($@"{name}:"".*"";");
            var match = rgx.Match(code);
            if(match.Success) {
                var attrcode = code.Substring(match.Index,match.Length);
                var splitdex = attrcode.IndexOf(':');
                var attr = new MochaAttribute(attrcode.Substring(0,splitdex));
                attr.Value = attrcode.Substring(splitdex+2,attrcode.Length-splitdex+3);
                return attr;
            }

            return null;
        }

        /// <summary>
        /// Return attributes from code.
        /// </summary>
        /// <param name="code">Code.</param>
        public static MochaAttributeCollection GetAttributes(ref string code) {
            var rgx = new Regex($"(( *?)|(;.*)):\".*\";");
            var matches = rgx.Matches(code);
            var attrs = new MochaAttributeCollection();
            for(int index = 0; index < matches.Count; index++) {
                var match = matches[index];
                var attrcode = code.Substring(match.Index,match.Length);
                var attr = GetAttribute(ref code,attrcode.Substring(0,attrcode.IndexOf(':')));
                attrs.Add(attr);
            }
            return attrs;
        }
    }
}
