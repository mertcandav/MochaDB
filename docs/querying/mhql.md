MHQL(<b>M</b>oc<b>h</b>adb <b>Q</b>uery <b>L</b>anguage), It is a data query language like SQL.<br>
However, as you know, although MochaDB is included in the RDBMS (Relational Database Management System) category, it has a slightly different design than the others. Therefore, MochaDB hosts MHQL, a data language for its ecosystem. Although it is somewhat different in syntax with SQL, it's easy enough for those who have worked with SQL to easily grasp it. Newcomers can learn without difficulty.<br>

MHQL is generally much faster than SQL. Although it is a language in itself, it is operated with the help of Interpreter. This Interpreter is written in C#. So the logic of work is similar to MochaScript. When you work with MHQL in C#, the functions and classes you use when working with classic, OleDb and MSSQL objects are quite similar, so you will not suffer much foreignness.<br>

Objects are located in the ```MochaDB.Mhql``` namespace. They are run in the code with the ``MochaDbCommand`` object.

#### Code Examples

```C#
var command = new MochaDbCommand("USE Persons",db);
var table = command.ExecuteScalar() as MochaTableResult;
```
For detail: <a href="https://github.com/mertcandav/MochaDB/wiki/MochaDbCommand">MochaDbCommand</a>
> With the ``Querying`` namespace, you can add ``MochaDbCommand`` functions directly to ``MochaDatabase``.

# 

### Comments

To indicate a comment, comments must be written between the characters ``/*`` and ``*/``. Written between ``/*`` and ``*/``  can be used with multiple lines. ``//`` should be used for single line comments. It is valid from the place where ``//`` is used until the end of the line. Comments are assumed not to exist in the command.<br>
#### Examples
```java
USE * /* Select all columns from table. */
FROM Persons /*From Persons table.*/
```
```java
// Author: Mertcan Davulcu
USE *
FROM Persons
```

# 

### Data types

#### Char
Char is It does not differ from programming languages.<br>
They are written in single quotes. As: ``'M'``
They can only be single characters.<br><br>
Some special characters are indicated by escape sequences.<br>
For example, to add single quote into the array. As: ``'\''``<br>
Result: ``'``

```
|| Escape Sequences ||

\\  Backslash
\'  Single quote
\"  Double quote
\n  ASCII Linefeed / New Line
\r  ASCII Carriage Return
\b  ASCII Backspace
\f  ASCII Formfeed
\t  ASCII Horizontal Tab
\v  ASCII Vertical Tab
\a  ASCII Bell
```

#### String
String values are formed by combining more than one char. It does not differ from programming languages.<br>
They are written in double quotes. As: ``"Hello, my name is Mertcan Davulcu"``

Some special characters are indicated by escape sequences. Just like chars.<br>
For example, to add double quotes into the array. As: ``"My name is \"Mertcan Davulcu\""``<br>
Result: ``My name is "Mertcan Davulcu"``

#### Boolean
Boolean values are written directly. See: ``TRUE`` and ``FALSE``.

#### Arithmetic
Arithmetic values must be written with hashtags before them.<br>
See: ``#1``, ``#5.3331``.

# 

### Keywords

- ```USE``` <br>
Allows you to get the areas to be used. To get the table itself completely, it is necessary to specify the name of the table by separating it with a period to get the column. Name conflicts are ignored. For this reason, you can print the same table twice and print it out as a single table, and combine the columns of the different tables in a single table. If the rows do not match, empty string data is assigned to the data of that column regardless of the data type. USE queries are TABLE mode.<br>
<b>Examples</b><br>
```USE Persons```: Returns all 'Persons' table content. <br>
```USE Persons.Name```: Returns only 'Name' column content from 'Persons' tables. <br>
```USE Persons.ID, Persons.Name```: Returns 'ID' and 'Name' columns content from 'Persons' tables. <br>
```USE Persons, Persons```: Returns the 'Persons' table by integrating it twice in the same table.

- ```AS``` <br>
Equal to ``AS`` command of SQL.<br>
<b>Examples</b><br>
```USE Name AS Name of persons, Age AS Age of persons FROM Persons```

