<div align="center">
  
![alt text](https://github.com/mertcandav/MochaDB/blob/master/pdocs/resources/MochaDB_Texted.ico)

[![Gitter](https://badges.gitter.im/mertcandv/MochaDB.svg)](https://gitter.im/mertcandv/MochaDB?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![license](https://img.shields.io/badge/License-MIT-TEAL.svg)](https://opensource.org/licenses/MIT)
[![.NET Core](https://github.com/mertcandav/MochaDB/workflows/.NET%20Core/badge.svg)](https://github.com/mertcandav/MochaDB/actions?query=workflow%3A%22.NET+Core%22)
[![Build Status](https://dev.azure.com/mertcandav/MochaDB/_apis/build/status/mertcandav.MochaDB?branchName=master)](https://dev.azure.com/mertcandav/MochaDB/_build/latest?definitionId=2&branchName=master)
[![Build Status](https://travis-ci.com/mertcandav/MochaDB.svg?branch=master)](https://travis-ci.com/mertcandav/MochaDB)
<br>
[![Documentation](https://img.shields.io/badge/Documentation-YELLOW.svg?style=flat-square)](https://github.com/mertcandav/MochaDB/wiki)
[![NuGet Page](https://img.shields.io/badge/NuGet-BLUE.svg?style=flat-square)](https://www.nuget.org/packages/MochaDB/)
[![Warnings](https://img.shields.io/badge/Warnings-RED.svg?style=flat-square)](https://github.com/mertcandav/MochaDB/wiki/Warnings)
<br>
<b>MochaDB is a user-friendly, loving database system that loves to help.</b>
</div>

<br>

## Featured features

+ Open source and free for everyone
+ Always up to date!
+ High performance
+ Lightweight
+ Single DLL
+ Single database file
+ Small
+ Thread-safe
+ Supports LINQ and PLINQ queries
+ <a href="https://github.com/mertcandav/MochaDB/wiki/MochaQ">MochaQ</a> for simple and fast queries
+ <a href="https://github.com/mertcandav/MochaDB/wiki/MHQL">MHQL(MochaDB Query Language)</a> for advanced queries
+ RDBMS(Relational Database Management System) features
+ Restore unwanted changes with logs
+ Embed files into the database with FileSystem
+ Full compatible with .NET Core(1.1 or higher), .NET Standard(1.3 or higher) and .NET Framework(4 or higher)
+ Script build and debug with <a href="https://github.com/mertcandav/MochaDB/wiki/MohaScriptDebugger">MochaScript</a>
+ Object oriented management

<br>

# MochaDB Studio
Manage with a powerful management system! Only Windows.
[![preview](https://github.com/mertcandav/MochaDBStudio/blob/master/docs/example-gifs/preview.gif)](https://github.com/mertcandav/MochaDBStudio)

<br>

# Querying

### LINQ / PLINQ

```c#
  var tables = DB.GetTables();
  var result =
    from table in tables
    where table.Name.StartsWith("A") && table.Columns.Count > 0
    select table;
  
  return result;
```
Or
```c#
var tables = DB.GetTables(x=> x.Name.StartsWith("A") && x.Columns.Count > 0);
```

# 

### MochaQ

```c#
MochaDatabase db = new MochaDatabase("path=.\\Databases\\Math.mochadb; AutoConnect=True");
string value = db.Query.GetRun("GETSECTORDATA:PiNumber").ToString();

if(value!="3.14")
    db.Query.Run("SETSECTORDATA:PiNumber:3.14");
```

# 

### MHQL

```java
@TABLES
USE
    Id, Name, Surname, Salary
FROM
    Employees
    
MUST
    $BETWEEN(Salary,1000,10000)
    AND
        Name(^(M|m|N|n).*) AND
      Salary == "0"
        AND
          $STARTW(Surname,"M")
END

ORDERBY
    DESC Salary
RETURN

```

<br>

# MochaScript

MochaScript is a scripting language for MochaDB databases.<br>
It is processed by the debugger and executed with C# code.<br>
It operates independently from the database, but no connection should be open to the targeted database when it is run!<br>
It allows you to work with its own functions and MochaQ commands.

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

```c#
public void Debug(string path) {
    MochaScriptDebugger debugger = new MochaScriptDebugger(path);
    debugger.DebugRun();
}
```
