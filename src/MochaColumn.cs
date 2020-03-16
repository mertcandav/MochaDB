using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is column object for MochaDB.
    /// </summary>
    public class MochaColumn:IMochaColumn {
        #region Fields

        private MochaDataType dataType;
        private string name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="name">Name.</param>
        public MochaColumn(string name) {
            Datas = new MochaColumnDataCollection(MochaDataType.String);
            Name = name;
            DataType = MochaDataType.String;
            Description = string.Empty;
        }

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="name">Name of column.</param>
        /// <param name="dataType">Data type of column.</param>
        public MochaColumn(string name,MochaDataType dataType) :
            this(name) {
            Name = name;
            DataType = dataType;
        }

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="name">Name of column.</param>
        /// <param name="dataType">Data type of column.</param>
        public MochaColumn(string name,MochaDataType dataType,IEnumerable<MochaData> datas) :
            this(name,dataType) {
            Datas.AddRange(datas);
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaColumn value) =>
            value.ToString();

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

        #region Overrides

        /// <summary>
        /// Returns <see cref="Name"/>.
        /// </summary>
        public override string ToString() {
            return Name;
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
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Datas of column.
        /// </summary>
        public MochaColumnDataCollection Datas { get; }

        /// <summary>
        /// Data type of datas.
        /// </summary>
        public MochaDataType DataType {
            get => dataType;
            set {
                if(value == dataType)
                    return;

                dataType = value;
                Datas.DataType=value;
            }
        }

        #endregion
    }
}
