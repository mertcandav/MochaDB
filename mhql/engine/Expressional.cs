using System;

namespace MochaDB.mhql.engine {
  /// <summary>
  /// Value output for conditions.
  /// </summary>
  internal struct Expressional {
    /// <summary>
    /// Returns true if equals, returns false if not.
    /// </summary>
    /// <param name="v">Value to compare.</param>
    public bool Equal(Expressional v) =>
      Value.ToString() == v.Value.ToString();

    /// <summary>
    /// Returns true if not equals, returns false if not.
    /// </summary>
    /// <param name="v">Value to compare.</param>
    public bool NotEqual(Expressional v) =>
      Value.ToString() != v.Value.ToString();

    /// <summary>
    /// Returns true if bigger, returns false if not.
    /// </summary>
    /// <param name="v">Value to compare.</param>
    public bool Bigger(Expressional v) {
      if(Type == ExpressionType.Boolean)
        return Value.ToString() == "True" && v.Value.ToString() == "False";
      else if(Type == ExpressionType.Char)
        return (int)Value > (int)v.Value;
      else if(Type == ExpressionType.Arithmetic)
        return decimal.Parse(Value.ToString()) > decimal.Parse(v.Value.ToString());
      throw new InvalidCastException("BIGGER operator is cannot compatible this data type!");
    }

    /// <summary>
    /// Returns true if lower, returns false if not.
    /// </summary>
    /// <param name="v">Value to compare.</param>
    public bool Lower(Expressional v) {
      if(Type == ExpressionType.Boolean)
        return Value.ToString() == "False" && v.Value.ToString() == "True";
      else if(Type == ExpressionType.Char)
        return (int)Value < (int)v.Value;
      else if(Type == ExpressionType.Arithmetic)
        return decimal.Parse(Value.ToString()) < decimal.Parse(v.Value.ToString());
      throw new InvalidCastException("LOWER operator is cannot compatible this data type!");
    }

    /// <summary>
    /// Returns true if biggereq, returns false if not.
    /// </summary>
    /// <param name="v">Value to compare.</param>
    public bool BiggerEqual(Expressional v) {
      if(Type == ExpressionType.Boolean)
        return
            (Value.ToString() == "True" && v.Value.ToString() == "False") ||
            (Value.ToString() == v.Value.ToString());
      else if(Type == ExpressionType.Char)
        return (int)Value >= (int)v.Value;
      else if(Type == ExpressionType.Arithmetic)
        return decimal.Parse(Value.ToString()) >= decimal.Parse(v.Value.ToString());
      throw new InvalidCastException("BIGGEREQ operator is cannot compatible this data type!");
    }

    /// <summary>
    /// Returns true if lowereq, returns false if not.
    /// </summary>
    /// <param name="v">Value to compare.</param>
    public bool LowerEqual(Expressional v) {
      if(Type == ExpressionType.Boolean)
        return
            (Value.ToString() == "False" && v.Value.ToString() == "True") ||
            (Value.ToString() == v.Value.ToString());
      else if(Type == ExpressionType.Char)
        return (int)Value <= (int)v.Value;
      else if(Type == ExpressionType.Arithmetic)
        return decimal.Parse(Value.ToString()) <= decimal.Parse(v.Value.ToString());
      throw new InvalidCastException("LOWEREQ operator is cannot compatible this data type!");
    }

    /// <summary>
    /// Value.
    /// </summary>
    public object Value;

    /// <summary>
    /// Type of value.
    /// </summary>
    public ExpressionType Type;
  }
}
