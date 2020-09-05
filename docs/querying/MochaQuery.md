``MochaQuery`` Allows you to process MochaQ queries.<br>
The database must be set as the base and the ``MochaDatabase`` must be connected to a database.

### Examples
Creates a new ``MochaQuery`` and creates a ``MochaDatabase`` and adds a reference.
```C#
MochaQuery query = new MochaQuery(new MochaDatabase("path=.\\mydb.mhdb"));
```
<br>

<strong>WARNING</strong><br>
It can be dangerous when it is not necessary to create it normally.<br>
It requires you to have the ``MochaDatabase`` object, but remember, the ``Query`` property of the ``MochaDatabase`` object already returns a ``MochaQuery`` object that is mapped to it.

<br>

### Favorite Functions
<table border="1">
    <tr>
        <td><strong>Run()</strong></td>
        <td width="100%">Runs the active MochaQ query. Even if there is an incoming value, it will not return. For "Run" MochaQ commands.</td>
    </tr>
    <tr>
        <td><strong>GetRun()</strong></td>
        <td width="100%">Runs the active MochaQ query. Returns the incoming value. For "GetRun" MochaQ commands.</td>
    </tr>
    <tr>
        <td><strong>Dynamic()</strong></td>
        <td width="100%">If the value is returned, it returns the function and performs the function; if not, it just performs the function. For "Dynamic" MochaQ commands.</td>
    </tr>
</table>
