using System;
using System.Text.RegularExpressions;

namespace MochaDB.Dynamic {
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

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                value=value.TrimStart().TrimEnd();
                if(string.IsNullOrWhiteSpace(value))
                    throw new Exception("Name is cannot null!");
                else if(bannedNamesRegex.IsMatch(value))
                    throw new Exception($@"Name is cannot ""{value}""");

                if(value==name)
                    return;

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

                this.value=value;
                OnValueChanged(this,new EventArgs());
            }
        }

        #endregion
    }
}