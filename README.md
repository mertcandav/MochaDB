# MochaDB
---
File based NoSQL database system.

It is designed as a fast and high data storage solution based on files.
It stores the data with XML infrastructure. It does not support SQL.

<br>

## Featured features.
- Open source and free for everyone
- Serverless
- File based
- Fast
- Lightweight
- Small
- Secure
- Full compatible with .NET Core, .NET Standard and NET Framework
- Script build and debug with MochaScript
- MochaQ for simple and fast querys
- Supports LINQ querys
- Encrypted database file
- Use indexes for fast searching
- MochaDB Studio for databases management with graphical user interface
- Single DLL
- Fast XML infrastructure

<br>

## Usage areas
- Desktop applications
- Web sites/applications
- Servers
- Libraries

<br>

## Use

You cannot work with SQL queries, but with MochaQ you can do basic operations with custom commands and work with LINQ queries.

```c#

public IEnumerable<MochaTable> GetMatchTables(string key) {
  IList<MochaTable> tables = DB.GetTables();
  IEnumerable<MochaTable> result =
    from table in datas
    where table==key
    select table;
  
  return result;
}

```

<br><br>

# MochaQ
MochaQ offers quick basic queries. With MochaQuery, you can process and run queries.

<br>

### Use

```c#

MochaDatabase db = new MochaDatabase("path");
string value = (string)db.Query.Run("GETSECTORDATA:PiNumber");

if(value!="3.14")
    db.Query.Run("SETSECTORDATA:PiNumber:3.14");

//Or

MochaDatabase db = new MochaDatabase("path");
MochaQuery query = new MochaQuery(db);
string value = (string)query.Run("GETSECTORDATA:PiNumber");

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

compiler_event OnFunctionInvoked()
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

# License
<a href="https://opensource.org/licenses/MIT">MIT License<a/>
