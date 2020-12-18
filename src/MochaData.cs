namespace MochaDB {
  using System;

  using MochaDB.Data.Types;

  /// <summary>
  /// This is data object for MochaDB.
  /// </summary>
  public class MochaData {
    #region Fields

    internal object data;
    internal MochaDataType dataType;

    #endregion Fields

    #region Internal Constructors

    /// <summary>
    /// Create new instance of <see cref="MochaData"/>.
    /// </summary>
    internal protected MochaData() { }

    #endregion Internal Constructors

    #region Constructors

    /// <summary>
    /// Create new instance of <see cref="MochaData"/>.
    /// </summary>
    /// <param name="dataType">Data type of data.</param>
    /// <param name="data">Data value.</param>
    public MochaData(MochaDataType dataType,object data) {
      this.data = data;
      DataType = dataType;
    }

    #endregion Constructors

    #region Static Members

    /// <summary>
    /// Convert value to MochaData.
    /// </summary>
    /// <param name="value">Value.</param>
    public static MochaData Parse(object value) {
      MochaDataType type = GetDataTypeFromType(value.GetType());
      return new MochaData(
          type,
          GetDataFromString(type,value.ToString()));
    }

    /// <summary>
    /// Try convert value to MochaData.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="data">Output.</param>
    public static bool TryParse(object value,out MochaData data) {
      try {
        data = Parse(value);
        return true;
      } catch { }

      data = null;
      return false;
    }

    /// <summary>
    /// Return the data equality to the data type.
    /// </summary>
    /// <param name="dataType">Base datatype.</param>
    /// <param name="data">Data to check.</param>
    public static bool IsType(MochaDataType dataType,object data) {
      if(data == null)
        return false;

      try {
        object testdata = GetDataFromString(dataType,data.ToString());
        return true;
      } catch { return false; }
    }

    /// <summary>
    /// Return MochaDataType by name.
    /// </summary>
    /// <param name="name">Name of MochaDataType.</param>
    public static MochaDataType GetDataTypeFromName(string name = "String") {
      name=name.Trim().ToLowerInvariant();
      MochaDataType dataType;
      if(Enum.TryParse(name,true,out dataType))
        return dataType;

      throw new MochaException("There is no MochaDB data type by this name!");
    }

    /// <summary>
    /// Return Type object from DataType targeting data.
    /// </summary>
    /// <param name="dataType">Targeted data type.</param>
    public static Type GetTypeFromDataType(MochaDataType dataType) {
      if(dataType == MochaDataType.String || dataType == MochaDataType.Unique)
        return typeof(string);
      if(dataType == MochaDataType.Byte)
        return typeof(byte);
      if(dataType == MochaDataType.Char)
        return typeof(char);
      if(dataType == MochaDataType.Decimal)
        return typeof(decimal);
      if(dataType == MochaDataType.Double)
        return typeof(double);
      if(dataType == MochaDataType.Float)
        return typeof(float);
      if(dataType == MochaDataType.Int16)
        return typeof(short);
      if(dataType == MochaDataType.Int32 || dataType == MochaDataType.AutoInt)
        return typeof(int);
      if(dataType == MochaDataType.Int64)
        return typeof(long);
      if(dataType == MochaDataType.Boolean)
        return typeof(bool);
      if(dataType == MochaDataType.SByte)
        return typeof(sbyte);
      if(dataType == MochaDataType.UInt16)
        return typeof(ushort);
      if(dataType == MochaDataType.UInt32)
        return typeof(uint);
      if(dataType == MochaDataType.UInt64)
        return typeof(ulong);
      if(dataType == MochaDataType.DateTime)
        return typeof(DateTime);
      return typeof(Bit);
    }

    /// <summary>
    /// Return MochaDataType from Type object.
    /// </summary>
    /// <param name="type">Type object.</param>
    public static MochaDataType GetDataTypeFromType(Type type) {
      if(type == typeof(string))
        return MochaDataType.String;
      if(type == typeof(byte))
        return MochaDataType.Byte;
      if(type == typeof(char))
        return MochaDataType.Char;
      if(type == typeof(decimal))
        return MochaDataType.Decimal;
      if(type == typeof(double))
        return MochaDataType.Double;
      if(type == typeof(float))
        return MochaDataType.Float;
      if(type == typeof(short))
        return MochaDataType.Int16;
      if(type == typeof(int))
        return MochaDataType.Int32;
      if(type == typeof(long))
        return MochaDataType.Int64;
      if(type == typeof(bool))
        return MochaDataType.Boolean;
      if(type == typeof(sbyte))
        return MochaDataType.SByte;
      if(type == typeof(ushort))
        return MochaDataType.UInt16;
      if(type == typeof(uint))
        return MochaDataType.UInt32;
      if(type == typeof(ulong))
        return MochaDataType.UInt64;
      if(type == typeof(DateTime))
        return MochaDataType.DateTime;
      if(type == typeof(Bit))
        return MochaDataType.Bit;

      throw new MochaException("There is no MochaDB data type of this type!");
    }

    /// <summary>
    /// Return the object value according to the data type from the string value.
    /// </summary>
    /// <param name="dataType">Targetting data type.</param>
    /// <param name="data">String data.</param>
    public static object GetDataFromString(MochaDataType dataType,string data) {
      if(data == null)
        throw new MochaException("Data is cannot null!");

      if(dataType == MochaDataType.String || dataType == MochaDataType.Unique)
        return data;
      if(dataType == MochaDataType.AutoInt || dataType == MochaDataType.Int32)
        return int.Parse(data);
      if(dataType == MochaDataType.Byte)
        return byte.Parse(data);
      if(dataType == MochaDataType.Char)
        return char.Parse(data);
      if(dataType == MochaDataType.Decimal)
        return decimal.Parse(data);
      if(dataType == MochaDataType.Double)
        return double.Parse(data);
      if(dataType == MochaDataType.Float)
        return float.Parse(data);
      if(dataType == MochaDataType.Int16)
        return short.Parse(data);
      if(dataType == MochaDataType.Int64)
        return long.Parse(data);
      if(dataType == MochaDataType.Byte)
        return byte.Parse(data);
      if(dataType == MochaDataType.Boolean)
        return bool.Parse(data);
      if(dataType == MochaDataType.SByte)
        return sbyte.Parse(data);
      if(dataType == MochaDataType.UInt16)
        return ushort.Parse(data);
      if(dataType == MochaDataType.UInt32)
        return uint.Parse(data);
      if(dataType == MochaDataType.UInt64)
        return ulong.Parse(data);
      if(dataType == MochaDataType.DateTime)
        return DateTime.Parse(data);
      return Bit.Parse(data);
    }

    /// <summary>
    /// Return data if the conversion successfully, but return null if not successfully.
    /// </summary>
    /// <param name="dataType">Targeting data type.</param>
    /// <param name="data">Data to convert.</param>
    public static object TryGetData(MochaDataType dataType,object data) {
      if(!IsType(dataType,data)) {
        if(dataType == MochaDataType.String || dataType == MochaDataType.Unique)
          return string.Empty;
        else if(dataType == MochaDataType.Boolean)
          return false;
        else
          return 0;
      }

      if(dataType == MochaDataType.String || dataType == MochaDataType.Unique)
        return data.ToString();

      return GetDataFromString(dataType,data.ToString());
    }

    /// <summary>
    /// Returns true if datatype is numeric, returns false if not.
    /// </summary>
    /// <param name="type">Datatype to check.</param>
    public static bool IsNumericType(MochaDataType type) =>
        type == MochaDataType.AutoInt ||
        type == MochaDataType.Bit ||
        type == MochaDataType.Byte ||
        type == MochaDataType.Decimal ||
        type == MochaDataType.Double ||
        type == MochaDataType.Float ||
        type == MochaDataType.Int16 ||
        type == MochaDataType.Int32 ||
        type == MochaDataType.Int64 ||
        type == MochaDataType.SByte ||
        type == MochaDataType.UInt16 ||
        type == MochaDataType.UInt64;

    #endregion Static Members

    #region Overrides

    /// <summary>
    /// Returns <see cref="Data"/> as string with UTF8.
    /// </summary>
    public override string ToString() =>
      DataType != MochaDataType.String ? Data.ToString() : Data as string;

    #endregion Overrides

    #region Properties

    /// <summary>
    /// Data value.
    /// </summary>
    public virtual object Data {
      get => data;
      set {
        if(DataType==MochaDataType.AutoInt)
          throw new MochaException("Value data cannot be edited because it is AutoInt!");

        if(!IsType(DataType,value))
          throw new MochaException("The submitted data is not compatible with the targeted data!");

        data = GetDataFromString(DataType,value.ToString());
      }
    }

    /// <summary>
    /// Data type of this data
    /// </summary>
    public virtual MochaDataType DataType {
      get => dataType;
      set {
        if(value == dataType)
          return;

        dataType = value;
        Data = TryGetData(DataType,Data);
      }
    }

    #endregion Properties
  }

  /// <summary>
  /// DataTypes for MochaDB.
  /// </summary>
  public enum MochaDataType {
    String = 0,
    Int16 = 1,
    Int32 = 2,
    Int64 = 3,
    UInt16 = 4,
    UInt32 = 5,
    UInt64 = 6,
    Double = 7,
    Float = 8,
    Decimal = 9,
    Byte = 10,
    DateTime = 11,
    SByte = 12,
    Boolean = 13,
    Char = 14,
    AutoInt = 15,
    Unique = 16,
    Bit = 17
  }
}
