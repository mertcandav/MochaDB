``MochaQuery`` MochaQ sorgularýný iþlemenizi saðlar. <br>
Bir ``MochaDatabase`` veritabaný temsilcisine baðlanmalýdýr.

### Örnekler
Yeni bir ``MochaQuery`` oluþturur ve bir ``MochaDatabase`` oluþturup referans ekler.
```C#
MochaQuery query = new MochaQuery(new MochaDatabase("path=.\\mydb.mhdb"));
```

<strong>UYARI</strong><br>
Normal bir þekilde oluþturulmasý gerekmediðinde tehlikeli olabilir. <br>
`` MochaDatabase`` nesnesine sahip olmanýzý gerektirir, ancak ``MochaDatabase`` nesnesinin `` Query`` özelliðinin zaten kendisine eþlenmiþ bir ``MochaQuery`` nesnesi döndürdüðünü unutmayýn.

### Sýk kullanýlan fonksiyonlar
<table border="1">
    <tr>
        <td><strong>Run()</strong></td>
        <td width="100%">Etkin MochaQ sorgusunu çalýþtýrýr. Gelen bir deðer olsa bile geri dönmeyecektir. MochaQ "Run" komutlarý için.</td>
    </tr>
    <tr>
        <td><strong>GetRun()</strong></td>
        <td width="100%">Etkin MochaQ sorgusunu çalýþtýrýr. Gelen deðeri döndürür. "GetRun" MochaQ komutlarý için.</td>
    </tr>
    <tr>
        <td><strong>Dynamic()</strong></td>
        <td width="100%">Deðer döndürülürse, iþlevi döndürür ve iþlevi yerine getirir; deðilse, sadece iþlevi yerine getirir. "Dynamic" MochaQ komutlarý için.</td>
    </tr>
</table>
