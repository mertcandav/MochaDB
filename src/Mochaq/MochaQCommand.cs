namespace MochaDB.Mochaq {
  using MochaDB.Querying;

  /// <summary>
  /// MochaQ query for MochaDB.
  /// </summary>
  public struct MochaQCommand {
    #region Fields

    private string command;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new MochaQCommand.
    /// </summary>
    /// <param name="command">MochaQ command.</param>
    public MochaQCommand(string command) :
        this() => Command=command;

    #endregion Constructors

    #region Operators

    public static implicit operator string(MochaQCommand value) =>
        value.Command;

    public static implicit operator MochaQCommand(string value) =>
        new MochaQCommand(value);

    #endregion Operators

    #region Members

    /// <summary>
    /// Set command.
    /// </summary>
    /// <param name="command">Command to set.</param>
    public void SetCommand(string command) =>
        Command = command;

    /// <summary>
    /// Return true if this MochaQ command ise GetRun command but return false if not.
    /// </summary>
    public bool IsGetRunQuery() {
      string command = Command.ToUpperInvariant();
      if(
          !string.IsNullOrEmpty(command) && (
          command.StartsWith("GET") ||
          command.StartsWith("TABLECOUNT") ||
          command.StartsWith("COLUMNCOUNT") ||
          command.StartsWith("ROWCOUNT") ||
          command.StartsWith("DATACOUNT") ||
          command.StartsWith("EXISTS") ||
          command.FirstChar() == '#'))
        return true;
      else
        return false;
    }

    /// <summary>
    /// Return true if this MochaQ command ise Run command but return false if not.
    /// </summary>
    public bool IsRunQuery() {
      string command = Command.ToUpperInvariant();
      if(
          !string.IsNullOrEmpty(command) && (
          command.StartsWith("RESET") ||
          command.StartsWith("SET") ||
          command.StartsWith("ADD") ||
          command.StartsWith("CREATE") ||
          command.StartsWith("CLEAR") ||
          command.StartsWith("REMOVE") ||
          command.StartsWith("RENAME") ||
          command.StartsWith("UPDATE") ||
          command.StartsWith("RESTORE")))
        return true;
      else
        return false;
    }

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns <see cref="Command"/>.
    /// </summary>
    public override string ToString() =>
      Command;

    #endregion Overrides

    #region Properties

    /// <summary>
    /// MochaQ command.
    /// </summary>
    public string Command {
      get => command;
      set {
        value=value.Trim();

        if(value == command)
          return;

        command=value;
      }
    }

    #endregion Properties
  }
}
