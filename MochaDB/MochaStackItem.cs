using MochaDB.Collections;
using System;

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

        #region Properties

        /// <summary>
        /// Name of item.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
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

        #endregion
    }
}
