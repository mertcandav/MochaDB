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
<b>MochaDB is a user-friendly, loving database system that loves to help.</b>
</div>

## Featured Features

+ Open source and free for everyone
+ High performance
+ Lightweight
+ ACID support
+ RDBMS(Relational Database Management System) features
+ MHQ(MochaDB Query) for simple and fast queries
+ MHQL(MochaDB Query Language) for advanced queries

## Compatibility
### MochaDB
<table>
  <tr>
    <td>.NET Standard</td>
    <td>.NET Core</td>
    <td>.NET Framework</td>
  </tr>
  <tr>
    <td>1.3 or higher</td>
    <td>1.1 or higher</td>
    <td>4.0 or higher</td>
  </tr>
</table>

### MochaDB Server
<table>
  <tr>
    <td>.NET Core 3.1 or higher</td>
  </tr>
</table>

### MochaDB Terminal
<table>
  <tr>
    <td>.NET Core 3.1 or higher</td>
  </tr>
</table>

## Components
+ [MochaDB Studio](https://github.com/mertcandav/MochaDBStudio)
+ [MochaDB Terminal](https://github.com/mertcandav/MochaDB/tree/master/terminal)
+ [MochaDB Server](https://github.com/mertcandav/MochaDB/tree/master/server)

## Work with MHQL
Perform deep queries with MHQL and avoid complex tasks manually.
```java
USE Name AS Username,
    $CompanyId,
    COUNT() AS Count of users
FROM Persons MUST
Name = "mertcandav" AND
  IN CompanyId {
    USE Id, $Name FROM Companies MUST
    Name = "Microsoft"
  }
CORDERBY ASC
ORDERBY Name
GROUPBY Name
SUBROW 100
```

## Example Use
```csharp
using MochaDB;
using MochaDB.Mhql;

// Create your database connection.
var database = new MochaDatabase(path: "db", password: "1231", logs: false);
var username = Console.ReadLine(); // Get username from user.
var password = Console.ReadLine(); // Get password from user.
database.Connect(); // Connect to database.

// Get table filtered by username and password with using mhql query.
var result = new MochaDbCommand(database).ExecuteScalar(
    $@"USE Username, Password
       FROM Persons
       MUST Username = ""{username}"" AND
       Password = ""{password}""");

database.Disconnect(); // Disconnect from database.
if (!result.Any()) { // If table is empty.
    Console.WriteLine("Username or password is wrong!");
} else {
    Console.WriteLine("Success!");
}
```

## Thanks for Supports
+ [Koray Akpınar](https://github.com/korayakpinar)
