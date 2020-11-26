MochaDatabase, MochaDB accesses database files and offers the ability to manage them.<br>
It provides easy management with the help of functions.<br>
It is an object to use in code.

### Examples
Use connection string to connect to the MochaDB database.
```C#
MochaDatabase db = new MochaDatabase("connection string");
```

Connect only by specifying the file path and password.
```C#
MochaDatabase db = new MochaDatabase("path","password");
```

Trying to take any action with the connection closed will result in an error. The connection must be open during the operations.
```C#
db.Connect(); // For open connection.
db.Disconnect(); // For close connection.
```

### Favorite Functions
#### General
<table border="1">
    <tr>
        <td><strong>GetPassword()</strong></td>
        <td width="100%">Returns the password of the MochaDB database.</td>
    </tr>
    <tr>
        <td><strong>SetPassword(string)</strong></td>
        <td width="100%">Sets the MochaDB Database password.</td>
    </tr>
    <tr>
        <td><strong>GetDescription()</strong></td>
        <td width="100%">Returns the description of the database.</td>
    </tr>
    <tr>
        <td><strong>SetDescription(string)</strong></td>
        <td width="100%">Sets the description of the database.</td>
    </tr>
    <tr>
        <td><strong>Reset()</strong></td>
        <td width="100%">MochaDB checks the existence of the database file and if not creates a new file. ALL DATA IS LOST!</td>
    </tr>
    <tr>
        <td><strong>GetXML()</strong></td>
        <td width="100%">Return xml schema of database.</td>
    </tr>
</table>

#### Tables
<table border="1">
    <tr>
        <td><strong>AddTable(MochaTable)</strong></td>
        <td width="100%">Add table.</td>
    </tr>
    <tr>
        <td><strong>RemoveTable(string)</strong></td>
        <td width="100%">Remove table by name.</td>
    </tr>
    <tr>
        <td><strong>GetTable(string)</strong></td>
        <td width="100%">Return table by name.</td>
    </tr>
    <tr>
        <td><strong>GetTables()</strong></td>
        <td width="100%">Return tables in database.</td>
    </tr>
    <tr>
        <td><strong>ExistsTable(string)</strong></td>
        <td width="100%">Returns whether there is a table with the specified name.</td>
    </tr>
</table>

#### Columns
<table border="1">
    <tr>
        <td><strong>AddColumn(string,MochaColumn)</strong></td>
        <td width="100%">Add colum in table.</td>
    </tr>
    <tr>
        <td><strong>RemoveColumn(string,string)</strong></td>
        <td width="100%">Remove column from table by name.</td>
    </tr>
    <tr>
        <td><strong>GetColumns(string)</strong></td>
        <td width="100%">Return columns in table by name.</td>
    </tr>
    <tr>
        <td><strong>ExistsColumn(string,string)</strong></td>
        <td width="100%">Returns whether there is a column with the specified name.</td>
    </tr>
</table>

#### Rows
<table border="1">
    <tr>
        <td><strong>AddRow(string,MochaRow)</strong></td>
        <td width="100%">Add row in table.</td>
    </tr>
    <tr>
        <td><strong>RemoveRow(string,int)</strong></td>
        <td width="100%">Remove row from table by index.</td>
    </tr>
    <tr>
        <td><strong>GetRow(string,int)</strong></td>
        <td width="100%">Return row from table by index.</td>
    </tr>
    <tr>
        <td><strong>GetRows(string)</strong></td>
        <td width="100%">Return rows from table.</td>
    </tr>
</table>

#### Datas
<table border="1">
    <tr>
        <td><strong>AddData(string,string,MochaData)</strong></td>
        <td width="100%">Add data.</td>
    </tr>
    <tr>
        <td><strong>UpdateData(string,string,int,object)</strong></td>
        <td width="100%">Update data by index.</td>
    </tr>
    <tr>
        <td><strong>GetData(string,string,int)</strong></td>
        <td width="100%">Return data by index.</td>
    </tr>
    <tr>
        <td><strong>GetDatas(string,string)</strong></td>
        <td width="100%">Return datas in column from table by name.</td>
    </tr>
</table>
