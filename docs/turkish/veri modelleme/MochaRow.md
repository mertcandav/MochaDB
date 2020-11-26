Tablodaki bir satýrý temsil eder. Her veri, o satýrdaki bir sütunun verilerini gösterir. <br>
Kodda kullanýlacak bir nesnedir.

### Örnekler

Yeni boþ bir satýr oluþturur.
```C#
MochaRow myRow = new MochaRow();
```

"String" veri türünde bir MochaData nesnesi oluþturur, deðerini "Mertcan" olarak ayarlar ve satýr verilerine ekler. Veri türü "String" olduðundan, bu sütunun veri türü de "String" anlamýna gelir.
```C#
myRow.Add(new MochaData(MochaDataType.String,"Mertcan"));
```

"0" dizinindeki veriyi siler.
```C#
myRow.RemoveDataAt(0);
```
