namespace MochaDB {
    /// <summary>
    /// Connection states for MochaDB.
    /// </summary>
    public enum MochaConnectionState {
        Connected,
        Disconnected
    }
    
    /// <summary>
    /// DataTypes for MochaDB.
    /// </summary>
    public enum MochaDataType {
        String,
        Int16,
        Int32,
        Int64,
        Double,
        Float,
        Decimal,
        Byte,
        Char,
        AutoInt,
        Unique
    }
}