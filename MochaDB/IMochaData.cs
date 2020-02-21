namespace MochaDB {
    /// <summary>
    /// Data interface for MochaDB datas.
    /// </summary>
    public interface IMochaData {
        #region Properties

        object Data { get; set; }
        MochaDataType DataType { get; set; }

        #endregion
    }
}