- ```FROM``` <br>
    > If ``FROM`` is used, column name can be used instead of index in regex queries.

    Equal to ``FROM`` command of SQL. Tables cannot be combined when used!<br>
    <b>Examples</b><br>
    ```USE * FROM Persons``` <br>
    ```USE Name,Age FROM Persons```

- ```SELECT``` <br>
Unlike the ``USE`` keyword, it is used to get content carriers, not content. It realizes the selection not with direct names but with Regex queries. Regex queries are written in parentheses.<br>
<b>Examples</b><br>
```SELECT ()```: Returns all tables. <br>
```SELECT (P.*)```: Returns only tables if name of starts with 'P'. <br>

- ```ORDERBY``` <br>
Reports that there will be data sorting, like ``ORDERBY`` command of SQL.<br>
First the column index or name is given, then the order type. If the order type is not given, ``ASC`` is set by default.<br>
``USE Names, Salary FROM Persons ORDERBY Names, Salary DESC`` <br>
``USE Persons.Name, Persons.Age ORDERBY 1 ASC``

- ```ASC``` <br>
It can only be used next to the ``ORDERBY`` keyword. It means increasing order from small to large.<br>
<b>Examples</b><br>
```USE Persons ORDERBY 1 ASC```

- ```DESC``` <br>
Can only be used next to the keyword ``ORDERBY``. It means the descending order from large to small.<br>
<b>Examples</b><br>
```USE Persons ORDERBY 1 DESC```

- ```MUST``` <br>
Like ``WHERE`` command of SQL. The style of writing is different.
<br>The column is specified and a regex query is written in parentheses.<br>
<b>Examples</b><br>
```USE Persons MUST 0 == "Mike"``` <br>
```USE Persons.Name, Persons.Age MUST 1(18|19|20) ORDERBY 1```

- ```AND``` <br>
It can only be used alongside ``MUST``. Indicates that there is another condition.<br>
<b>Examples</b><br>
```USE Persons MUST 0(Mike) AND 3(Male|Female)``` <br>
```USE Persons.Name, Persons.Age MUST 0(M.*) AND 1(^(18|19|20)$) ORDERBY 1```

- ```IN``` <br>
It can only be used alongside ``MUST``. Subquery declare.<br>
<b>Examples</b><br>
```USE Name, $Country FROM Persons MUST IN Country { USE Name FROM Countries MUST Name == "Turkey" }``` <br>

- ```INEQ``` <br>
It can only be used alongside ``MUST``. Subquery declare. It is exactly the same as ``IN`` but with one extra requirement, the returned table must have only one row.<br>
<b>Examples</b><br>
```USE Name, $Country FROM Persons MUST INEQ Country { USE Name FROM Countries MUST Name == "Turkey" }``` <br>

- ```GROUPBY``` <br>
Like ``GROUPBY`` command of SQL. The style of writing is different.<br>
After writing column index or name is given.<br>
<b>Examples</b><br>
```USE Name, $Salary, AVG(Salary) AS Avarage Salary FROM Persons GROUPBY Name```

- ```SUBCOL``` <br>
When written alone, it takes the columns from the first column to the highest number given. When two values ​​are specified, it starts from the first value and takes the column as many as the second value. Returns 0 column if greater than the starting number of columns.<br>
<b>Examples</b><br>
```USE Persons SUBCOL 100``` - It starts from the first column and takes the first 100 columns. <br>
```USE Persons SUBCOL 10, 5``` - It starts at column 10 and takes the next 5 column after column 10.

- ```DELCOL``` <br>
When written alone, it deletes the columns from the first line to the highest number given. When two values ​​are specified, it starts from the first value and deletes the column as many as the second value.<br>
<b>Examples</b><br>
```USE Persons DELCOL 100``` - Deletes the first 100 column and returns the rest. <br>
```USE Persons DELCOL 10, 5``` - It starts at column 10 and deleted the next 5 column after column 10.

