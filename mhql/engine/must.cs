namespace MochaDB.mhql.engine {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using MochaDB.mhql.keywords;
  using MochaDB.mhql.must;
  using MochaDB.mhql.must.functions;
  using MochaDB.Mhql;
  using MochaDB.Querying;

  /// <summary>
  /// MHQL MUST core.
  /// </summary>
  internal class MhqlEng_MUST {
    /// <summary>
    /// Process mhql query part.
    /// </summary>
    /// <param name="value">Query.</param>
    /// <param name="table">Table.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static void ProcessPart(ref string value,MochaTableResult table,bool from) {
      if(!from) {
        int _dex = value.IndexOf('(');
        if(_dex == -1)
          return;
        string _val = value.Substring(0,_dex).Trim();
        value = _val + value.Substring(_dex);
        return;
      }

      int dex = value.IndexOf('(');
      if(dex == -1)
        return;
      string val = value.Substring(0,dex).Trim();
      IEnumerable<MochaColumn> result = table.Columns.Where(x => x.Name == val);
      if(result.Count() == 0)
        return;
      value = Array.IndexOf(table.Columns,result.First()) + value.Substring(dex);
    }

    /// <summary>
    /// Returns data by command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="row">Base row.</param>
    public static MochaData GetDataFromCommand(string command,MochaRow row) {
      command = command.Trim();
      if(!char.IsNumber(command.FirstChar()))
        throw new MochaException("Column is not defined!");
      int bracketdex = command.IndexOf('(');
      if(bracketdex == -1)
        throw new MochaException("Pattern is not defined!");
      int dex = int.Parse(command.Substring(0,bracketdex));

      if(dex < 0)
        throw new MochaException("Index is cannot lower than zero!");
      else if(dex > row.Datas.MaxIndex())
        throw new MochaException("The specified index is more than the number of columns!");
      return row.Datas[dex];
    }

    /// <summary>
    /// Returns command must result.
    /// </summary>
    /// <param name="tdb">Target database.</param>
    /// <param name="command">Command.</param>
    /// <param name="table">Table.</param>
    /// <param name="row">Row.</param>
    /// <param name="from">Use state FROM keyword.</param>
    public static bool IsPassTable(MochaDatabase tdb,ref string command,MochaTableResult table,MochaRow row,bool from) {
      command = command.Trim();
      if(Mhql_INEQ.IsINEQ(command)) {
        return Mhql_INEQ.Process(tdb,command,table,row,from);
      } else if(Mhql_IN.IsIN(command)) {
        return Mhql_IN.Process(tdb,command,table,row,from);
      } else if(MhqlEng_CONDITION.IsCondition(command,out _)) {
        return MhqlEng_CONDITION.Process(command,table,row,from);
      } else if(char.IsNumber(command.FirstChar())) {
        return MhqlMust_REGEX.Match(
                    MhqlMust_REGEX.GetCommand(command),
                    GetDataFromCommand(command,row).ToString());
      } else if(command.StartsWith("BETWEEN(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_BETWEEN.Pass(command.Substring(8,command.Length-9),table,row,from);
      } else if(command.StartsWith("BIGGER(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_BIGGER.Pass(command.Substring(7,command.Length-8),table,row,from);
      } else if(command.StartsWith("LOWER(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_LOWER.Pass(command.Substring(6,command.Length-7),table,row,from);
      } else if(command.StartsWith("EQUAL(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_EQUAL.Pass(command.Substring(6,command.Length-7),table,row,from);
      } else if(command.StartsWith("NOTEQUAL(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_NOTEQUAL.Pass(command.Substring(9,command.Length-10),table,row,from);
      } else if(command.StartsWith("STARTW(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_STARTW.Pass(command.Substring(7,command.Length-8),table,row,from);
      } else if(command.StartsWith("ENDW(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_ENDW.Pass(command.Substring(5,command.Length-6),table,row,from);
      } else if(command.StartsWith("CONTAINS(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_CONTAINS.Pass(command.Substring(9,command.Length-10),table,row,from);
      } else if(command.StartsWith("NOTCONTAINS(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_NOTCONTAINS.Pass(command.Substring(12,command.Length-13),table,row,from);
      } else if(command.StartsWith("NOTSTARTW(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_NOTSTARTW.Pass(command.Substring(10,command.Length-11),table,row,from);
      } else if(command.StartsWith("NOTENDW(",StringComparison.OrdinalIgnoreCase) && command.LastChar() == ')') {
        return MhqlMustFunc_NOTENDW.Pass(command.Substring(8,command.Length-9),table,row,from);
      } else
        throw new MochaException($"'{command}' is cannot processed!");
    }
  }
}
