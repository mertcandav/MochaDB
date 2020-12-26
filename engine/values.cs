namespace MochaDB.engine {
  /// <summary>
  /// Value engine of MochaDB.
  /// </summary>
  internal static class Engine_VALUES {
    /// <summary>
    /// Check and process float value.
    /// </summary>
    /// <param name="type">DataType.</param>
    /// <param name="value">Value.</param>
    public static void CheckFloat(MochaDataType type,ref string value) {
      if(
          type == MochaDataType.Decimal ||
          type == MochaDataType.Double ||
          type == MochaDataType.Float)
        value = value.Replace('.',',');
    }
  }
}
