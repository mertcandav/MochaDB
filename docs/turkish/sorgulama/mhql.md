MHQL(<b>M</b>oc<b>h</b>adb <b>Q</b>uery <b>L</b>anguage), SQL benzeri bir sorgulama dilidir.<br>
Ancak bildiğiniz gibi MochaDB RDBMS (İlişkisel Veritabanı Yönetim Sistemi) kategorisinde yer alsa da diğerlerinden biraz farklı bir tasarıma sahip. Bu nedenle MochaDB, ekosistemi için bir veri dili olan MHQL'i barındırır. SQL ile sözdizimi biraz farklı olsa da, SQL ile çalışanların onu kolayca kavraması için yeterince kolaydır. Yeni gelenler zorluk çekmeden öğrenebilirler.<br>

MHQL genellikle SQL'den çok daha hızlıdır. Kendi içinde bir dil olmasına rağmen Tercüman yardımı ile çalıştırılır. Bu Tercüman C# ile yazılmıştır. Yani işin mantığı MochaScript'e benzer. C#'da MHQL ile çalıştığınızda, klasik, OleDb ve MSSQL nesneleriyle çalışırken kullandığınız fonksiyonlar ve sınıflar oldukça benzerdir, bu nedenle fazla yabancılık çekmeyeceksiniz.<br>

Nesneler, ``MochaDB.Mhql`` ad alanında bulunur. ``MochaDbCommand`` nesnesiyle kodda çalıştırılırlar.

#### Kod Örnekleri

```C#
var command = new MochaDbCommand("USE Persons",db);
var table = command.ExecuteScalar() as MochaTableResult;
```
Detaylar için: <a href="https://github.com/mertcandav/MochaDB/wiki/MochaDbCommand">MochaDbCommand</a>
> `` Querying`` ad alanıyla ``MochaDbCommand`` işlevlerini doğrudan ``MochaDatabase`` e ekleyebilirsiniz.

# 

### Yorumlar

Yorum belirtmek için ``/*`` ve ``*/`` karakterleri arasına yorum yazılmalıdır. ``/*`` Ve ``*/`` arasında yazılanlar birden çok satır ile kullanılabilir. Tek satır açıklamalar için ``//`` kullanılmalıdır. ``//`` kullanılan yerden satır sonuna kadar geçerlidir. Komutta açıklamaların olmadığı varsayılır.<br>
#### Örnekler
```java
USE * /* Tablodaki tüm sütunları seç. */
FROM Persons /* Hedef tablo Persons tablosu. */
```
```java
// Yazar: Mertcan Davulcu
USE *
FROM Persons
```

# 

### Veri Tipleri

#### Char
Char, programlama dillerindekinden farklı değildir.<br>
Tek tırnak içinde yazılırlar. Bkz: ``'M'``
Yalnızca tek karakter olabilirler.<br><br>
Bazı özel karakterler, kaçış dizeleriyle gösteririlir.<br>
Örneğin, diziye tek tırnak eklemek için. Bkz: ``'\''``<br>
Result: ``'``

```
|| Kaçış Karakterleri ||

\\  Backslash
\'  Tek tırnak
\"  Çift tırnak
\n  ASCII Linefeed / Yeni satır
\r  ASCII Carriage Return
\b  ASCII Backspace
\f  ASCII Formfeed
\t  ASCII Yatay Tab
\v  ASCII Dikey Tab
\a  ASCII Bell
```

#### String
Birden fazla karakter birleştirilerek string değerleri oluşturulur. Programlama dillerinden farklı değildir.<br>
Çift tırnak içinde yazılırlar. Bkz: ``"Hello, my name is Mertcan Davulcu"``

Bazı özel karakterler, kaçış dizileriyle gösterilir. Tıpkı char gibi.<br>
Örneğin, diziye çift tırnak eklemek için. Bkz: ``"My name is \"Mertcan Davulcu\""``<br>
Sonuç: ``My name is "Mertcan Davulcu"``

#### Boolean
Boole değerleri doğrudan yazılır. Bkz: ``TRUE`` ve ``FALSE``.

#### Aritmetik
Aritmetik değerler, önlerinde hashtag ile yazılmalıdır.<br>
Bkz: ``#1``, ``#5.3331``.

# 

