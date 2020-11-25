Represents a single data in a column. Keeps the data as "object".<br>
It is an object to use in code.


### Examples

Creates a data with "Mertcan" value in "String" data type.
```C#
MochaData myData = new MochaData(MochaDataType.String,"Mertcan");
```

The data is "Hello World!" set to.
```C#
myData.Data = "Hello World!";
```
However, if you try to enter data other than the data type, you will get Exception. Example;
```C#
myData.Data = 18;
```

Sets the data type to "Int32". if the data is compatible with the new data type, it will remain the same, if not, it will be reset to the compatible default value.
```C#
myData.DataType = MochaDataType.Int32;
```
