<div align="center">
  
![alt text](https://github.com/mertcandav/MochaDB/blob/master/res/MochaDB_Texted.ico)

[![Gitter](https://badges.gitter.im/mertcandv/MochaDB.svg)](https://gitter.im/mertcandv/MochaDB?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![license](https://img.shields.io/badge/License-MIT-BLUE.svg)](https://opensource.org/licenses/MIT)
[![CodeFactor](https://www.codefactor.io/repository/github/mertcandav/mochadb/badge)](https://www.codefactor.io/repository/github/mertcandav/mochadb)
[![.NET Core](https://github.com/mertcandav/MochaDB/workflows/.NET%20Core/badge.svg)](https://github.com/mertcandav/MochaDB/actions?query=workflow%3A%22.NET+Core%22)
[![Build Status](https://dev.azure.com/mertcandav/MochaDB/_apis/build/status/mertcandav.MochaDB?branchName=master)](https://dev.azure.com/mertcandav/MochaDB/_build/latest?definitionId=2&branchName=master)
[![Build Status](https://travis-ci.com/mertcandav/MochaDB.svg?branch=master)](https://travis-ci.com/mertcandav/MochaDB)
[![Terminal](https://github.com/mertcandav/MochaDB/workflows/Terminal/badge.svg)](https://github.com/mertcandav/MochaDB/actions?query=workflow%3A%22terminal%22)
<br>
[![Documentation](https://img.shields.io/badge/Documentation-YELLOW.svg?style=flat-square)](https://github.com/mertcandav/MochaDB/tree/master/docs)
[![NuGet Page](https://img.shields.io/badge/NuGet-BLUE.svg?style=flat-square)](https://www.nuget.org/packages/MochaDB/)
<br><br>
<b>MochaDB is a user-friendly, loving database system that loves to help.<br>Your best choice local database in .NET platform.</b>
</div>

## Featured features

+ Open source and free for everyone
+ High performance
+ Lightweight
+ Single DLL and database file
+ OOM(Object Oriented Management)
+ ACID support
+ RDBMS(Relational Database Management System) features
+ Supports LINQ queries
+ MochaQ for simple and fast queries
+ MHQL(MochaDB Query Language) for advanced queries

## Compatibility
<table>
  <tr>
    <td>.NET Standard</td>
    <td>.NET Core</td>
    <td>.NET Framework</td>
  </tr>
  <tr>
    <td>1.3 or higher</td>
    <td>1.1 or higher</td>
    <td>4 or higher</td>
  </tr>
</table>

## MochaDB Studio
Manage with a powerful management system! Only Windows.
[![preview](https://github.com/mertcandav/MochaDBStudio/blob/master/docs/example-gifs/preview.gif)](https://github.com/mertcandav/MochaDBStudio)

## Work with MHQL
```java
USE Id, Name, Password, CompanyId
FROM Persons MUST
Name == "mertcandav" AND
  IN CompanyId {
    USE Id, $Name FROM Companies MUST
    Name == "Microsoft"
  }
CORDERBY ASC
ORDERBY Name
SUBROW 100
```

## Example use
```csharp
MochaDatabase database = new MochaDatabase(path: "db", password: "1231", logs: false);
string username = Console.ReadLine();
string password = Console.ReadLine();
database.Connect();
MochaTableResult result = database.ExecuteScalarTable(
    $@"USE Username, Password
       FROM Persons
       MUST Username == ""{username}"" AND Password == ""{password}""");
database.Disconnect();
if(result.IsEmpty())
    Console.WriteLine("Username or password is wrong!");
else
    Console.WriteLine("Success!");
```

## Thanks for supports
+ [Koray Akpınar](https://github.com/korayakpinar)
