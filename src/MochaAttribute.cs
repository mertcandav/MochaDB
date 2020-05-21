using System;
using System.Text.RegularExpressions;
using MochaDB.engine;

namespace MochaDB {
    /// <summary>
    /// Attribute for MochaDB.
    /// </summary>
    public class MochaAttribute:IMochaAttribute {
        #region Fields

        private Regex bannedNamesRegex = new Regex(
@"^(Description)\b");

        private string
            name,
            value;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaAttribute.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        public MochaAttribute(string name) {
            Name=name;
            Value=string.Empty;
        }

        /// <summary>
        /// Create new MochaAttribute.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="value">Value of attribute.</param>
        public MochaAttribute(string name,string value) :
            this(name) {
            Value=value;
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaAttribute value) =>
            value.ToString();

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

        #region Overrides

        /// <summary>
        /// Returns <see cref="Value"/>.
        /// </summary>
        public override string ToString() {
            return Value;
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
                value=value.Trim();
                if(value==name)
                    return;

                if(string.IsNullOrWhiteSpace(value))
                    throw new MochaException("Name is cannot null or whitespace!");

                Engine_NAMES.AttributeCheckThrow(value);

                if(bannedNamesRegex.IsMatch(value))
                    throw new MochaException($@"Name is cannot ""{value}""");

                name=value;
                OnNameChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value {
            get =>
                value;
            set {
                if(this.value==value)
                    return;

                Engine_VALUES.AttributeCheckThrow(value);

                this.value=value;

                OnValueChanged(this,new EventArgs());
            }
        }

        #endregion
    }
}
