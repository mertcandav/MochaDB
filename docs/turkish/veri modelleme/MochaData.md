Bir sütundaki tek bir veriyi temsil eder. Verileri "object" olarak tutar. <br>
Kodda kullanýlacak bir nesnedir.


### Örnekler

"String" veri tipinde "Mertcan" deðeri ile veri oluþturur.
```C#
MochaData myData = new MochaData(MochaDataType.String,"Mertcan");
```

Veri "Merhaba Dünya!" olarak ayarlanýr.
```C#
myData.Data = "Hello World!";
```

Bununla birlikte, veri türü dýþýnda bir veri girmeye çalýþýrsanýz, Exception alýrsýnýz. Misal;
```C#
myData.Data = 18;
```

Veri türünü "Int32" olarak ayarlar. Veri yeni veri tipiyle uyumluysa ayný kalacak, deðilse uyumlu varsayýlan deðere sýfýrlanacaktýr.
```C#
myData.DataType = MochaDataType.Int32;
```
