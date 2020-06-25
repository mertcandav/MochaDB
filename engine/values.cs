using System.Text.RegularExpressions;

namespace MochaDB.engine {
    /// <summary>
    /// Value engine of MochaDB.
    /// </summary>
    internal static class Engine_VALUES {
        /// <summary>
        /// Returns true if pass but returns false if not.
        /// </summary>
        public static bool PasswordCheck(string value) {
            var pattern = new Regex(
".*(;|=).*");
            return !pattern.IsMatch(value);
        }

        /// <summary>
        /// Check name and give exception if not pass.
        /// </summary>
        /// <param name="value">Value.</param>
        public static void PasswordCheckThrow(string value) {
            if(!PasswordCheck(value))
                throw new MochaException("The password did not meet the password conventions!");
        }

        /// <summary>
        /// Returns true if pass but returns false if not.
        /// </summary>
        public static bool AttributeCheck(string value) {
            var pattern = new Regex(
".*(;|:).*");
            return !pattern.IsMatch(value);
        }

        /// <summary>
        /// Check name and give exception if not pass.
        /// </summary>
        /// <param name="value">Value.</param>
        public static void AttributeCheckThrow(string value) {
            if(!AttributeCheck(value))
                throw new MochaException("The value did not meet the value conventions!");
        }
    }
}
