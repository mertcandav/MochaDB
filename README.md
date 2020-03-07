# MochaDB

.NET embedded high performance NoSQL database system

<a href="https://opensource.org/licenses/MIT">![license](https://camo.githubusercontent.com/890acbdcb87868b382af9a4b1fac507b9659d9bf/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f6c6963656e73652d4d49542d626c75652e737667)</a>
[![Build Status](https://travis-ci.com/mertcandav/MochaDB.svg?branch=master)](https://travis-ci.com/mertcandav/MochaDB)

---

## Featured features.
- Open source and free for everyone
- Serverless
- File based
- High performance
- Lightweight
- Small
- Secure
- Embedded
- RDBMS(Relational Database Management System) features
- Embed files into the database with FileSystem
- Full compatible with .NET Core, .NET Standard and .NET Framework
- Script build and debug with MochaScript
- MochaQ for simple and fast querys
- Supports LINQ querys
- Encrypted database file
- Object oriented management support
- <a href="https://github.com/mertcandav/MochaDBStudio">MochaDB Studio</a> for databases management with graphical user interface
- Single DLL
- Single database file
- Portable

<br>

## Usage areas
- Desktop applications
- Web sites/applications
- Mobile applications
- Servers
- Libraries
- Games

<br>

## Use

You cannot work with SQL queries, but with MochaQ you can do basic operations with custom commands and work with LINQ queries.

```c#
public IEnumerable<MochaTable> GetMatchTables(string key) {
  Regex rgx = new Regex(key);
  var tables = DB.GetTables();
  var result =
    from table in tables
    where rgx.IsMatch(table.Name)
    select table;
  
  return result;
}
```

<br><br>

# MochaQ
MochaQ offers quick basic queries. With MochaQuery, you can process and run queries.

<br>

## Use

```c#
MochaDatabase db = new MochaDatabase("connection string");
db.Connect();
string value = (MochaResult<string>)db.Query.GetRun("GETSECTORDATA:PiNumber");

if(value!="3.14")
    db.Query.Run("SETSECTORDATA:PiNumber:3.14");

//Or

MochaDatabase db = new MochaDatabase("connection string");
db.Connect();
MochaQuery query = new MochaQuery(db);
string value = (MochaResult<string>)query.GetRun("GETSECTORDATA:PiNumber");

if(value!="3.14")
    query.Run("SETSECTORDATA:PiNumber:3.14");
```

<br><br>

# MochaScript

MochaScript is a scripting language for MochaDB databases.<br>
It is processed by the debugger and executed with C# code.<br>
It operates independently from the database, but no connection should be open to the targeted database when it is run!
It allows you to work with its own functions and MochaQ commands.

<br>

## Use

```
//Author: Mertcan DAVULCU

//Connect.
//Provider "Path" or "Path" "Password"
Provider .\bin\Sources\MyDatabase.mochadb 1230

//Start script commands.
Begin

//Main function.
func Main()
{
    String PiNumber = ""
    SetPINumber()
    String PiNumber2 = PiNumber
    Boolean ExistsState = EXISTSSECTOR:PINumber

    if ExistsState == True
    {
        if GETSECTORDATA:PINumber == PiNumber2
        {
            DELETESECTOR:PINumber
        }
    }
    elif ExistsState == False
    {
        ADDSECTOR:PINumber:3.14:This is the pi number.
    }
    else
    {
        //Classical conditioning. "else" does not work because the elif meets "False" status.
        ADDSECTOR:PINumber:3.14:This is the pi number.
    }
}

func SetPINumber()
{
    PiNumber = "3.14"

    //Or.
    //delete PiNumber;
    //String PiNumber = "3.14"
}

// ******* COMPILER EVENTS *******

compilerevent OnFunctionInvoked()
{
    echo "Function called!"
}

//End script commands.
Final
```

<br>

## Debugging

```c#
public void Debug(string path) {
    MochaScriptDebugger debugger = new MochaScriptDebugger(path);
    debugger.DebugRun();
}
```
<br>

## And More

- <a href="https://github.com/mertcandav/MochaDB/wiki/Warnings">Warnings</a>
- <a href="https://github.com/mertcandav/MochaDB/wiki">Documentation</a>
