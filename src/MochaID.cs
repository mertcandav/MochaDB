namespace MochaDB {
  using System;

  /// <summary>
  /// ID for MochaID.
  /// </summary>
  public struct MochaID {
    #region Constructors

    /// <summary>
    /// Create a new MochaID.
    /// </summary>
    /// <param name="id">ID Value.</param>
    public MochaID(string id) =>
      Value = id;

    /// <summary>
    /// Create a new MochaID.
    /// </summary>
    /// <param name="type">ID type.</param>
    public MochaID(MochaIDType type) =>
      Value = GetID(type);

    #endregion Constructors

    #region Internal Static Members

    /// <summary>
    /// Flat ID algorithm.
    /// </summary>
    internal static string Flat() {
      Random rnd = new Random();
      string value = string.Empty;
      for(int counter = 1; counter <= 10; ++counter)
        value += (char)rnd.Next('a','z');
      return value;
    }

    /// <summary>
    /// Bit ID algorithm.
    /// </summary>
    internal static string Bit() {
      Random rnd = new Random();
      string value = string.Empty;
      for(int counter = 0; counter <= 9; ++counter)
        value += rnd.Next(0,9);
      return value;
    }

    /// <summary>
    /// Hexabit ID algorithm.
    /// </summary>
    internal static string Hexabit() {
      Random rnd = new Random();

      string
          flat = Flat(),
          bit = Bit(),
          value = string.Empty;

      for(int counter = 1; counter <= 10; ++counter)
        value += counter%2==1 ? flat[rnd.Next(0,9)] : bit[rnd.Next(0,9)];
      return value;
    }

    /// <summary>
    /// Hexabitx ID algorithm.
    /// </summary>
    internal static string Hexabitx() {
      Random rnd = new Random();
      string value = string.Empty;
      for(int counter = 1; counter <= 20; ++counter) {
        int dex = rnd.Next(0,2);
        value +=
            dex == 0 ? ((char)rnd.Next('a','z')) :
            dex == 1 ? Bit()[rnd.Next(0,9)] : Hexabit()[rnd.Next(0,9)];
      }
      return value;
    }

    /// <summary>
    /// Hash ID algorithms.
    /// </summary>
    internal static string Hash(int rate) {
      Random rnd = new Random();
      string value = string.Empty;
      for(int counter = 1; counter <= rate; ++counter)
        value += $"{rnd.Next(0,9).GetHashCode()}";
      return value;
    }

    #endregion Internal Static Members

    #region Static Members

    /// <summary>
    /// Get ID by ID Type.
    /// </summary>
    /// <param name="type">ID type.</param>
    public static string GetID(MochaIDType type) =>
        type == MochaIDType.Flat ? Flat() :
        type == MochaIDType.Bit ? Bit() :
        type == MochaIDType.Hexabit ? Hexabit() :
        type == MochaIDType.Hexabitx ? Hexabitx() :
        type == MochaIDType.Hash16 ? Hash(16) :
        type == MochaIDType.Hash32 ? Hash(32) :
        type == MochaIDType.Hash64 ? Hash(64) :
        type == MochaIDType.Hash128 ? Hash(128) : Hash(248);

    #endregion

    #region Members

    /// <summary>
    /// Set id value by ID type.
    /// </summary>
    /// <param name="type">Type of ID.</param>
    public void SetValue(MochaIDType type) =>
      Value = GetID(type);

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() =>
      Value;

    #endregion Overrides

    #region Properties

    /// <summary>
    /// ID.
    /// </summary>
    public string Value { get; set; }

    #endregion Properties
  }

  /// <summary>
  /// Enum for MochaID types.
  /// </summary>
  public enum MochaIDType {
    Flat = 0,
    Bit = 1,
    Hexabit = 2,
    Hexabitx = 3,
    Hash248 = 4,
    Hash128 = 5,
    Hash64 = 6,
    Hash32 = 7,
    Hash16 = 8
  }
}
