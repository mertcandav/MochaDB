MochaDbCommand Allows you to process MHQL queries.
The database must be set as the base and the MochaDatabase must be connected to a database.

### Examples
Creates a new MochaQuery and creates a MochaDatabase and adds a reference.
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
MochaDbCommand query = new MochaDbCommand("USE *",db);
```

It processes the query in the database and pulls single data with ``ExecuteScalar``. ``ExecuteScalar`` is the most logical choice because the keyword ``USE`` always returns a table. Since the rotator always returns ``object`` type, we need to specify what type it is. The table returned from ``USE`` MHQL queries is always MochaTableResult.
```C#
MochaDbCommand command = new MochaDbCommand("USE *",db);
MochaTableResult table = command.ExecuteScalar() as MochaTableResult;
```

It attracts all objects and gets them with ``ExecuteReader``. It then returns the ``while`` loop as if the data was read and takes its value as ``IMochaDatabaseItem`` and returns its name as a message.
```C#
MochaDbCommand command = new MochaDbCommand("SELECT ()",db);
MochaReader<object> results = command.ExecuteReader();
while(results.Read()) {
    IMochaDatabaseItem item = (IMochaDatabaseItem)results.Value;
    Console.WriteLine(item.Name);
}
```

### Favorite Functions
<table border="1">
    <tr>
        <td><strong>ExecuteScalar()</strong></td>
        <td width="100%">Retrieves only one of the returned result. It is the most logical choice if you have written a query that will return single result.</td>
    </tr>
    <tr>
        <td><strong>ExecuteScalarTable()</strong></td>
        <td width="100%">Returns scalar result as MochaTableResult. Handy for queries that take a single table.</td>
    </tr>
    <tr>
        <td><strong>ExecuteReader()</strong></td>
        <td width="100%">Returns all incoming result with 'MochaReader'. It should be used in queries with the possibility of more than one result.</td>
    </tr>
</table>
