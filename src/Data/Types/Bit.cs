using System;

namespace MochaDB.Data.Types {
  /// <summary>
  /// Bit data type of MochaDB.
  /// </summary>
  public struct Bit {
    #region Fields

    private char value;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value">Value.</param>
    public Bit(bool value) =>
      this.value = value ? '1' : '0';

    #endregion Constructors

    #region Operators

    public static implicit operator Bit(bool value) =>
        new Bit(value);

    public static implicit operator Bit(int value) =>
        Parse(value);

    public static implicit operator Bit(uint value) =>
        Parse(value);

    public static implicit operator Bit(double value) =>
        Parse(value);

    public static implicit operator Bit(float value) =>
        Parse(value);

    public static implicit operator Bit(long value) =>
        Parse(value);

    public static implicit operator Bit(ulong value) =>
        Parse(value);

    public static implicit operator Bit(byte value) =>
        Parse(value);

    public static implicit operator Bit(sbyte value) =>
        Parse(value);

    public static implicit operator Bit(short value) =>
        Parse(value);

    public static implicit operator Bit(ushort value) =>
        Parse(value);

    public static implicit operator bool(Bit value) =>
        value.value == '1';

    public static implicit operator int(Bit value) =>
        value.value == '1' ? 1 : 0;

    public static implicit operator uint(Bit value) =>
        value.value == '1' ? 1u : 0u;

    public static implicit operator double(Bit value) =>
        value.value == '1' ? 1d : 0d;

    public static implicit operator float(Bit value) =>
        value.value == '1' ? 1f : 0f;

    public static implicit operator long(Bit value) =>
        value.value == '1' ? 1 : 0;

    public static implicit operator ulong(Bit value) =>
        value.value == '1' ? 1ul : 0ul;

    public static implicit operator byte(Bit value) =>
        value.value == '1' ? (byte)1 : (byte)0;

    public static implicit operator sbyte(Bit value) =>
        value.value == '1' ? (sbyte)1 : (sbyte)0;

    public static implicit operator short(Bit value) =>
        value.value == '1' ? (short)1 : (short)0;

    public static implicit operator ushort(Bit value) =>
        value.value == '1' ? (ushort)1 : (ushort)0;

    #endregion Operators

    #region Static Members

    /// <summary>
    /// Returns bit from value.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static Bit Parse(bool value) =>
        new Bit(value);

    /// <summary>
    /// Returns bit from value.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static Bit Parse(int value) {
      if(value < 0 || value > 1)
        throw new OutOfMemoryException("A bit can only be 0 or 1!");
      return new Bit(value == 1);
    }

    /// <summary>
    /// Returns bit from value.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static Bit Parse(double value) {
      if(value < 0 || value > 1)
        throw new OutOfMemoryException("A bit can only be 0 or 1!");
      return new Bit(value == 1);
    }

    /// <summary>
    /// Returns bit from value.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static Bit Parse(string value) =>
        Parse(int.Parse(value));

    /// <summary>
    /// Returns bit from value if parse is successfully.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static bool TryParse(int value,out Bit bit) {
      if(value < 0 || value > 1) {
        bit = new Bit();
        return false;
      }
      bit = new Bit(value == 1);
      return true;
    }

    /// <summary>
    /// Returns bit from value if parse is successfully.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static bool TryParse(float value,out Bit bit) {
      if(value < 0 || value > 1) {
        bit = new Bit();
        return false;
      }
      bit = new Bit(value == 1);
      return true;
    }

    /// <summary>
    /// Returns bit from value if parse is successfully.
    /// </summary>
    /// <param name="value">Value to parse.</param
    public static bool TryParse(string value,out Bit bit) {
      try {
        bit = Parse(int.Parse(value));
        return true;
      } catch {
        bit = new Bit();
        return false;
      }
    }

    #endregion Static Members

    #region Overrides

    /// <summary>
    /// Returns value as string.
    /// </summary>
    public override string ToString() =>
      value.ToString();

    #endregion Overrides
  }
}
