using System.Text.RegularExpressions;

namespace MochaDB.engine {
    /// <summary>
    /// Attribute engine of MochaDB.
    /// </summary>
    internal static class Engine_ATTRIBUTES {
        /// <summary>
        /// Returns attribute as code.
        /// </summary>
        public static string GetAttributeCode(ref IMochaAttribute attr) {
            string code;
            code = $"{attr.Name}:{attr.Value};";
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
            var rgx = new Regex($@"{name}:.*;");
            var match = rgx.Match(code);
            if(match.Success) {
                var parts = match.Value.Split(':');
                var attr = new MochaAttribute(parts[0]);
                attr.Value = parts[1];
                attr.Value = attr.Value.Substring(0,attr.Value.Length-1);
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
            var rgx = new Regex($@"{name}:.*;");
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
            var parts = code.Split(';');
            var attrs = new MochaAttributeCollection();
            for(int index = 0; index < parts.Length; index++) {
                var attrcode = parts[index];

                if(string.IsNullOrWhiteSpace(attrcode))
                    continue;

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
