namespace MochaDB.engine {
    /// <summary>
    /// Lexer of MochaDB.
    /// </summary>
    internal static class Engine_LEXER {
        /// <summary>
        /// File extension of MochaDB.
        /// </summary>
        public static string Extension => ".mhdb";

        /// <summary>
        /// Version of MochaDB.
        /// </summary>
        public static string Version =>
            "3.4.5";

        /// <summary>
        /// The most basic content of the database.
        /// </summary>
        public static string EmptyContent =>
$@"<?MochaDB Version=\""{Version}""?>
<MochaDB Description=""Root element of database."">>
    <Root Description=""Root of database."">>
        <Password DataType=""String"" Description=""Password of database.""></Password>
        <Description DataType=""String"" Description=""Description of database.""></Description>
    </Root>
    <Sectors Description=""Base of sectors."">>
    </Sectors>
    <Stacks Description=""Base of stacks."">>
    </Stacks>
    <Tables Description=""Base of tables."">
    </Tables>
    <FileSystem Description=""FileSystem of database."">
        <C Type=""Disk"" Name=""Default"" Description=""Default disk.""></C>
    </FileSystem>
    <Logs Description=""Logs of database."">
    </Logs>
</MochaDB>";
    }
}
