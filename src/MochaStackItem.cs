using System;
using MochaDB.engine;

namespace MochaDB {
    /// <summary>
    /// Item for stacks.
    /// </summary>
    public class MochaStackItem:IMochaStackItem {
        #region Fields

        private string name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaStackItem.
        /// </summary>
        /// <param name="name">Name of item.</param>
        public MochaStackItem(string name) {
            Items= new MochaStackItemCollection();
            Attributes = new MochaAttributeCollection();
            Name=name;
            Description=string.Empty;
            Value=string.Empty;
        }

        /// <summary>
        /// Create new MochaStackItem.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="description">Description of item.</param>
        public MochaStackItem(string name,string description) :
            this(name) {
            Description =description;
        }

        /// <summary>
        /// Create new MochaStackItem.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="description">Description of item.</param>
        /// <param name="value">Value of item.</param>
        public MochaStackItem(string name,string description,string value) :
            this(name,description) {
            Value=value;
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaStackItem value) =>
            value.ToString();

        public static explicit operator MochaElement(MochaStackItem value) =>
            value.ToElement();

        #endregion

        #region Events

        /// <summary>
        /// This happens before name changed.
        /// </summary>
        public event EventHandler<EventArgs> NameChanged;
        private void OnNameChanged(object sender,EventArgs e) {
            //Invoke.
            NameChanged?.Invoke(sender,e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns MochaSector converted to MochaElement.
        /// </summary>
        public MochaElement ToElement() =>
            new MochaElement(Name,Description,Value);

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
        /// Name of item.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                value=value.Trim();
                if(string.IsNullOrEmpty(value))
                    throw new MochaException("Name is cannot null or whitespace!");

                Engine_NAMES.CheckThrow(value);

                if(value==name)
                    return;

                name=value;
                OnNameChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Value of item.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Description of item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Items of item.
        /// </summary>
        public MochaStackItemCollection Items { get; }

        /// <summary>
        /// Attributes of item.
        /// </summary>
        public MochaAttributeCollection Attributes { get; }

        #endregion
    }
}
