using System;
using System.Linq;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is column object for MochaDB.
    /// </summary>
    public sealed class MochaColumn {
        #region Fields

        internal List<MochaData> datas;
        private MochaDataType dataType;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="dataType">Data type.</param>
        /// <param name="name">Name.</param>
        public MochaColumn(string name,MochaDataType dataType) {
            datas = new List<MochaData>();
            Name = name;
            DataType = dataType;
            Description = string.Empty;
        }

        /// <summary>
        /// Create new MochaColumn.
        /// </summary>
        /// <param name="dataType">Data type.</param>
        /// <param name="name">Name.</param>
        /// <param name="datas">Datas.</param>
        public MochaColumn(string name,MochaDataType dataType,MochaData[] datas)
            : this(name,dataType) {
            this.datas.AddRange(datas);
        }

        #endregion

        #region Events

        /// <summary>
        /// This happends after data changed.
        /// </summary>
        internal event EventHandler<EventArgs> Changed;

        #endregion

        #region Data

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">MochaData to add.</param>
        public void AddData(MochaData data) {
            if(DataType==MochaDataType.AutoInt)
                throw new Exception("Data cannot be added directly to a column with AutoInt!");

            if(data.DataType == DataType) {
                datas.Add(data);
                Changed?.Invoke(this,new EventArgs());
            } else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">Data to add.</param>
        public void AddData(object data) {
            if(MochaData.IsType(DataType,data))
                AddData(new MochaData(DataType,data));
            else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add datas from array.
        /// </summary>
        /// <param name="datas">Container array of datas.</param>
        public void AddDataRange(IList<MochaData> datas) {
            for(int index = 0; index < datas.Count; index++) {
                AddData(datas[index]);
            }
            Changed?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// Remove first data equal to sample data.
        /// </summary>
        /// <param name="data">Data to remove.</param>
        public void RemoveData(object data) {
            for(int index = 0; index < datas.Count; index++)
                if(datas[index].Data == data) {
                    datas.RemoveAt(index);
                    Changed?.Invoke(this,new EventArgs());
                    break;
                }
        }

        /// <summary>
        /// Removes all data equal to sample data.
        /// </summary>
        /// <param name="data">Sample data.</param>
        public void RemoveAllData(object data) {
            int count = datas.Count;
            datas = (
                from currentdata in datas
                where currentdata.Data != data
                select currentdata).ToList();

            if(datas.Count != count)
                Changed?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// Remove all datas.
        /// </summary>
        public void ClearDatas() {
            datas.Clear();
            Changed?.Invoke(this,new EventArgs());
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
        /// All datas.
        /// </summary>
        public MochaData[] Datas =>
            datas.ToArray();

        /// <summary>
        /// Count of data.
        /// </summary>
        public int DataCount =>
            datas.Count;

        /// <summary>
        /// Data type of datas.
        /// </summary>
        public MochaDataType DataType {
            get => dataType;
            set {
                if(value == dataType)
                    return;

                dataType = value;

                if(value == MochaDataType.AutoInt) {
                    return;
                }

                for(int index = 0; index < datas.Count; index++)
                    datas[index].DataType = dataType;
            }
        }

        #endregion
    }
}
