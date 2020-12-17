MochaQ can be called an inquiry language that you can use for simple and basic operations.

### Commands

#### Run
Commands that can be run only. they are not commands that return values.
<table border="1">
    <tr>
       <td><strong>RESETMOCHA</strong></td>
       <td width="100%">Reset the database.</td>
    </tr>
    <tr>
       <td><strong>RESETTABLES</strong></td>
       <td width="100%">Reset the all tables.</td>
    </tr>
    <tr>
       <td><strong>CLEARLOGS</strong></td>
       <td width="100%">Clear all logs.</td>
    </tr>
    <tr>
       <td><strong>RESTORETOFIRSTLOG</strong></td>
       <td width="100%">Restore database to first keeped log.</td>
    </tr>
    <tr>
       <td><strong>RESTORETOLASTLOG</strong></td>
       <td width="100%">Restore database to last keeped log.</td>
    </tr>
    <tr>
       <td><strong>RESTORETOLOG</strong>:id</td>
       <td width="100%">Restore database to log by id.</td>
    </tr>
    <tr>
       <td><strong>RESETTABLE</strong>:TableName</td>
       <td width="100%">Reset the table.</td>
    </tr>
    <tr>
       <td><strong>CLEARROWS</strong>:TableName</td>
       <td width="100%">Clear all rows of table.</td>
    </tr>
    <tr>
       <td><strong>CREATEMOCHA</strong>:FullPath</td>
       <td width="100%">Create new MochaDB in directory.</td>
    </tr>
    <tr>
       <td><strong>SETPASSWORD</strong>:Passoword</td>
       <td width="100%">Set password of database.</td>
    </tr>
    <tr>
       <td><strong>SETDESCRIPTION</strong>:Description</td>
       <td width="100%">Set description of database.</td>
    </tr>
    <tr>
       <td><strong>REMOVETABLE</strong>:TableName</td>
       <td width="100%">Remove the table.</td>
    </tr>
    <tr>
       <td><strong>REMOVECOLUMN</strong>:TableName:ColumnName</td>
       <td width="100%">Remove the column.</td>
    </tr>
    <tr>
       <td><strong>REMOVEROW</strong>:TableName:RowIndex</td>
       <td width="100%">Remove the row.</td>
    </tr>
    <tr>
       <td><strong>CREATEMOCHA</strong>:DirectoryPath:Name</td>
       <td width="100%">Create new MochaDB in directory.</td>
    </tr>
    <tr>
       <td><strong>RENAMETABLE</strong>:TableName:NewName</td>
       <td width="100%">Rename the table.</td>
    </tr>
    <tr>
       <td><strong>RENAMECOLUMN</strong>:TableName:ColumnName:NewName</td>
       <td width="100%">Rename the column.</td>
    </tr>
    <tr>
       <td><strong>SETCOLUMNDATATYPE</strong>:TableName:ColumnName:DataTypeName</td>
       <td width="100%">Set the column datatype by name..</td>
    </tr>
    <tr>
       <td><strong>CREATETABLE</strong>:Name</td>
       <td width="100%">Create new table.</td>
    </tr>
    <tr>
       <td><strong>SETTABLEDESCRIPTION:</strong>TableName:Description</td>
       <td width="100%">Set the table description.</td>
    </tr>
    <tr>
       <td><strong>CREATECOLUMN</strong>:TableName:Name</td>
       <td width="100%">Create column in table.</td>
    </tr>
    <tr>
       <td><strong>SETCOLUMNDESCRIPTION</strong>:TableName:ColumnName:Description</td>
       <td width="100%">Set the column description.</td>
    </tr>
    <tr>
       <td><strong>UPDATEFIRSTDATA</strong>:TableName:ColumnName:NewData</td>
       <td width="100%">Update first data in column.</td>
    </tr>
    <tr>
       <td><strong>UPDATELASTDATA</strong>:TableName:ColumnName:NewData</td>
       <td width="100%">Update last data in column.</td>
    </tr>
    <tr>
       <td><strong>ADDDATA</strong>:TableName:ColumnName:Data</td>
       <td width="100%">Add new data.</td>
    </tr>
    <tr>
       <td><strong>UPDATEDATA</strong>:TableName:ColumnName:RowIndex:NewData</td>
       <td width="100%">Update data in column by index.</td>
    </tr>
    <tr>
       <td><strong>CLEARALL</strong></td>
       <td width="100%">Remove all tables and others.</td>
    </tr>
    <tr>
       <td><strong>CLEARTABLES</strong></td>
       <td width="100%">Remove all tables.</td>
    </tr>
    <tr>
       <td><strong>REMOVETABLEATTRIBUTE</strong>:TableName:AttributeName</td>
       <td width="100%">Remove attribute from table by name.</td>
    </tr>
    <tr>
       <td><strong>REMOVECOLUMNATTRIBUTE</strong>:TableName:ColumnName:AttributeName</td>
       <td width="100%">Remove attribute from column by name.</td>
    </tr>
</table>

