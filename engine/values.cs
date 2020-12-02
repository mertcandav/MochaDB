﻿namespace MochaDB.engine {
  using System.Text.RegularExpressions;

  /// <summary>
  /// Value engine of MochaDB.
  /// </summary>
  internal static class Engine_VALUES {
    /// <summary>
    /// Returns true if pass but returns false if not.
    /// </summary>
    public static bool PasswordCheck(string value) =>
      !new Regex(".*(;|=).*").IsMatch(value);

    /// <summary>
    /// Check name and give exception if not pass.
    /// </summary>
    /// <param name="value">Value.</param>
    public static void PasswordCheckThrow(string value) {
      if(!PasswordCheck(value))
        throw new MochaException("The password did not meet the password conventions!");
    }

    /// <summary>
    /// Returns true if pass but returns false if not.
    /// </summary>
    public static bool AttributeCheck(string value) =>
      !new Regex(".*(;|:).*").IsMatch(value);

    /// <summary>
    /// Check name and give exception if not pass.
    /// </summary>
    /// <param name="value">Value.</param>
    public static void AttributeCheckThrow(string value) {
      if(!AttributeCheck(value))
        throw new MochaException("The value did not meet the value conventions!");
    }

    /// <summary>
    /// Check and process float value.
    /// </summary>
    /// <param name="type">DataType.</param>
    /// <param name="value">Value.</param>
    public static void __CHK_FLOAT__(MochaDataType type,ref string value) {
      if(
          type == MochaDataType.Decimal ||
          type == MochaDataType.Double ||
          type == MochaDataType.Float)
        value = value.Replace('.',',');
    }
  }
}
