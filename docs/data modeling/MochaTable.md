Represents a table in the database. It consists of columns and rows.
It is an object to use in code.

### Examples

Creates an empty table named ``Personels``.
```C#
MochaTable PersonelTable = new MochaTable("Personels");
```

<br>

Adds the ``ID``, ``Name`` and ``Age`` columns.
```C#
PersonelTable.Columns.Add("ID",MochaDataType.AutoInt);
PersonelTable.Columns.Add("Name",MochaDataType.String);
PersonelTable.Columns.Add("Age",MochaDataType.Int32);
```

<br>

Adds ``Mertcan`` and ``18`` data to ``Name`` and ``Age`` columns. Assignment cannot be made because ``ID`` column is AutoInt, but we still need to specify it (The number of data must be the same as the column count), so I'm specifying the data of the column that is AutoInt with null. The null value will not be processed and will be replaced by the number assigned by AutoInt.
```C#
PersonelTable.Rows.Add(null,"Mertcan",18);
```
