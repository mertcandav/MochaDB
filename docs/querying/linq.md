### Examples

Retrieves tables starting with the letter "M".
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
var datas =
    from value in db.GetDatas("Personels","Name")
    where value.ToString()[0] == 'M'
    select value;
```

It takes records between the ages of 18 and 30 and ranks from small to large.
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
MochaTable personels = db.GetTable("Personels");
var datas =
    from value in personels.Rows
    where ((int)value.Datas[2]) >= 18 && 30 <= ((int)value.Datas[2])
    select value;
```