### Anahtar Kelimeler

- ```USE``` <br>
Kullanılacak alanları almanızı sağlar. Tablonun kendisini tam olarak elde etmek için, sütunu elde etmek için tablonun adını nokta ile ayırarak belirtmek gerekir. İsim uyuşmazlıkları göz ardı edilir. Bu nedenle aynı tabloyu iki kez yazdırıp tek bir tablo olarak yazdırabilir, farklı tabloların sütunlarını tek bir tabloda birleştirebilirsiniz. Satırlar eşleşmezse, veri türünden bağımsız olarak o sütunun verilerine boş dize verileri atanır.<br>
<b>Örnekler</b><br>
```USE Persons```: Returns all 'Persons' table content. <br>
```USE Persons.Name```: Returns only 'Name' column content from 'Persons' tables. <br>
```USE Persons.ID, Persons.Name```: Returns 'ID' and 'Name' columns content from 'Persons' tables. <br>
```USE Persons, Persons```: Returns the 'Persons' table by integrating it twice in the same table.

- ```AS``` <br>
Sütunu yeniden adlandırır.<br>
<b>Örnekler</b><br>
```USE Name AS Name of person, Age AS Age of person FROM Persons```

- ```FROM``` <br>
    > Eğer ``FROM`` kullanılıyorsa, regex sorgularında dizin yerine sütun adı kullanılabilir.

    SQL'in ``FROM`` komutuna eşittir. Tablolar kullanıldığında birleştirilemez!<br>
    <b>Örnekler</b><br>
    ```USE * FROM Persons``` <br>
    ```USE Name,Age FROM Persons```

- ```SELECT``` <br>
``USE`` anahtar kelimesinin aksine, içeriği değil, içerik taşıyıcılarını almak için kullanılır. Seçimi doğrudan isimlerle değil, Regex sorgularıyla gerçekleştirir. Regex sorguları parantez içinde yazılır.<br>
<b>Örnekler</b><br>
```SELECT ()```: Returns all tables. <br>
```SELECT (P.*)```: Returns only tables if name of starts with 'P'. <br>

- ```ORDERBY``` <br>
SQL'in ``ORDER BY`` komutu gibi veri sıralama yapılacağını bildirir.<br>
Önce sütun indeksi veya adı, ardından sıra türü verilir. Türü verilmemişse, varsayılan olarak ``ASC`` ayarlanır.<br>
``USE Names, Salary FROM Persons ORDERBY Names, Salary DESC`` <br>
``USE Persons.Name, Persons.Age ORDERBY 1 ASC``

- ```CORDERBY``` <br>
Sütun sıralaması olacağını bildirir.<br>
``USE * FROM Persons CORDERBY ASC``<br>
``USE * FROM Persons CORDERBY DESC``

- ```ASC``` <br>
Yalnızca ``ORDERBY`` anahtar kelimesinin yanında kullanılabilir. Küçükten büyüğe sıralama demektir.<br>
<b>Örnekler</b><br>
```USE Persons ORDERBY 1 ASC```

- ```DESC``` <br>
Yalnızca ``ORDERBY`` anahtar kelimesinin yanında kullanılabilir. Büyükten küçüğe sıralama demektir.<br>
<b>Örnekler</b><br>
```USE Persons ORDERBY 1 DESC```

- ```MUST``` <br>
SQL'in ``WHERE`` komutu gibi. Yazım tarzı farklıdır.<br>
Sütun belirtilir ve bir normal ifade sorgusu parantez içinde yazılır.<br>
<b>Örnekler</b><br>
```USE Persons MUST 0 == "Mike"``` <br>
```USE Persons.Name, Persons.Age MUST 1(18|19|20) ORDERBY 1```

- ```AND``` <br>
Yalnızca ``MUST`` ile birlikte kullanılabilir. Başka bir koşul daha olduğunu gösterir.<br>
<b>Örnekler</b><br>
```USE Persons MUST 0(Mike) AND 3(Male|Female)``` <br>
```USE Persons.Name, Persons.Age MUST 0(M.*) AND 1(^(18|19|20)$) ORDERBY 1```

- ```IN``` <br>
Yalnızca ``MUST`` ile birlikte kullanılabilir. Alt sorgu bildirir.<br>
<b>Örnekler</b><br>
```USE Name, $Country FROM Persons MUST IN Country { USE Name FROM Countries MUST Name == "Turkey" }``` <br>

