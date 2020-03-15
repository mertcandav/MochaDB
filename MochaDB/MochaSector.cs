using System;

namespace MochaDB {
    /// <summary>
    /// This is sector object for MochaDB.
    /// </summary>
    public class MochaSector:IMochaSector {
        #region Fields

        private string name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaSector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        public MochaSector(string name) {
            Name = name;
            Description = string.Empty;
            Data = string.Empty;
        }

        /// <summary>
        /// Create new MochaSector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data value.</param>
        public MochaSector(string name,string data)
            : this(name) {
            Data = data;
        }

        /// <summary>
        /// Create new MochaSector.
        /// </summary>
        /// <param name="name">Name of sector.</param>
        /// <param name="data">Data value.</param>
        /// <param name="description">Description of sector.</param>
        public MochaSector(string name,string data,string description)
            : this(name,data) {
            Description = description;
        }

        #endregion

        #region Events

        /// <summary>
        /// This happens after name changed;
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
            new MochaElement(Name,Description,Data);

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Data"/>.
        /// </summary>
        public override string ToString() {
            return Data;
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
                if(string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Name is cannot null or whitespace!");

                if(value==name)
                    return;

                name=value;
                OnNameChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
