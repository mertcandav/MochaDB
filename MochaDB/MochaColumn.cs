using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is column object for MochaDB.
    /// </summary>
    public class MochaColumn:IMochaColumn {
        #region Fields

        private MochaDataType dataType;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="dataType">Data type.</param>
        /// <param name="name">Name.</param>
        public MochaColumn(string name,MochaDataType dataType) {
            Datas = new MochaColumnDataCollection(dataType);
            Name = name;
            DataType = dataType;
            Description = string.Empty;
        }

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="dataType">Data type.</param>
        /// <param name="name">Name.</param>
        public MochaColumn(string name,MochaDataType dataType,IEnumerable<MochaData> datas) :
            this(name,dataType) {
            Datas.AddRange(datas);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

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
