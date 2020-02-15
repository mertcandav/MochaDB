namespace MochaDB {
    /// <summary>
    /// Data interface for MochaDB datas.
    /// </summary>
    public interface IMochaData {
        #region Properties

        public object Data { get; set; }
        public MochaDataType DataType { get; set; }

        #endregion
    }
}
