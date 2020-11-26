Veritabanýndaki bir tabloyu temsil eder. Sütun ve satýrlardan oluþur. <br>
Kodda kullanýlacak bir nesnedir.

### Örnekler

``Personels`` adýnda yeni bir boþ tablo oluþturur.
```C#
MochaTable PersonelTable = new MochaTable("Personels");
```

``ID``, ``Name`` ve ``Age`` adýnda sütunlar ekler.
```C#
PersonelTable.Columns.Add("ID",MochaDataType.AutoInt);
PersonelTable.Columns.Add("Name",MochaDataType.String);
PersonelTable.Columns.Add("Age",MochaDataType.Int32);
```

``Mertcan`` ve ``18`` verilerini ``Name`` ve ``Age`` sütunlarýna ekler. `` ID '' sütunu AutoInt olduðu için atama yapýlamýyor, ancak yine de belirtmemiz gerekiyor (Veri sayýsý sütun sayýsýyla ayný olmalýdýr), bu yüzden AutoInt olan sütuna Null deðer belirtiyorum. Null deðer iþlenmeyecek ve AutoInt tarafýndan atanan numara ile deðiþtirilecektir.
```C#
PersonelTable.Rows.Add(null,"Mertcan",18);
```
