MochaDatabase, MochaDB veritabaný dosyalarýna eriþir ve bunlarý yönetme olanaðý sunar. <br>
Fonksiyonlar yardýmýyla kolay yönetim saðlar. <br>
Kodda kullanýlacak bir nesnedir.

### Örnekler
MochaDB veri tabanýna baðlanmak için baðlantý dizesini kullanýn.
```C#
MochaDatabase db = new MochaDatabase("connection string");
```

Yalnýzca dosya yolunu ve parolayý belirterek baðlanýn.
```C#
MochaDatabase db = new MochaDatabase("path","password");
```

Baðlantý kapalýyken herhangi bir iþlem yapmaya çalýþmak bir hatayla sonuçlanacaktýr. Ýþlemler sýrasýnda baðlantý açýk olmalýdýr.
```C#
db.Connect(); // Baðlantýyý açar.
db.Disconnect(); // Baðlantýyý kapar.
```

### Sýk kullanýlan Fonksiyonlar
#### Genel
<table border="1">
    <tr>
        <td><strong>GetPassword()</strong></td>
        <td width="100%">Veritabanýnýn þifresini döndürür.</td>
    </tr>
    <tr>
        <td><strong>SetPassword(string)</strong></td>
        <td width="100%">Veritabanýnýn þifresini ayarlar.</td>
    </tr>
    <tr>
        <td><strong>GetDescription()</strong></td>
        <td width="100%">Veritabanýnýn açýklamasýný döndürür.</td>
    </tr>
    <tr>
        <td><strong>SetDescription(string)</strong></td>
        <td width="100%">Veritabanýnýn açýklamasýný ayarlar.</td>
    </tr>
    <tr>
        <td><strong>Reset()</strong></td>
        <td width="100%">MochaDB, veritabaný dosyasýnýn varlýðýný kontrol eder ve yoksa yeni bir dosya oluþturur. TÜM VERÝLER KAYBOLUR!</td>
    </tr>
    <tr>
        <td><strong>GetXML()</strong></td>
        <td width="100%">Veritabanýnýn XML þemasýný döndürür.</td>
    </tr>
</table>

#### Tablolar
<table border="1">
    <tr>
        <td><strong>AddTable(MochaTable)</strong></td>
        <td width="100%">Bir tablo ekler.</td>
    </tr>
    <tr>
        <td><strong>RemoveTable(string)</strong></td>
        <td width="100%">Adýný kullanarak bir tabloyu siler.</td>
    </tr>
    <tr>
        <td><strong>GetTable(string)</strong></td>
        <td width="100%">Adýný kullanarak bir tabloyu alýr.</td>
    </tr>
    <tr>
        <td><strong>GetTables()</strong></td>
        <td width="100%">Veritabanýndaki tüm tablolarý alýr.</td>
    </tr>
    <tr>
        <td><strong>ExistsTable(string)</strong></td>
        <td width="100%">Adýna göre tablonun olup olmadýðýný kontrol eder.</td>
    </tr>
</table>

#### Sütunlar
<table border="1">
    <tr>
        <td><strong>AddColumn(string,MochaColumn)</strong></td>
        <td width="100%">Bir sütun ekler.</td>
    </tr>
    <tr>
        <td><strong>RemoveColumn(string,string)</strong></td>
        <td width="100%">Adýný kullanarak bir sütunu siler.</td>
    </tr>
    <tr>
        <td><strong>GetColumns(string)</strong></td>
        <td width="100%">Adýný kullanarak bir tablodaki tüm sütunlarý alýr.</td>
    </tr>
    <tr>
        <td><strong>ExistsColumn(string,string)</strong></td>
        <td width="100%">Adýna göre sütunun olup olmadýðýný kontrol eder.</td>
    </tr>
</table>

#### Satýrlar
<table border="1">
    <tr>
        <td><strong>AddRow(string,MochaRow)</strong></td>
        <td width="100%">Bir satýr ekler.</td>
    </tr>
    <tr>
        <td><strong>RemoveRow(string,int)</strong></td>
        <td width="100%">Ýndeks kullanarak bir satýrý siler.</td>
    </tr>
    <tr>
        <td><strong>GetRow(string,int)</strong></td>
        <td width="100%">Ýndeks kullanarak bir satýrý alýr.</td>
    </tr>
    <tr>
        <td><strong>GetRows(string)</strong></td>
        <td width="100%">Adýna göre bir tablodaki tüm satýrlarý alýr.</td>
    </tr>
</table>

#### Veriler
<table border="1">
    <tr>
        <td><strong>AddData(string,string,MochaData)</strong></td>
        <td width="100%">Veri ekler.</td>
    </tr>
    <tr>
        <td><strong>UpdateData(string,string,int,object)</strong></td>
        <td width="100%">Ýndeks kullanarak veriyi günceller.</td>
    </tr>
    <tr>
        <td><strong>GetData(string,string,int)</strong></td>
        <td width="100%">Ýndeks kullanarak veriyi alýr.</td>
    </tr>
    <tr>
        <td><strong>GetDatas(string,string)</strong></td>
        <td width="100%">Adýný kullanarak sütundaki tüm verileri alýr.</td>
    </tr>
</table>
