Connection strings are used for connections. They are processed by ``MochaProvider``.  It is an object to use in code.

### Examples
```C#
"Path = .\\Databases\\Personels.mhdb"
"Path = .\\Databases\\Personels; Password=1234"
"Password= 1234;Path =.\\Databases\\Personels.mhdb; AutoConnect= True"
```

### Attributes
Attributes are features within the connection string.<br>
They are the main elements that allow the connection string to be created, and they all have their own unique functions.<br>
An attribute can be defined at most once in the connection string. A connection string must have defined the "Path" attribute.<br>
If an attribute is not defined or incorrectly defined, its default value is assigned.<br>
If more than one attribute is defined, the attributes are ``;`` should be separated by.
<table border="1">
    <tr>
        <td width="15%"><strong>Name</strong></td>
        <td width="75%"><strong>Attribute</strong></td>
        <td width="20%"><strong>Value Type</strong></td>
    </tr>
    <tr>
        <td>Path</td>
        <td>Specifies the path of the database.</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Password</td>
        <td>Specifies the password of the database.</td>
        <td>String</td>
    </tr>
    <tr>
        <td>AutoConnect</td>
        <td">If true, it is automatically connected to the database.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>Readonly</td>
        <td">If true, cannot task of write in database.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>AutoCreate</td>
        <td">If True, a new database will be created if there is no database every time a connection is opened.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>Logs</td>
        <td">If true, a copy of the database is kept in database whenever the content changes.</td>
        <td>True/False</td>
    </tr>
</table>

### Functions
Functions are simple and easy-to-use alternatives that you can use to make your job easier.<br>

- ```>SOURCEDIR<subcount``` <br>
You can only use the this function in the ``Path`` attribute. Returns the location where the script file is accessed. The subcount parameter, on the other hand, is the number that indicates how many times to go to a higher directory. If you want to get the location directly accessed, you can write ``0``. If it is not specified at all, the function is not recognized and may affect ``Path`` attribute's content as it may cause an error.
<br><b>Examples</b><br>
```>SOURCEDIR<0```: Function is returns ```C:\Users\user\Desktop``` <br>
```Path= >SOURCEDIR<3\testdocs\testdb.mhdb``` -- Returns: ```C:\``` <br>
```Path= >SOURCEDIR<0\testdocs\testdb.mhdb``` -- Returns: ```C:\Users\user\Desktop``` <br>
```Path= >SOURCEDIR<1\testdocs\testdb.mhdb``` -- Returns: ```C:\Users\user``` <br>
```Path= >SOURCEDIR<\testdocs\testdb.mhdb``` -- Output path: ```>SOURCEDIR<\testdocs\testdb.mhdb```