#### GetRun
They are commands that return a value. They are returned as ``object``, Dynamic commands are not executed.
<table border="1">
    <tr>
       <td><strong>GETPASSWORD</strong></td>
       <td width="100%">Returns password of database.</td>
    </tr>
    <tr>
       <td><strong>GETDESCRIPTION</strong></td>
       <td width="100%">Returns description of database.</td>
    </tr>
    <tr>
       <td><strong>GETTABLES</strong></td>
       <td width="100%">Returns all tables in database.</td>
    </tr>
    <tr>
       <td><strong>GETLOGS</strong></td>
       <td width="100%">Returns all logs.</td>
    </tr>
    <tr>
       <td><strong>GETDATAS</strong></td>
       <td width="100%">Returns all datas in database.</td>
    </tr>
    <tr>
       <td><strong>TABLECOUNT</strong></td>
       <td width="100%">Returns table count in database.</td>
    </tr>
    <tr>
       <td><strong>GETFIRSTCOLUMN_NAME</strong>:TableName</td>
       <td width="100%">Returns first table name in database.</td>
    </tr>
    <tr>
       <td><strong>GETTABLE</strong>:TableName</td>
       <td width="100%">Returns table by name.</td>
    </tr>
    <tr>
       <td><strong>GETCOLUMNS</strong>:TableName</td>
       <td width="100%">Returns all column in table.</td>
    </tr>
    <tr>
       <td><strong>GETDATAS</strong>:TableName</td>
       <td width="100%">Returns all datas in table.</td>
    </tr>
    <tr>
       <td><strong>GETROWS</strong>:TableName</td>
       <td width="100%">Returns all rows in table.</td>
    </tr>
    <tr>
       <td><strong>COLUMNCOUNT</strong>:TableName</td>
       <td width="100%">Returns column count in table.</td>
    </tr>
    <tr>
       <td><strong>DATACOUNT</strong>:TableName</td>
       <td width="100%">Returns data count in table.</td>
    </tr>
    <tr>
       <td><strong>ROWCOUNT</strong>:TableName</td>
       <td width="100%">Returns row count in table.</td>
    </tr>
    <tr>
       <td><strong>EXISTSTABLE</strong>:TableName</td>
       <td width="100%">Returns true if table exists but return false if table not exists.</td>
    </tr>
    <tr>
       <td><strong>DATACOUNT</strong>:TableName:ColumnName</td>
       <td width="100%">Returns data count of column.</td>
    </tr>
    <tr>
       <td><strong>GETCOLUMN</strong>:TableName:ColumnName</td>
       <td width="100%">Returns row.</td>
    </tr>
    <tr>
       <td><strong>EXISTSCOLUMN</strong>:TableName:ColumnName</td>
       <td width="100%">Returns true if column exists in table but return false if column not exists in table.</td>
    </tr>
    <tr>
       <td><strong>EXISTSDATA</strong>:TableName:ColumnName:Data</td>
       <td width="100%">Returns true if data exists in table but return false if data not exists in table.</td>
    </tr>
    <tr>
       <td><strong>GETDATAS</strong>:TableName:ColumnName</td>
       <td width="100%">Returns datas in column.</td>
    </tr>
    <tr>
       <td><strong>GETCOLUMNDESCRIPTION</strong>:TableName:ColumnName</td>
       <td width="100%">Returns column description.</td>
    </tr>
    <tr>
       <td><strong>EXISTSLOG</strong>:id</td>
       <td width="100%">Returns whether there is a log with the specified id.</td>
    </tr>
    <tr>
       <td><strong>GETDATA</strong>:TableName:ColumnName:DataIndex</td>
       <td width="100%">Returns data by index.</td>
    </tr>
    <tr>
       <td><strong>GETTABLEATTRIBUTES</strong>:TableName</td>
       <td width="100%">Returns attributes from table.</td>
    </tr>
    <tr>
       <td><strong>GETCOLUMNATTRIBUTES</strong>:TableName:ColumnName</td>
       <td width="100%">Returns attributes from column.</td>
    </tr>
    <tr>
       <td><strong>GETTABLEATTRIBUTE</strong>:TableName:AttributeName</td>
       <td width="100%">Returns attribute from table by name.</td>
    </tr>
    <tr>
       <td><strong>GETCOLUMNATTRIBUTE</strong>:TableName:ColumnName:AttributeName</td>
       <td width="100%">Returns attribute from column by name.</td>
    </tr>
    <tr>
       <td><strong>GETCOLUMNDATATYPE</strong>:TableName:ColumnName</td>
       <td width="100%">Returns data type of column.</td>
    </tr>
    <tr>
       <td><strong>#REMOVETABLE</strong>:TableName</td>
       <td width="100%">Remove the table and returns result.</td>
    </tr>
    <tr>
       <td><strong>#REMOVECOLUMN</strong>:TableName:ColumnName</td>
       <td width="100%">Remove the column and returns result.</td>
    </tr>
    <tr>
       <td><strong>#REMOVEROW</strong>:TableName:RowIndex</td>
       <td width="100%">Remove the row and returns result.</td>
    </tr>
    <tr>
       <td><strong>#REMOVETABLEATTRIBUTE</strong>:TableName:AttributeName</td>
       <td width="100%">Remove attribute from table by name and returns result.</td>
    </tr>
    <tr>
       <td><strong>#REMOVECOLUMNATTRIBUTE</strong>:TableName:ColumnName:AttributeName</td>
       <td width="100%">Remove attribute from column by name and returns result.</td>
    </tr>
</table>

#### Dynamic
More functional commands without simplicity. <br>
If the value is to be returned, it returns but if not, it returns ``null``. Either way, it returns.
<table border="1">
    <tr>
       <td><strong>SELECT</strong> ColumnName,ColumnName0... <strong>FROM</strong> TableName</td>
       <td width="55.7%">Get the table with specific columns.</td>
    </tr>
</table>
