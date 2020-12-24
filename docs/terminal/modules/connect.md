Connect to MochaDB database. Answer the quescions. <br>
Exmaple: <br>

```
desktop connect
Name: MyDb
Password:
Logs(default if false):
MyDb
```

Use ``$`` operator for XML output. <br>
Example: <br>

```
testdb\MHQL $ USE * FROM Persons
<Table>
  <Name Description="" DataType="String">
    <Data>Mertcan</Data>
    <Data>Mertcan</Data>
    <Data>Mertcan</Data>
    <Data>Ayþe</Data>
    <Data>Enis</Data>
  </Name>
  <Password Description="" DataType="String">
    <Data>100</Data>
    <Data>900</Data>
    <Data>330</Data>
    <Data>70</Data>
    <Data>600</Data>
  </Password>
  <new Description="" DataType="Decimal">
    <Data>0</Data>
    <Data>0</Data>
    <Data>2,2</Data>
    <Data>0</Data>
    <Data>0</Data>
  </new>
</Table>
```
