using System;
using System.Text.RegularExpressions;

namespace MochaDB.Connection {
    /// <summary>
    /// Attributes for MochaProviders.
    /// </summary>
    public class MochaProviderAttribute:IMochaProviderAttribute {
        #region Fields

        internal static Regex booleanAttributesRegex = new Regex(@"
AutoConnect|Readonly|AutoCreate",RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        internal string
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

        #region Events

        /// <summary>
        /// This happens after name changed.
        /// </summary>
        public event EventHandler<EventArgs> NameChanged;
        private void OnNameChanged(object sender,EventArgs e) {
            //Invoke.
            NameChanged?.Invoke(sender,e);
        }

        /// <summary>
        /// This happens after value changed.
        /// </summary>
        public event EventHandler<EventArgs> ValueChanged;
        private void OnValueChanged(object sender,EventArgs e) {
            //Invoke.
            ValueChanged?.Invoke(sender,e);
        }

        #endregion

        #region Methods

        #region Internal

        /// <summary>
        /// Check value by name.
        /// </summary>
        internal void CheckValue() {
            if(string.IsNullOrWhiteSpace(value)) {
                if(Name.Equals("Path"))
                    throw new Exception("File path cannot be empty!");
                if(booleanAttributesRegex.IsMatch(Name))
                    value="False";
            } else {
                if(booleanAttributesRegex.IsMatch(Name) && (!value.Equals("true",StringComparison.OrdinalIgnoreCase) ||
                    !value.Equals("false",StringComparison.OrdinalIgnoreCase)))
                    value="False";
            }
        }

        #endregion

        /// <summary>
        /// Returns string as in provider.
        /// </summary>
        public string GetProviderString() =>
            $"{Name}={Value};";

        #endregion

        #region Overrides

        /// <summary>
        /// Returns provider string.
        /// </summary>
        public override string ToString() {
            return GetProviderString();
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
                OnNameChanged(this,new EventArgs());
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
                OnValueChanged(this,new EventArgs());
                CheckValue();
            }
        }

        #endregion
    }
}
