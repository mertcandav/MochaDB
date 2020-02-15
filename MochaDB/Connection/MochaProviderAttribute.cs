using System;
using System.Text.RegularExpressions;

namespace MochaDB.Connection {
    /// <summary>
    /// Attributes for MochaProviders.
    /// </summary>
    public sealed class MochaProviderAttribute:IMochaProviderAttribute {
        #region Fields

        internal static Regex booleanAttributesRegex = new Regex(@"
AutoConnect",RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private string
            name,
            value;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaProviderAttribute.
        /// </summary>
        internal MochaProviderAttribute() {
            name=string.Empty;
            value=string.Empty;
        }

        /// <summary>
        /// Create new MochaProviderAttribute.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="value">Value of attribute.</param>
        public MochaProviderAttribute(string name,string value) {
            this.value=value;
            Name=name;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Check value by name.
        /// </summary>
        internal void CheckValue() {
            if(string.IsNullOrWhiteSpace(value)) {
                if(Name.Equals("Path",StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("File path cannot be empty!");
                if(booleanAttributesRegex.IsMatch(Name))
                    value="False";
            } else {
                if(booleanAttributesRegex.IsMatch(Name) && (
                    !value.Equals(bool.TrueString,StringComparison.InvariantCultureIgnoreCase) ||
                    !value.Equals(bool.FalseString,StringComparison.InvariantCultureIgnoreCase)))
                    value="False";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new Exception("Attribute name is can not empty or white space!");

                if(value==name)
                    return;

                name=value;
                CheckValue();
            }
        }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value {
            get =>
                value;
            set {
                this.value=value;
                CheckValue();
            }
        }

        #endregion
    }
}
