Represents a row in the table. Each data shows the data of a column in that row. <br>
It is an object to use in code.

### Examples

Creates new empty row.
```C#
MochaRow myRow = new MochaRow();
```

Creates a MochaData object of the "String" data type and sets its value to "Mertcan" and adds it to the row data. Since the data type is "String", the data type of that column also means "String".
```C#
myRow.Add(new MochaData(MochaDataType.String,"Mertcan"));
```

Delete the data in index "0".
```C#
myRow.RemoveDataAt(0);
```