- ```INEQ``` <br>
Yalnızca ``MUST`` ile birlikte kullanılabilir. Alt sorgu bildirimi. ``IN`` ile tamamen aynıdır, ancak fazladan bir gereksinimle, döndürülen tablonun yalnızca bir satırı olmalıdır.<br>
<b>Örnekler</b><br>
```USE Name, $Country FROM Persons MUST INEQ Country { USE Name FROM Countries MUST Name == "Turkey" }``` <br>

- ```GROUPBY``` <br>
SQL'in ``GROUP BY`` komutu gibi. Yazım tarzı farklıdır.<br>
Sütun yazıldıktan sonra indeks veya isim verilir.<br>
<b>Örnekler</b><br>
```USE Name, $Salary, AVG(Salary) AS Avarage Salary FROM Persons GROUPBY Name```

- ```SUBCOL``` <br>
Tek başına yazıldığında, sütunları ilk sütundan verilen en yüksek sayıya götürür. İki değer belirtildiğinde, ilk değerden başlar ve ikinci değer kadar sütunu alır. Başlangıç ​​sütun sayısından büyükse 0 sütun döndürür.<br>
<b>Örnekler</b><br>
```USE Persons SUBCOL 100``` - İlk sütundan başlar ve ilk 100 sütunu alır. <br>
```USE Persons SUBCOL 10, 5``` - 10. sütundan başlar ve 10. sütundan sonraki 5 sütunu alır.

- ```DELCOL``` <br>
Tek başına yazıldığında, sütunları ilk satırdan verilen en yüksek sayıya kadar siler. İki değer belirtildiğinde, ilk değerden başlar ve ikinci değer kadar sütunu siler.<br>
<b>Örnekler</b><br>
```USE Persons DELCOL 100``` - İlk 100 sütunu siler ve geri kalanını döndürür. <br>
```USE Persons DELCOL 10, 5``` - 10. sütundan başlar ve 10. sütundan sonraki 5 sütunu siler.

- ```SUBROW``` <br>
Tek başına yazıldığında, satırları ilk satırdan verilen en yüksek sayıya götürür. İki değer belirtildiğinde, ilk değerden başlar ve ikinci değer kadar satırı alır. Satırların başlangıç ​​sayısından büyükse 0 satır döndürür.<br>
<b>Örnekler</b><br>
```USE Persons SUBROW 100``` - İlk satırdan başlar ve ilk 100 satırı alır. <br>
```USE Persons SUBROW 10, 5``` - 10. satırdan başlar ve 10. satırdan sonraki 5 satırı alır.

- ```DELROW``` <br>
Tek başına yazıldığında, satırları ilk satırdan verilen en yüksek sayıya kadar siler. İki değer belirtildiğinde, ilk değerden başlar ve ikinci değer kadar satırı siler.<br>
<b>Örnekler</b><br>
```USE Persons DELROW 100``` - İlk 100 satırı siler ve geri kalanını döndürür. <br>
```USE Persons DELROW 10, 5``` - 10. satırdan başlar ve 10. satırdan sonraki 5 satırı siler.

- ```ADDROW``` <br>
Belirtilen sayıda yeni satır ekler ve veri türünün varsayılan değerini ayarlar.
<b>Örnekler</b><br>
```USE Persons ADDROW 100``` - Tam tablo içeriğini alın ve varsayılan değerlerle yeni 100 satır ekleyin.

# 

### Özel

- ```$``` <br>
MUST koşulların işlevlerinde kullanılır. Ayrıca, yalnızca tablo modunda geçici bir sütun belirtmek için de kullanılır. İşaretli sütunlar sorgu boyunca mevcuttur, ancak sorgu sonucunda döndürülen tabloda bulunmaz.<br>
<b>Örnekler</b><br>
```USE Persons.Age MUST BIGGER(0,18)```<br>
```USE Name, $Salary, AVG(Salary) FROM Persons GROUPBY Name``` - Kalan sütunlan: ``Name``, ``AVG(Salary)`` <br>
```USE Persons.Name, Persons.$Salary ORDERBY 1 DESC``` - Kalan sütunlar: ``Persons.Name`` <br>
```USE $Persons``` - Klan sütunlar: Boş tablo

