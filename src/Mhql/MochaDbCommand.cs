namespace MochaDB.Mhql {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using MochaDB.mhql;
  using MochaDB.mhql.keywords;

  /// <summary>
  /// MHQL Command processor for MochaDB.
  /// </summary>
  public class MochaDbCommand {
    #region Fields

    private string command;
    private MochaDatabase db;
    internal MhqlKeyword[] keywords;
    internal Mhql_USE USE;
    internal Mhql_REMOVE REMOVE;
    internal Mhql_ORDERBY ORDERBY;
    internal Mhql_MUST MUST;
    internal Mhql_GROUPBY GROUPBY;
    internal Mhql_SUBROW SUBROW;
    internal Mhql_SUBCOL SUBCOL;
    internal Mhql_DELCOL DELCOL;
    internal Mhql_DELROW DELROW;
    internal Mhql_ADDROW ADDROW;
    internal Mhql_CORDERBY CORDERBY;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new MochaDbCommand.
    /// </summary>
    /// <param name="db">Target MochaDatabase.</param>
    public MochaDbCommand(MochaDatabase db) {
      //Load mhql core.
      USE = new Mhql_USE(Database);
      ORDERBY = new Mhql_ORDERBY(Database);
      GROUPBY = new Mhql_GROUPBY(Database);
      MUST = new Mhql_MUST(Database);
      REMOVE = new Mhql_REMOVE(Database);
      SUBROW = new Mhql_SUBROW(Database);
      SUBCOL = new Mhql_SUBCOL(Database);
      DELROW = new Mhql_DELROW(Database);
      DELCOL = new Mhql_DELCOL(Database);
      ADDROW = new Mhql_ADDROW(Database);
      CORDERBY = new Mhql_CORDERBY(Database);
      keywords = new MhqlKeyword[] {
                USE, REMOVE, ORDERBY, GROUPBY, MUST, SUBROW,
                SUBCOL, DELROW, DELCOL, ADDROW, CORDERBY
            };
      Database=db;
      Command=string.Empty;
    }

    /// <summary>
    /// Create a new MochaDbCommand.
    /// </summary>
    /// <param name="command">MQL Command.</param>
    /// <param name="db">Target MochaDatabase.</param>
    public MochaDbCommand(string command,MochaDatabase db) :
        this(db) => Command=command;

    #endregion Constructors

    #region Internal Members

    /// <summary>
    /// Check connection and database.
    /// </summary>
    internal virtual void CheckConnection() {
      if(Database==null)
        throw new MochaException("Target database is cannot null!");
      if(Database.State!=MochaConnectionState.Connected)
        throw new MochaException("Connection is not open!");
    }

    #endregion Internal Members

    #region Members

    /// <summary>
    /// Returns first result or null.
    /// </summary>
    /// <param name="command">MQL Command to set.</param>
    public virtual MochaTableResult ExecuteScalar(string command) {
      Command=command;
      return ExecuteScalar();
    }

    /// <summary>
    /// Returns first result or null.
    /// </summary>
    public virtual MochaTableResult ExecuteScalar() {
      CheckConnection();

      bool fromkw;
      string lastcommand;
      if(!USE.Command.StartsWith("USE",StringComparison.OrdinalIgnoreCase))
        throw new MochaException("MHQL is cannot processed!");
      string use = USE.GetUSE(out lastcommand);
      fromkw = Mhql_FROM.IsFROM(use);
      MochaTableResult table = USE.GetTable(use,fromkw);

      do {
        if(ORDERBY.IsORDERBY(lastcommand)) //Orderby
          ORDERBY.OrderBy(ORDERBY.GetORDERBY(lastcommand,out lastcommand),ref table,fromkw);
        else if(GROUPBY.IsGROUPBY(lastcommand)) //Groupby
          GROUPBY.GroupBy(GROUPBY.GetGROUPBY(lastcommand,out lastcommand),ref table,fromkw);
        else if(MUST.IsMUST(lastcommand)) //Must
          MUST.MustTable(MUST.GetMUST(lastcommand,out lastcommand),ref table,fromkw);
        else if(SUBROW.IsSUBROW(lastcommand)) //Subrow
          SUBROW.Subrow(SUBROW.GetSUBROW(lastcommand,out lastcommand),ref table);
        else if(SUBCOL.IsSUBCOL(lastcommand)) //Subcol.
          SUBCOL.Subcol(SUBCOL.GetSUBCOL(lastcommand,out lastcommand),ref table);
        else if(DELROW.IsDELROW(lastcommand)) //Delrow
          DELROW.Delrow(DELROW.GetDELROW(lastcommand,out lastcommand),ref table);
        else if(DELCOL.IsDELCOL(lastcommand)) //Delcol
          DELCOL.Delcol(DELCOL.GetDELCOL(lastcommand,out lastcommand),ref table);
        else if(ADDROW.IsADDROW(lastcommand)) //Addrow
          ADDROW.Addrow(ADDROW.GetADDROW(lastcommand,out lastcommand),ref table);
        else if(CORDERBY.IsCORDERBY(lastcommand)) // Corderby
          CORDERBY.COrderBy(CORDERBY.GetCORDERBY(lastcommand,out lastcommand),ref table);
        else if(lastcommand == string.Empty) { //Return.
          IEnumerable<MochaColumn> cols = table.Columns.Where(x => x.Tag != "$");
          if(cols.Count() != table.Columns.Length) {
            table.Columns = cols.ToArray();
            table.SetRowsByDatas();
          }
          break;
        } else
          throw new MochaException($"'{lastcommand}' command is cannot processed!");
      } while(true);
      return table;
    }

    #endregion Members

    #region Properties

    /// <summary>
    /// Current MQL command.
    /// </summary>
    public virtual string Command {
      get => command;
      set {
        if(value==command)
          return;

        command = Mhql_LEXER.RemoveComments(value).Trim();
        for(int index = 0; index < keywords.Length; ++index)
          keywords[index].Command = command;
        command = value;
      }
    }

    /// <summary>
    /// Target database.
    /// </summary>
    public virtual MochaDatabase Database {
      get => db;
      set {
        if(value==null)
          throw new MochaException("This MochaDatabase is not affiliated with a database!");
        if(value==db)
          return;

        db = value;
        for(int index = 0; index < keywords.Length; ++index)
          keywords[index].Tdb = value;
      }
    }

    #endregion Properties
  }
}