- ```SUBROW``` <br>
When written alone, it takes the rows from the first line to the highest number given. When two values ​​are specified, it starts from the first value and takes the row as many as the second value. Returns 0 rows if greater than the starting number of rows.<br>
<b>Examples</b><br>
```USE Persons SUBROW 100``` - It starts from the first line and takes the first 100 lines. <br>
```USE Persons SUBROW 10, 5``` - It starts at line 10 and takes the next 5 lines after line 10.

- ```DELROW``` <br>
When written alone, it deletes the rows from the first line to the highest number given. When two values ​​are specified, it starts from the first value and deletes the row as many as the second value.<br>
<b>Examples</b><br>
```USE Persons DELROW 100``` - Deletes the first 100 lines and returns the rest. <br>
```USE Persons DELROW 10, 5``` - It starts at line 10 and deleted the next 5 lines after line 10.

- ```ADDROW``` <br>
Adds a specified number of new lines and sets the default value of the data type.
<b>Examples</b><br>
```USE Persons ADDROW 100``` - Get full table content and add new 100 row with default values.

# 

### Special

- ```$``` <br>
It is used in the functions of MUST conditions. It is also used to specify a temporary column only in TABLE mode. The marked columns are available throughout the query but are not found in the returned table as a result of the query.<br>
<b>Examples</b><br>
```USE Persons.Age MUST BIGGER(0,18)```<br>
```USE Name, $Salary, AVG(Salary) FROM Persons GROUPBY Name``` - Result columns: ``Name``, ``AVG(Salary)`` <br>
```USE Persons.Name, Persons.$Salary ORDERBY 1 DESC``` - Result columns: ``Persons.Name``

# 

### Functions of USE

> These functions only work properly when used with ``GROUPBY``, such as SQL.<br>
  These functions can only be used with ``FROM``.

- ```COUNT()``` <br>
Equal to ``COUNT()`` command of SQL. Returns the number of data grouped.<br>
<b>Examples</b><br>
```USE Name, COUNT() FROM Persons GROUPBY Name```

- ```MAX(column)``` <br>
Equal to ``MAX()`` command of SQL. Returns the greatest value among grouped data.<br>
<b>Examples</b><br>
```USE Name, Salary, MAX(Salary) FROM Persons GROUPBY Name```

- ```MIN(column)``` <br>
Equal to ``MIN()`` command of SQL. Returns the minimum value among grouped data.<br>
<b>Examples</b><br>
```USE Name, Salary, MIN(Salary) FROM Persons GROUPBY Name```

- ```AVG(column)``` <br>
Equal to ``AVG()`` command of SQL. Returns the average value grouped data.<br>
<b>Examples</b><br>
```USE Name, Salary, AVG(Salary) FROM Persons GROUPBY Name```

# 

### Functions of MUST

- ```BETWEEN``` <br>
Equal to ``BETWEEN`` command of SQL. The style of writing is different.<br>
Returns a specified numerical range condition. The data of the column in which it is used must contain a numerical value.<br>It is written as ``BETWEEN(column,range1,range2)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST BETWEEN(1,18,35)```

- ```BIGGER``` <br>
Returns a specified numerical bigger and equal condition. The data of the column in which it is used must contain a numerical value.<br>
It is written as ``BIGGER(column,value)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST BIGGER(1,18)```

- ```LOWER``` <br>
Returns a specified numerical lower and equal condition. The data of the column in which it is used must contain a numerical value.<br>
It is written as ``LOWER(column,value)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST LOWER(1,17)```

- ```EQUAL``` <br>
"Is it equal any value?" Returns the condition.<br>
It is written as ``EQUAL(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST EQUAL(1,18,19,20)```

