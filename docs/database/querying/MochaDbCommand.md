MochaDbCommand Allows you to process MHQL queries. <br>
The database must be set as the base and the MochaDatabase must be connected to a database.

### Examples
Creates a new MochaQuery and creates a MochaDatabase and adds a reference.
```C#
MochaDatabase db = new MochaDatabase("mydb.mhdb");
MochaDbCommand query = new MochaDbCommand("USE *", db);
```

It processes the query in the database and pulls single data with ``ExecuteScalar``. ``ExecuteScalar`` is the most logical choice because the keyword ``USE`` always returns a table.
```C#
MochaDbCommand command = new MochaDbCommand("USE *", db);
MochaTableResult table = command.ExecuteScalar();
```

### Favorite Functions
<table border="1">
    <tr>
        <td><strong>ExecuteScalar()</strong></td>
        <td width="100%">Retrieves only one of the returned result. It is the most logical choice if you have written a query that will return single result.</td>
    </tr>
</table>
