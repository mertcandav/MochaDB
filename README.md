<div align="center">
  
![alt text](https://github.com/mertcandav/MochaDB/blob/master/res/MochaDB_Texted.ico)

[![Gitter](https://badges.gitter.im/mertcandv/MochaDB.svg)](https://gitter.im/mertcandv/MochaDB?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![license](https://img.shields.io/badge/License-MIT-BLUE.svg)](https://opensource.org/licenses/MIT)
[![CodeFactor](https://www.codefactor.io/repository/github/mertcandav/mochadb/badge)](https://www.codefactor.io/repository/github/mertcandav/mochadb)
[![.NET Core](https://github.com/mertcandav/MochaDB/workflows/.NET%20Core/badge.svg)](https://github.com/mertcandav/MochaDB/actions?query=workflow%3A%22.NET+Core%22)
[![Build Status](https://dev.azure.com/mertcandav/MochaDB/_apis/build/status/mertcandav.MochaDB?branchName=master)](https://dev.azure.com/mertcandav/MochaDB/_build/latest?definitionId=2&branchName=master)
[![Build Status](https://travis-ci.com/mertcandav/MochaDB.svg?branch=master)](https://travis-ci.com/mertcandav/MochaDB)
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
+ <a href="https://github.com/mertcandav/MochaDB/blob/master/docs/querying/MochaQ.md">MochaQ</a> for simple and fast queries
+ <a href="https://github.com/mertcandav/MochaDB/blob/master/docs/querying/mhql.md">MHQL(MochaDB Query Language)</a> for advanced queries

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

# MochaDB Studio
Manage with a powerful management system! Only Windows.
[![preview](https://github.com/mertcandav/MochaDBStudio/blob/master/docs/example-gifs/preview.gif)](https://github.com/mertcandav/MochaDBStudio)

# Querying

### LINQ

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
db.Query.Run("CreateTable:Personels");
```

# 

### MHQL

```java
USE
    Name, Surname, $Salary,
    SUM(Salary) AS Total Salary
FROM
    Employees /* 
      Get columns from Employees table.
    */
    
MUST
    $BETWEEN(Salary,1000,10000)
    AND
        Name(^(M|m|N|n).*) AND
      Salary != #5000
        AND
          $STARTW(Surname,M)

ORDERBY
    Name ASC, Salary DESC
GROUPBY
    Name
  SUBROW 1000
```

## Example use
```csharp
using System;
using System.Windows.Forms;
using MochaDB;
using MochaDB.Querying;

namespace ExampleUse
{
    public partial class LoginForm:Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        MochaDatabase database = new MochaDatabase("path=.\\db; password=1231; logs= false");
        private void loginButton_Click(object sender,EventArgs e)
        {
            string username = usernameTextBox.Text.Trim(),
                   password = passwordTextBox.Text;
            if(username == string.Empty)
            {
                MessageBox.Show("Please type your username!");
                return;
            }
            if(password == string.Empty)
            {
                MessageBox.Show("Please type your password!");
                return;
            }
            database.Connect();
            MochaTable result = database.ExecuteScalarTable(
            $@"USE Username, Password
              FROM Persons
              MUST
                Username == ""{username}"" AND
                Password == ""{password}""");
            database.Disconnect();
            if(result.IsEmpty())
            {
                MessageBox.Show("Username or password is wrong!");
                return;
            }
            AppWindow wnd = new AppWindow();
            Hide();
            wnd.Show();
        }
    }
}
```
