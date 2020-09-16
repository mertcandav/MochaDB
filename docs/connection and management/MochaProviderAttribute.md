Represents a single attribute in the connection string. It is an object to use in code.

### Examples
This code creates a new attribute and sets it as "Path".<br>
Sets its value to ``.\Developers\Personels.mhdb``.
```C#
MochaProviderAttribute attribute =
    new MochaProviderAttribute("Path",".\\Developers\\Personels.mhdb");
```