# 

### USE anahtar kelimesinin fonksiyonları

> Bu işlevler yalnızca SQL gibi ``GROUPBY`` ile kullanıldığında düzgün çalışır.<br>
  Bu işlevler yalnızca ``FROM`` ile kullanılabilir.

- ```COUNT()``` <br>
SQL'in ``COUNT()`` komutuna eşittir. Gruplanan veri sayısını döndürür.<br>
<b>Örnekler</b><br>
```USE Name, COUNT() FROM Persons GROUPBY Name```

- ```MAX(sütun)``` <br>
SQL'in ``MAX()`` komutuna eşittir. Gruplanmış veriler arasındaki en büyük değeri döndürür.<br>
<b>Örnekler</b><br>
```USE Name, Salary, MAX(Salary) FROM Persons GROUPBY Name```

- ```MIN(sütun)``` <br>
SQL'in ``MIN()`` komutuna eşittir. Gruplanmış veriler arasında minimum değeri döndürür.<br>
<b>Örnekler</b><br>
```USE Name, Salary, MIN(Salary) FROM Persons GROUPBY Name```

- ```AVG(sütun)``` <br>
SQL'in ``AVG()`` komutuna eşittir. Ortalama değer gruplandırılmış verileri döndürür.<br>
<b>Örnekler</b><br>
```USE Name, Salary, AVG(Salary) FROM Persons GROUPBY Name```

# 

### MUST anahtar kelimesinin fonksiyonları

- ```BETWEEN``` <br>
SQL'in ``BETWEEN`` komutuna eşittir. Yazım tarzı farklıdır.<br>
Belirtilen bir sayısal aralık koşulunu döndürür. Kullanıldığı sütunun verileri sayısal bir değer içermelidir.<br>
Yazılır: ``BETWEEN(sütun,aralık1,aralık2)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST BETWEEN(1,18,35)```

- ```BIGGER``` <br>
Belirtilen sayısal olarak daha büyük ve eşit bir koşul döndürür. Kullanıldığı sütunun verileri sayısal bir değer içermelidir.<br>
Yazılır: ``BIGGER(column,value)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST BIGGER(1,18)```

- ```LOWER``` <br>
Belirtilen bir sayısal alt ve eşit koşul döndürür. Kullanıldığı sütunun verileri sayısal bir değer içermelidir.<br>
It is written as ``LOWER(column,value)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST LOWER(1,17)```

- ```EQUAL``` <br>
"Herhangi bir değere eşit mi?" Durumu döndürür.<br>
Yazılır: ``EQUAL(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST EQUAL(1,18,19,20)```

