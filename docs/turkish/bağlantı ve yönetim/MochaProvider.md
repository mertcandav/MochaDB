MochaProvider, baðlantýlarda kullanýlan baðlantý dizelerini iþleyen bir yapýdýr. Kodda kullanýlacak bir nesnedir.

### Örnekler
Bu kod, dosya yolunu ve veritabaný parolasýný belirten bir baðlantý dizesine sahip yeni bir saðlayýcý oluþturur.
```C#
MochaProvider provider =
    new MochaProvider("Path=.\\Databases\\Personels.mhdb; Password=MyDb045089");
```

Bu kod, belirtilen baðlantý dizesinde belirtilen özniteliði döndürür.
```C#
MochaProviderAttribute password =
    MochaProvider.GetAttribute("Password",provider.ConnectionString);
```
