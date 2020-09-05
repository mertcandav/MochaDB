MochaProvider is a structure that processes connection strings, used in connections. It is an object to use in code.

### Examples
This code creates a new provider with a connection string that specifies the file path and database password.
```C#
MochaProvider provider =
    new MochaProvider("Path=.\\Databases\\Personels.mhdb; Password=MyDb045089");
```

<br>

This code returns the attribute specified on the specified connection string.
```C#
MochaProviderAttribute password =
    MochaProvider.GetAttribute("Password",provider.ConnectionString);
```