using System;
using System.Linq;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is column object for MochaDB.
    /// </summary>
    public class MochaColumn {
        #region Fields

        private List<MochaData> datas;
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

        #region Methods

        /// <summary>
        /// It adapts all transported data to data type.
        /// </summary>
        public void AdapteDatasValue() {
            if(DataType == MochaDataType.AutoInt) {
                for(int index = 0; index < Datas.Count; index++) {
                    Datas[index].DataType = MochaDataType.Int32;
                    Datas[index].Data = index;
                }
            }
        }

        #endregion

        #region Data

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">MochaData to add.</param>
        public void AddData(MochaData data) {
            if(data.DataType == DataType)
                datas.Add(data);
            else
                throw new Exception("This data's datatype not compatible column datatype.");
        }

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">Data to add.</param>
        public void AddData(object data) {
            if(MochaData.IsType(DataType,data))
                datas.Add(new MochaData(DataType,data));
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
        }

        /// <summary>
        /// Remove first data equal to sample data.
        /// </summary>
        /// <param name="data">Data to remove.</param>
        public void RemoveData(object data) {
            IList<MochaData> datas = Datas;
            for(int index = 0; index < datas.Count; index++)
                if(datas[index].Data == data) {
                    this.datas.RemoveAt(index);
                    break;
                }
        }

        /// <summary>
        /// Removes all data equal to sample data.
        /// </summary>
        /// <param name="data">Sample data.</param>
        public void RemoveAllData(object data) {
            /*MochaData[] datas = Datas;
            for(int index = 0; index < datas.Length; index++)
                if(datas[index].Data == data)
                    this.datas.RemoveAt(index);*/

            datas = (
                from currentdata in datas
                where currentdata.Data != data
                select currentdata).ToList();
        }

        /// <summary>
        /// Remove all datas.
        /// </summary>
        public void ClearDatas() =>
            datas.Clear();

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
        public IList<MochaData> Datas =>
            datas;

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
                    AdapteDatasValue();
                    return;
                }

                for(int index = 0; index < datas.Count; index++)
                    datas[index].DataType = dataType;
            }
        }

        #endregion
    }
}
