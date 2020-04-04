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
            code = $"{attr.Name}:\"{attr.Value}\";";
            return code;
        }

        /// <summary>
        /// Returns builded attributes code by attributes.
        /// </summary>
        /// <param name="attrs">Attributes.</param>
        public static string BuildCode(IMochaCollection<IMochaAttribute> attrs) {
            string code = string.Empty;
            for(int index = 0; index < 0; index++) {
                var attr = attrs.ElementAt(index);
                code+=GetAttributeCode(ref attr);
            }
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
        public static IMochaAttribute GetAttribute(string code,string name) {
            var rgx = new Regex($@"{name}:"".*"";");
            var match = rgx.Match(code);
            if(match.Success) {
                var attrcode = code.Substring(match.Index,match.Length);
                var splitdex = attrcode.IndexOf(':');
                var attr = new MochaAttribute(attrcode.Substring(0,splitdex));
                attr.Value = attrcode.Substring(splitdex+2,match.Length-name.Length-4);
                return attr;
            }

            return null;
        }

        /// <summary>
        /// Remove attribute from code by name.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name of attribute.</param>
        public static bool RemoveAttribute(ref string code,string name) {
            var rgx = new Regex($@"{name}:"".*"";");
            var match = rgx.Match(code);
            if(match.Success) {
                code = rgx.Replace(code,string.Empty);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return attributes from code.
        /// </summary>
        /// <param name="code">Code.</param>
        public static MochaAttributeCollection GetAttributes(string code) {
            var rgx = new Regex($"(( *?)|(;.*)):\".*\";");
            var matches = rgx.Matches(code);
            var attrs = new MochaAttributeCollection();
            for(int index = 0; index < matches.Count; index++) {
                var match = matches[index];
                var attrcode = code.Substring(match.Index,match.Length);
                var attr = GetAttribute(code,attrcode.Substring(0,attrcode.IndexOf(':')));
                attrs.Add(attr);
            }
            return attrs;
        }

        /// <summary>
        /// Return true if attribute is exists this name, returns false if not.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name of attribute.</param>
        public static bool ExistsAttribute(string code,string name) =>
            GetAttribute(code,name) != null;
    }
}