- ```NOTEQUAL``` <br>
"Is it not equal all values?" Returns the condition.<br>
It is written as ``NOTEQUAL(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST NOTEQUAL(0,Mike,Ashley)```

- ```STARTW``` <br>
"Does it start with ...?" Returns the condition.<br>
It is written as ``STARTW(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST STARTW(0,M,C,A)```

- ```NOTSTARTW``` <br>
"Does it not start with ...?" Returns the condition.<br>
It is written as ``NOTSTARTW(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST NOTSTARTW(0,M,C,A)```

- ```ENDW``` <br>
"Does it end with ...?" Returns the condition.<br>
It is written as ``ENDW(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST ENDW(0,lia,can)```

- ```NOTENDW``` <br>
"Does it not end with ...?" Returns the condition.<br>
It is written as ``NOTENDW(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST NOTENDW(0,lia,can)```

- ```CONTAINS``` <br>
"Does it include?" Returns the condition.<br>
It is written as ``CONTAINS(column,value)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST CONTAINS(0,il)```

- ```NOTCONTAINS``` <br>
"Doesn't it include?" Returns the condition.<br>
It is written as ``NOTCONTAINS(column,value0,value1,...)``.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST NOTCONTAINS(0,as,is)```

# 

### Operators
> Column names or indexes, and columns can also be compared.

> The first pattern cannot contain the same operator, but the second pattern can contain.<br>
  True: ``0 == "=="``<br>
  False: ``"==" == "Hello"``

#### Equal</b> ( ``==`` )<br>
It controls equality. It is written between two values.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST 1 == "18"```<br>
```USE ID, Name FROM Persons MUST ID == '1'```<br>
```USE ID, Name FROM Persons MUST ID == Name```

#### NotEqual</b> ( ``!=`` )<br>
Checks for inequality. It is written between two values.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST 1 != "18"```<br>
```USE ID, Name FROM Persons MUST ID != '1'```<br>
```USE ID, Name FROM Persons MUST ID != Name```

#### Bigger</b> ( ``>`` )<br>
Check bigger then "x". Cannot be used on string values. It is written between two values.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST 1 > #18```<br>
```USE ID, Name FROM Persons MUST ID > #1```<br>
```USE ID, Name FROM Persons MUST ID > 1```

#### Lower</b> ( ``<`` )<br>
Check lower then "x". Cannot be used on string values. It is written between two values.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST 1 < #18```<br>
```USE ID, Name FROM Persons MUST ID < #1```<br>
```USE ID, Name FROM Persons MUST ID < 1```

#### BiggerEq</b> ( ``>=`` )<br>
Check bigger then or equal to "x". Cannot be used on string values. It is written between two values.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST 1 >= #18```<br>
```USE ID, Name FROM Persons MUST ID >= #1```<br>
```USE ID, Name FROM Persons MUST ID >= 1```

#### LowerEq</b> ( ``<=`` )<br>
Check lower then or equal to "x". Cannot be used on string values. It is written between two values.<br>
<b>Examples</b><br>
```USE Persons.Name,Persons.Age MUST 1 <= #18```<br>
```USE ID, Name FROM Persons MUST ID <= #1```<br>
```USE ID, Name FROM Persons MUST ID <= 1```

# 

### Subqueries
Subqueries allow you to query different tables at the same time and query the tables as a whole with each other. Subqueries can be nested. The subquery should return only one column compatible with the data type of the query column. Logic is such that if the query data is found in the data of the column returned from the subquery, it will be ``false`` if ``true`` is not found.
<br><br>
It is a query that brings the names of people whose salary is higher than 5000 and who are affiliated with Microsoft. 
```java
USE *
FROM Persons
MUST IN Name {
  USE $Salary, $Company, Name
  FROM Salaries
  MUST Salary > #5000 AND IN Company {
    USE Name
    FROM Companies
    MUST
        Name == "Microsoft"
  }
}
```

# 

### Example codes
```java
USE *
FROM Persons
```
```java
USE Name,Gender,Age
FROM Persons
```
```java
USE *
FROM Persons
MUST
    IsAdmin == TRUE
```
```java
USE Name,$Gender,Age
FROM Persons
MUST
    2 > #18 AND
    Gender == "Female"
```
```java
USE * FROM Persons
MUST BETWEEN(3,18,45) AND 0(*.ale)
```
```java
USE Persons, Persons, Persons
ORDERBY 0
```
```java
USE Salary
FROM Persons
GROUPBY 0
```
```java
USE *
```
```java
USE *
MUST 0(NumberKey.*)
ORDERBY 0
```
```java
SELECT (A.*)
```
```java
SELECT ()
REMOVE
```
```java
SELECT ([A-Z])
REMOVE
```
