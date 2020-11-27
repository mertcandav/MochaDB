MochaDbCommand MHQL sorgularýný iþlemenizi saðlar. <br>
Veritabaný temel olarak ayarlanmalý ve MochaDatabase bir veritabanýna baðlanmalýdýr.

### Örnekler
Yeni bir MochaQuery oluþturur ve bir MochaDatabase oluþturur ve bir referans ekler.
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
MochaDbCommand query = new MochaDbCommand("USE *",db);
```

Veritabanýndaki sorguyu iþler ve `` ExecuteScalar`` ile tek veriyi çeker. `` ExecuteScalar '' en mantýklý seçimdir çünkü `` USE '' anahtar kelimesi her zaman bir tablo döndürür. Döndürücü her zaman ``nesne`` türünü döndürdüðü için, ne tür olduðunu belirlememiz gerekir. ``USE`` MHQL sorgularýndan döndürülen tablo her zaman MochaTableResult'dur.
```C#
MochaDbCommand command = new MochaDbCommand("USE *",db);
MochaTableResult table = command.ExecuteScalar() as MochaTableResult;
```

Tüm nesneleri çeker ve ``ExecuteReader`` ile alýr. Daha sonra sanki veriler okunmuþ gibi ``while`` döngüsünü döndürür ve deðerini ``MochaTable`` olarak alýr ve adýný bir mesaj olarak döndürür.
```C#
MochaDbCommand command = new MochaDbCommand("SELECT ()",db);
MochaReader<object> results = command.ExecuteReader();
while(results.Read())
  Console.WriteLine(((MochaTable)item).Name);
```

### Sýk kullanýlan fonksiyonlar
<table border="1">
    <tr>
        <td><strong>ExecuteScalar()</strong></td>
        <td width="100%">Döndürülen sonuçlardan yalnýzca birini alýr. Tek sonuç döndürecek bir sorgu yazdýysanýz en mantýklý seçim budur.</td>
    </tr>
    <tr>
        <td><strong>ExecuteScalarTable()</strong></td>
        <td width="100%">Sonucu MochaTableResult olarak döndürür. Tek bir tablo alan sorgular için kullanýþlýdýr.</td>
    </tr>
    <tr>
        <td><strong>ExecuteReader()</strong></td>
        <td width="100%">Gelen tüm sonuçlarý 'MochaReader' ile döndürür. Birden fazla sonuç olasýlýðý olan sorgularda kullanýlmalýdýr.</td>
    </tr>
</table>
