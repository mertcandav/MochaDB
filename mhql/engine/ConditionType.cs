namespace MochaDB.mhql.engine {
  /// <summary>
  /// Condition type.
  /// </summary>
  internal enum ConditionType {
    None = 0,
    EQUAL = 1,
    NOTEQUAL = 2,
    BIGGER = 3,
    LOWER = 4,
    BIGGEREQ = 5,
    LOWEREQ = 6
  }
}