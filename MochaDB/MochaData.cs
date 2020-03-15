using System;

namespace MochaDB {
    /// <summary>
    /// This is data object for MochaDB.
    /// </summary>
    public class MochaData:IMochaData {
        #region Fields

        internal object data;
        internal MochaDataType dataType;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaData.
        /// </summary>
        internal MochaData() {

        }

        /// <summary>
        /// Create new MochaData.
        /// </summary>
        /// <param name="dataType">Data type of data.</param>
        /// <param name="data">Data value.</param>
        public MochaData(MochaDataType dataType,object data) {
            this.data = data;
            DataType = dataType;
        }

        #endregion

        #region Static

        /// <summary>
        /// Return the data equality to the data type.
        /// </summary>
        /// <param name="dataType">Base datatype.</param>
        /// <param name="data">Data to check.</param>
        public static bool IsType(MochaDataType dataType,object data) {
            if(data == null)
                return false;

            try {
                object testdata = GetDataFromString(dataType,(string)data);
                return true;
            } catch { return false; }
        }

        /// <summary>
        /// Return MochaDataType by name.
        /// </summary>
        /// <param name="name">Name of MochaDataType.</param>
        public static MochaDataType GetDataTypeFromName(string name = "String") {
            name=name.TrimStart().TrimEnd().ToLowerInvariant();
            MochaDataType dataType;
            if(Enum.TryParse(name,true,out dataType))
                return dataType;

            throw new Exception("There is no MochaDB data type by this name!");
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
            //if(dataType == MochaDataType.DateTime)
            return typeof(DateTime);
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

            throw new Exception("There is no MochaDB data type of this type!");
        }

        /// <summary>
        /// Return the object value according to the data type from the string value.
        /// </summary>
        /// <param name="dataType">Targetting data type.</param>
        /// <param name="data">String data.</param>
        public static object GetDataFromString(MochaDataType dataType,string data) {
            if(data == null)
                throw new NullReferenceException("Data is cannot null!");

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
            //if(dataType == MochaDataType.DateTime)
            return DateTime.Parse(data);
        }

        /// <summary>
        /// Return data if the conversion successfully, but return null if not successfully.
        /// </summary>
        /// <param name="dataType">Targeting data type.</param>
        /// <param name="data">Data to convert.</param>
        public static object TryGetData(MochaDataType dataType,object data) {
            if(!IsType(dataType,data)) {
                if(dataType == MochaDataType.String || dataType == MochaDataType.Unique)
                    return "";
                else if(dataType == MochaDataType.Boolean)
                    return false;
                else
                    return 0;
            }

            if(dataType == MochaDataType.String || dataType == MochaDataType.Unique) {
                if(data == null)
                    data = string.Empty;
                return data.ToString();
            }

            return GetDataFromString(dataType,data.ToString());
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Data"/> as string with UTF8.
        /// </summary>
        public override string ToString() {
            return DataType != MochaDataType.String ? Data.ToString() : Data as string;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data value.
        /// </summary>
        public object Data {
            get => data;
            set {
                if(DataType==MochaDataType.AutoInt)
                    throw new Exception("Value data cannot be edited because it is AutoInt!");

                if(!IsType(DataType,value))
                    throw new Exception("The submitted data is not compatible with the targeted data!");

                data = GetDataFromString(DataType,value.ToString());
            }
        }

        /// <summary>
        /// Data type of this data
        /// </summary>
        public MochaDataType DataType {
            get => dataType;
            set {
                if(value == dataType)
                    return;

                dataType = value;

                Data = TryGetData(DataType,Data);
            }
        }

        #endregion
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
        Unique = 16
    }
}