- ```NOTEQUAL``` <br>
"Tüm değerlere eşit değil mi?" Durumu döndürür.<br>
Yazılır: ``NOTEQUAL(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST NOTEQUAL(0,Mike,Ashley)```

- ```STARTW``` <br>
"... ile mi başlıyor?" Durumu döndürür.<br>
Yazılır: ``STARTW(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST STARTW(0,M,C,A)```

- ```NOTSTARTW``` <br>
"... ile başlamıyor mu?" Durumu döndürür.<br>
Yazılır: ``NOTSTARTW(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST NOTSTARTW(0,M,C,A)```

- ```ENDW``` <br>
"... ile bitiyor mu?" Durumu döndürür.<br>
Yazılır: ``ENDW(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST ENDW(0,lia,can)```

- ```NOTENDW``` <br>
"... ile bitmiyor mu?" Durumu döndürür.<br>
Yazılır: ``NOTENDW(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST NOTENDW(0,lia,can)```

- ```CONTAINS``` <br>
"İçeriyor mu?" Durumu döndürür.<br>
Yazılır: ``CONTAINS(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST CONTAINS(0,as,is)```

- ```NOTCONTAINS``` <br>
"İçermiyor mu?" Durumu döndürür.<br>
Yazılır: ``NOTCONTAINS(column,value0,value1,...)``.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST NOTCONTAINS(0,as,is)```

# 

### Operatörler
> Sütun adları veya dizinler ve sütunlar da karşılaştırılabilir.

> İlk kalıp aynı operatörü içeremez, ancak ikinci kalıp içerebilir.<br>
  True: ``0 == "=="``<br>
  False: ``"==" == "Hello"``

#### Equal</b> ( ``==`` )<br>
Eşitliği kontrol eder. İki değer arasında yazılır.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST 1 == "18"```<br>
```USE ID, Name FROM Persons MUST ID == '1'```<br>
```USE ID, Name FROM Persons MUST ID == Name```

#### NotEqual</b> ( ``!=`` )<br>
Eşitsizliği kontrol eder. İki değer arasında yazılır.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST 1 != "18"```<br>
```USE ID, Name FROM Persons MUST ID != '1'```<br>
```USE ID, Name FROM Persons MUST ID != Name```

#### Bigger</b> ( ``>`` )<br>
"x" ten daha büyük kontrol edin. String değerlerinde kullanılamaz. İki değer arasında yazılır.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST 1 > #18```<br>
```USE ID, Name FROM Persons MUST ID > #1```

#### Lower</b> ( ``<`` )<br>
Altını ve ardından "x" i kontrol edin. String değerlerinde kullanılamaz. İki değer arasında yazılır.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST 1 < #18```<br>
```USE ID, Name FROM Persons MUST ID < #1```

#### BiggerEq</b> ( ``>=`` )<br>
"x" den büyük veya eşittir işaretleyin. String değerlerinde kullanılamaz. İki değer arasında yazılır.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST 1 >= #18```<br>
```USE ID, Name FROM Persons MUST ID >= #1```

#### LowerEq</b> ( ``<=`` )<br>
Daha düşük veya "x" e eşit olarak kontrol edin. String değerlerinde kullanılamaz. İki değer arasında yazılır.<br>
<b>Örnekler</b><br>
```USE Persons.Name,Persons.Age MUST 1 <= #18```<br>
```USE ID, Name FROM Persons MUST ID <= #1```

# 

### Alt Sorgular
Alt sorgular, aynı anda farklı tabloları sorgulamanıza ve tabloları bir bütün olarak birbirleriyle sorgulamanıza olanak tanır. Alt sorgular yuvalanabilir. Alt sorgu, sorgu sütununun veri türüyle uyumlu yalnızca bir sütun döndürmelidir. Mantık öyledir ki, eğer sorgu verileri alt sorgudan döndürülen sütunun verilerinde bulunursa, ``true`` bulunmazsa ``false`` olacaktır.
<br><br>
Maaşı 5000'den fazla olan ve Microsoft'a bağlı kişilerin isimlerini getiren sorgudur.
```java
USE *
FROM Persons
MUST IN Name {
  USE $Salary, $Company, Name
  FROM Salaries
  MUST Salary > #5000 AND IN Company {
    USE Name
    FROM Companies
    MUST
        Name == "Microsoft"
  }
}
```
<br>

Kişiler tablosundan Ad sütununu ve yalnızca 60 yaşın üzerindeki kişilerin parolalarını içeren Parola sütununu ekleyin.

> ``FROM`` anahtar kelimesi ile kullanılamaz!

```java
USE Persons.Name, {
  USE Password, $Age
  FROM Persons
  MUST BIGGER(Age, 60)
}
```

# 

### Örnek Sorgular
```java
USE *
FROM Persons
```
```java
USE Name,Gender,Age
FROM Persons
```
```java
USE *
FROM Persons
MUST
    IsAdmin == TRUE
```
```java
USE Name,$Gender,Age
FROM Persons
MUST
    2 > #18 AND
    Gender == "Female"
```
```java
USE {
  USE Name, Password
  FROM Persons MUST
    Name == "mertcandav"
}, ${
  USE Id, $Name
  FROM Idenditities MUST
    Name == "mertcandav"
} MUST 2 > #20
```
```java
USE * FROM Persons
MUST BETWEEN(3,18,45) AND 0(*.ale)
```
```java
USE Persons, Persons, Persons
ORDERBY 0
```
```java
USE Salary
FROM Persons
GROUPBY 0
```
```java
USE *
```
```java
USE *
MUST 0(NumberKey.*)
ORDERBY 0
```
```java
SELECT (A.*)
```
```java
SELECT ()
REMOVE
```
```java
SELECT ([A-Z])
REMOVE
```
