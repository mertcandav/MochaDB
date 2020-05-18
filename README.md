<div align="center">
  
![alt text](https://github.com/mertcandav/MochaDB/blob/master/pdocs/resources/MochaDB_Texted.ico)

[![Gitter](https://badges.gitter.im/mertcandv/MochaDB.svg)](https://gitter.im/mertcandv/MochaDB?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![license](https://camo.githubusercontent.com/890acbdcb87868b382af9a4b1fac507b9659d9bf/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f6c6963656e73652d4d49542d626c75652e737667)](https://opensource.org/licenses/MIT)
[![.NET Core](https://github.com/mertcandav/MochaDB/workflows/.NET%20Core/badge.svg)](https://github.com/mertcandav/MochaDB/actions?query=workflow%3A%22.NET+Core%22)
[![Build Status](https://dev.azure.com/mertcandav/MochaDB/_apis/build/status/mertcandav.MochaDB?branchName=master)](https://dev.azure.com/mertcandav/MochaDB/_build/latest?definitionId=2&branchName=master)
[![Build Status](https://travis-ci.com/mertcandav/MochaDB.svg?branch=master)](https://travis-ci.com/mertcandav/MochaDB)

<b>MochaDB is a user-friendly, loving database system that loves to help.<br>
Come on, meet the world's most advanced .NET database system!</b>
</div>

<br>

## Featured features
- Open source and free for everyone
- Always up to date!
- Serverless
- File based
- High performance
- Lightweight
- Small
- Secure
- Embedded
- RDBMS(Relational Database Management System) features
- Restore unwanted changes with logs
- Embed files into the database with FileSystem
- Full compatible with .NET Core, .NET Standard and .NET Framework
- Script build and debug with MochaScript
- MochaQ for simple and fast querys
- Supports LINQ querys
- Encrypted database file
- Object oriented management support
- [MochaDB Studio](https://github.com/mertcandav/MochaDBStudio) for databases management with graphical user interface
- Single DLL
- Single database file
- Portable

<br>

## Usage areas
- Remotely accessible database with a provided server
- Desktop applications
- Web sites/applications
- Mobile applications
- Data storages of servers
- Libraries
- Games

<br>

# MochaDB Studio
Manage with a powerful management system!
[![preview](https://github.com/mertcandav/MochaDBStudio/blob/master/docs/example-gifs/preview.gif)](https://github.com/mertcandav/MochaDBStudio)

<br>

# Preview

<br>

## Querying

You cannot work with SQL queries, but with MochaQ you can do basic operations with custom commands and work with LINQ queries.

```c#
  var tables = DB.GetTables();
  var result =
    from table in tables
    where table.Name.StartsWith("A") && table.Columns.Count > 0
    select table;
  
  return result;
```

<br>

Of course, when you pull data from the database, you can also query it directly. For this, MochaDB strives to help you, as always!

```c#
var tables = DB.GetTables(x=> x.Name.StartsWith("A") && x.Columns.Count > 0);
```

<br><br>

## MochaQ
MochaQ offers quick basic queries. With MochaQuery, you can process and run queries.

### Use

```c#
MochaDatabase db = new MochaDatabase("path=.\\Databases\\Math.mochadb; AutoConnect=True");
string value = db.Query.GetRun("GETSECTORDATA:PiNumber").ToString();

if(value!="3.14")
    db.Query.Run("SETSECTORDATA:PiNumber:3.14");

//Or

MochaDatabase db = new MochaDatabase("path=.\\Databases\\Math.mochadb; AutoConnect=True");
MochaQuery query = new MochaQuery(db);
string value = query.GetRun("GETSECTORDATA:PiNumber").ToString();

if(value!="3.14")
    query.Run("SETSECTORDATA:PiNumber:3.14");
```

<br><br>

## MochaScript

MochaScript is a scripting language for MochaDB databases.<br>
It is processed by the debugger and executed with C# code.<br>
It operates independently from the database, but no connection should be open to the targeted database when it is run!<br>
It allows you to work with its own functions and MochaQ commands.

### Use

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

### Debugging

```c#
public void Debug(string path) {
    MochaScriptDebugger debugger = new MochaScriptDebugger(path);
    debugger.DebugRun();
}
```
<br>

#

[Warnings](https://github.com/mertcandav/MochaDB/wiki/Warnings)<br>
[Documentation](https://github.com/mertcandav/MochaDB/wiki)<br>
[NuGet Page](https://www.nuget.org/packages/MochaDB/)
