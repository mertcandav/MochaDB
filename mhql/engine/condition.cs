namespace MochaDB.mhql.engine {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  using MochaDB.mhql.engine.value;
  using MochaDB.Mhql;
  using MochaDB.Querying;

  /// <summary>
  /// Condition engine for MHQL.
  /// </summary>
  internal static class MhqlEng_CONDITION {
    /// <summary>
    /// Value output for conditions.
    /// </summary>
    public struct Expressional {
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
        throw new MochaException("BIGGER operator is cannot compatible this data type!");
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
        throw new MochaException("LOWER operator is cannot compatible this data type!");
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
        throw new MochaException("BIGGEREQ operator is cannot compatible this data type!");
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
        throw new MochaException("LOWEREQ operator is cannot compatible this data type!");
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

    /// <summary>
    /// Types for values of <see cref="Expressional"/>.
    /// </summary>
    public enum ExpressionType {
      String = 1,
      Char = 2,
      Boolean = 3,
      Arithmetic = 4,
    }

    #region Members

    /// <summary>
    /// Process condition and returns condition result.
    /// </summary>
    /// <param name="command">Condition.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static bool Process(string command,MochaTableResult table,MochaRow row,bool from) {
      ConditionType type;
      if(!IsCondition(command,out type))
        return false;

      string[] parts = GetConditionParts(command,MhqlEng_CONDITION_LEXER.Operators[type.ToString()]);
      Expressional value0 = GetValue(parts[0],table,row,from);
      Expressional value1 = GetValue(parts[1],table,row,from);
      if(value0.Type != value1.Type)
        throw new MochaException("Value types is are not compatible!");

      switch(type) {
        case ConditionType.EQUAL: return value0.Equal(value1);
        case ConditionType.NOTEQUAL: return value0.NotEqual(value1);
        case ConditionType.BIGGER: return value0.Bigger(value1);
        case ConditionType.LOWER: return value0.Lower(value1);
        case ConditionType.BIGGEREQ: return value0.BiggerEqual(value1);
        case ConditionType.LOWEREQ: return value0.LowerEqual(value1);
        default: return false;
      }
    }

    /// <summary>
    /// Returns true if command is condition but returns false if not.
    /// </summary>
    /// <param name="command">Command to check.</param>
    /// <param name="type">Type of condition.</param>
    public static bool IsCondition(string command,out ConditionType type) {
      type = ConditionType.None;
      if(new Regex(".*\\(").IsMatch(command))
        return false;

      foreach(string key in MhqlEng_CONDITION_LEXER.Operators.Keys) {
        if(!command.Contains(MhqlEng_CONDITION_LEXER.Operators[key]))
          continue;
        type = (ConditionType)Enum.Parse(typeof(ConditionType),key);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns condition parts.
    /// </summary>
    /// <param name="command">Condition.</param>
    /// <param name="operator">Operator.</param>
    public static string[] GetConditionParts(string command,string @operator) {
      string[] parts = command.Split(new[] { @operator },2,0);
      if(parts.Length < 2)
        throw new MochaException("Condition is cannot processed!");
      parts[0] = parts[0].Trim();
      parts[1] = parts[1].Trim();
      return parts;
    }

    /// <summary>
    /// Returns value.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static Expressional GetValue(string value,MochaTableResult table,MochaRow row,bool from) {
      if(value.StartsWith("'")) {
        MhqlEngVal_CHAR.Process(ref value);
        return new Expressional {
          Type = ExpressionType.Char,
          Value = value
        };
      } else if(value.StartsWith("\"")) {
        MhqlEngVal_STRING.Process(ref value);
        return new Expressional {
          Type = ExpressionType.String,
          Value = value
        };
      } else if(value.StartsWith("#")) {
        decimal val;
        if(!decimal.TryParse(value.Substring(1).Replace('.',','),out val))
          throw new MochaException("Value is not arithmetic value!");
        return new Expressional {
          Type = ExpressionType.Arithmetic,
          Value = val
        };
      } else if(value == "TRUE")
        return new Expressional {
          Type = ExpressionType.Boolean,
          Value = true
        };
      else if(value == "FALSE")
        return new Expressional {
          Type = ExpressionType.Boolean,
          Value = false
        };

      if(from) {
        IEnumerable<MochaColumn> result = table.Columns.Where(x => x.Name == value);

        if(!result.Any())
          goto index;

        MochaData data = row.Datas[Array.IndexOf(table.Columns,result.First())];
        return new Expressional {
          Type =
                data.dataType == MochaDataType.Unique ||
                data.dataType == MochaDataType.String ||
                data.dataType == MochaDataType.DateTime ?
                    ExpressionType.String :
                        data.dataType == MochaDataType.Char ?
                        ExpressionType.Char :
                            data.dataType == MochaDataType.Boolean ?
                            ExpressionType.Boolean :
                                ExpressionType.Arithmetic,
          Value = data.data
        };
      }

    index:

      if(!char.IsNumber(value.FirstChar()))
        throw new MochaException("Column is not defined!");
      int dex = int.Parse(value);
      if(dex < 0)
        throw new MochaException("Index is cannot lower than zero!");
      else if(dex > row.Datas.Count - 1)
        throw new MochaException("The specified index is more than the number of columns!");
      MochaData _data = row.Datas[dex];
      return new Expressional {
        Type =
              _data.dataType == MochaDataType.Unique ||
              _data.dataType == MochaDataType.String ||
              _data.dataType == MochaDataType.DateTime ?
              ExpressionType.String :
                  _data.dataType == MochaDataType.Char ?
                  ExpressionType.Char :
                      _data.dataType == MochaDataType.Boolean ?
                      ExpressionType.Boolean :
                          ExpressionType.Arithmetic,
        Value = _data.data
      };
    }

    #endregion Members
  }
}
