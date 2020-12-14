namespace MochaDB.engine {
  /// <summary>
  /// Lexer of MochaDB.
  /// </summary>
  internal static class Engine_LEXER {
    /// <summary>
    /// File extension of MochaDB.
    /// </summary>
    public static string __EXTENSION__ => ".mhdb";

    /// <summary>
    /// MochaScript file extension of MochaDB.
    /// </summary>
    public static string __SCRIPT_EXTENSION__ => ".mhsc";

    /// <summary>
    /// Version of MochaDB.
    /// </summary>
    public static string __VERSION__ => "3.4.9";

    /// <summary>
    /// The most basic content of the database.
    /// </summary>
    public static string __EMPTY__ =>
$@"<?MochaDB Version=\""{__VERSION__}""?>
<MochaDB Description=""Root element of database."">>
    <Root Description=""Root of database."">>
        <Password DataType=""String"" Description=""Password of database.""></Password>
        <Description DataType=""String"" Description=""Description of database.""></Description>
    </Root>
    <Tables Description=""Base of tables."">
    </Tables>
    <Logs Description=""Logs of database."">
    </Logs>
</MochaDB>";
  }
}
