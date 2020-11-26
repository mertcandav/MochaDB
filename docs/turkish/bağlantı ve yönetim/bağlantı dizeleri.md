Connection strings are used for connections. They are processed by ``MochaProvider``.  It is an object to use in code.

### Örnekler
```C#
"Path = .\\Databases\\Personels.mhdb"
"Path = .\\Databases\\Personels; Password=1234"
"Password= 1234;Path =.\\Databases\\Personels.mhdb; AutoConnect= True"
```

### Öznitelikler
Öznitelikler, baðlantý dizesindeki özelliklerdir. <br>
Baðlantý dizesinin oluþturulmasýna izin veren ana öðelerdir ve hepsinin kendi benzersiz iþlevleri vardýr. <br>
Bir öznitelik, baðlantý dizesinde en fazla bir kez tanýmlanabilir. Bir baðlantý dizesi "Path" özniteliðini tanýmlamýþ olmalýdýr. <br>
Bir öznitelik tanýmlanmamýþsa veya yanlýþ tanýmlanmýþsa, varsayýlan deðeri atanýr. <br>
Birden fazla öznitelik tanýmlanmýþsa öznitelikler ``;`` ile ayrýlmalýdýr.
<table border="1">
    <tr>
        <td width="15%"><strong>Ad</strong></td>
        <td width="75%"><strong>Öznitelik</strong></td>
        <td width="20%"><strong>Deðer tipi</strong></td>
    </tr>
    <tr>
        <td>Path</td>
        <td>Veritabanýnýn yolunu gösterir.</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Password</td>
        <td>Veritabanýnýn þifresini belirtir.</td>
        <td>String</td>
    </tr>
    <tr>
        <td>AutoConnect</td>
        <td">Eðer true olarak ayarlanmýþsa, otomatik olarak baðlantý açýlýr.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>Readonly</td>
        <td">Eðer true olarak ayarlanýrsa veritabanýna o baðlantý üstünden veri yazamazsýnýz.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>AutoCreate</td>
        <td">True ise, her baðlantý açýldýðýnda veritabaný yoksa yeni bir boþ veritabaný oluþturulur.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>Logs</td>
        <td">If true, a copy of the database is kept in database whenever the content changes.</td>
        <td>True/False</td>
    </tr>
</table>

### Fonksiyonlar
Fonksiyonlar, iþinizi kolaylaþtýrmak için kullanabileceðiniz basit ve kullanýmý kolay alternatiflerdir.<br>

- ```>SOURCEDIR<subcount``` <br>
Bu iþlevi yalnýzca ``Path`` özniteliðinde kullanabilirsiniz. Komut dosyasýna eriþilen konumu döndürür. Alt hesap parametresi ise daha yüksek bir dizine kaç kez gideceðini gösteren sayýdýr. Konuma doðrudan eriþim saðlamak istiyorsanýz, ``0`` yazabilirsiniz. Hiç belirtilmezse, iþlev tanýnmaz ve bir hataya neden olabileceðinden ``Path`` özniteliðinin içeriðini etkileyebilir.
<br><b>Examples</b><br>
```>SOURCEDIR<0```: Þunu döndürür ```C:\Users\user\Desktop``` <br>
```Path= >SOURCEDIR<3\testdocs\testdb.mhdb``` -- Döndürür: ```C:\``` <br>
```Path= >SOURCEDIR<0\testdocs\testdb.mhdb``` -- Döndürür: ```C:\Users\user\Desktop``` <br>
```Path= >SOURCEDIR<1\testdocs\testdb.mhdb``` -- Döndürür: ```C:\Users\user``` <br>
```Path= >SOURCEDIR<\testdocs\testdb.mhdb``` -- Çýktý Path: ```>SOURCEDIR<\testdocs\testdb.mhdb```
