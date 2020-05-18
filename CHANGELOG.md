CHANGELOGS of MochaDB.

# rlsv 3.4.5 [ 18 May 2020 ]

+ Performance improvements.
+ Optimizations.
+ MochaDatabase ConnectionState property rename to State.
+ The obligation to write the tags one after the other was removed on MHQL.
+ Fix column Attributes null bug.
+ Fix sector Attributes null bug.
+ Fix stack Attributes null bug.
+ Fix mhql and keyword bug.
+ Fix not saving the deleted row.
+ Fix the error of adding rows as many as the number of columns when adding rows.
+ Fix MHQL AS keyword column name bug.
+ Fix MHQL column name bug with FROM keyword.
+ Fix value type check system wrong check bug.
+ Fix the indistinguishable from AND or keyword AND, which has condition content in MHQL code.
+ Fix the indistinguishable from MUST or keyword MUST, which has condition content in MHQL code.
+ Fix row remove bug.
+ Fix attribute engine and improvements.
+ Fix MHQL multiline comment pattern.
+ Fix the gaps to the error at the start of the MHQL.
+ Add singleline comments to MHQL.
+ Add END keyword to MHQL.
+ Add ClearRows function to MochaDatabase.
+ Add ClearRows:TableName command to MochaQ.
+ Add ExecuteScalarTable functions to MochaDbCommand.
+ Add ExecuteScalarTable extension function to MochaDatabase with Querying namespace.
+ Add all data types support to MHQL EQUAL function.
+ Add all data types support to MHQL NOTEQUAL function.
+ Add NOTEQUAL function to MHQL.
+ Add IsEmpty function to MochaTable.
+ Add IsEmpty function to MochaTableResult.
+ Add UpdateFirstData function to MochaDatabase.
+ Add UpdateLastData function to MochaDatabase.
+ Add ``*`` pattern to only USE keyword use type.
+ Add casting support for string, char, int, long, short, uint, ulong, ushort, byte, sbyte, float, decimal, double, bool and DateTime to MochaData.
+ Add ToMochaData function to MochaConvert.
+ Add ToMochaData extension function to string, char, int, long, short, uint, ulong, ushort, byte, sbyte, float, decimal, double, bool and DateTime.
+ Add object params support constructor to MochaRow.
+ Add object params support constructor to MochaRowCollection.
+ Add object params support to AddRow fucntion of MochaDatabase.
+ Add Parse static function to MochaData.
+ Add TryParse static function to MochaData.

# rlsv 3.4.4 [ 11 April 2020 ]

+ Performance optimizations
+ Fix Mhql RETURN keyword fix "'RETURN' command is cannot processed!" error with used SELECT keyword
+ Fix MochaQFormatter GetRun keywords
+ [MochaDatabase]Normalized return functions.
+ Add Attribute support for MochaDatabase extensions of Querying namespace.
+ Add Element support for MochaDatabase extensions of Querying namespace.
+ Add [Feature]Attributes
+ Add [Class]MochaAttributeCollection
+ Add [IMochaTable:Propertie]Attributes
+ Add [MochaTable:Propertie]Attributes
+ Add [IMochaColumn:Propertie]Attributes
+ Add [MochaColumn:Propertie]Attributes
+ Add [IMochaSecor:Propertie]Attributes
+ Add [MochaSector:Propertie]Attributes
+ Add [IMochaStack:Propertie]Attributes
+ Add [MochaStack:Propertie]Attributes
+ Add [IMochaStackItem:Propertie]Attributes
+ Add [MochaStackItem:Propertie]Attributes
+ Add [[Namespace]Qerying:Extension Method -> MochaDatabase]ExecuteCommand
+ Add [IMochaDatabase:Method]AddTableAttribute
+ Add [IMochaDatabase:Returnable Method]GetTableAttribute
+ Add [IMochaDatabase:Returnable Method]RemoveTableAttribute
+ Add [IMochaDatabase:Method]AddColumnAttribute
+ Add [IMochaDatabase:Returnable Method]GetColumnAttribute
+ Add [IMochaDatabase:Returnable Method]RemoveColumnAttribute
+ Add [IMochaDatabase:Method]AddStackAttribute
+ Add [IMochaDatabase:Returnable Method]GetStackAttribute
+ Add [IMochaDatabase:Returnable Method]RemoveStackAttribute
+ Add [IMochaDatabase:Method]AddStackItemAttribute
+ Add [IMochaDatabase:Returnable Method]GetStackItemAttribute
+ Add [IMochaDatabase:Returnable Method]RemoveStackItemAttribute
+ Add [IMochaDatabase:Method]AddSectorAttribute
+ Add [IMochaDatabase:Returnable Method]GetSectorAttribute
+ Add [IMochaDatabase:Returnable Method]RemoveSectorAttribute
+ Add [MochaDatabase:Method]AddTableAttribute
+ Add [MochaDatabase:Returnable Method]GetTableAttribute
+ Add [MochaDatabase:Returnable Method]RemoveTableAttribute
+ Add [MochaDatabase:Method]AddColumnAttribute
+ Add [MochaDatabase:Returnable Method]GetColumnAttribute
+ Add [MochaDatabase:Returnable Method]RemoveColumnAttribute
+ Add [MochaDatabase:Method]AddStackAttribute
+ Add [MochaDatabase:Returnable Method]GetStackAttribute
+ Add [MochaDatabase:Returnable Method]RemoveStackAttribute
+ Add [MochaDatabase:Method]AddStackItemAttribute
+ Add [MochaDatabase:Returnable Method]GetStackItemAttribute
+ Add [MochaDatabase:Returnable Method]RemoveStackItemAttribute
+ Add [MochaDatabase:Method]AddSectorAttribute
+ Add [MochaDatabase:Returnable Method]GetSectorAttribute
+ Add [MochaDatabase:Returnable Method]RemoveSectorAttribute
+ Add [MochaDatabase:Returnable Method]GetSectorAttributes
+ Add [MochaDatabase:Returnable Method]GetStackAttributes
+ Add [MochaDatabase:Returnable Method]GetStackItemAttributes
+ Add [MochaDatabase:Returnable Method]GetTableAttributes
+ Add [MochaDatabase:Returnable Method]GetColumnAttributes
+ Add [MochaQ:GetRun Command]GETTABLEATTRIBUTES:TableName
+ Add [MochaQ:GetRun Command]GETSECTORATTRIBUTES:SectorName
+ Add [MochaQ:GetRun Command]GETSTACKATTRIBUTES:StackName
+ Add [MochaQ:GetRun Command]GETCOLUMNATTRIBUTES:TableName:ColumnName
+ Add [MochaQ:GetRun Command]GETSTACKITEMATTRIBUTES:StackName:Path
+ Add [MochaQ:Run Command]REMOVETABLEATTRIBUTE:TableName:AttributeName
+ Add [MochaQ:Run Command]REMOVESECTORATTRIBUTE:SectorName:AttributeName
+ Add [MochaQ:Run Command]REMOVESTACKATTRIBUTE:StackName:AttributeName
+ Add [MochaQ:Run Command]REMOVESTACKITEMATTRIBUTE:StackName:Path:AttributeName
+ Add [MochaQ:Run Command]REMOVECOLUMNATTRIBUTE:TableName:ColumnName:AttributeName
+ Add [MochaQ:GetRun Command]GETTABLEATTRIBUTE:TableName:AttributeName
+ Add [MochaQ:GetRun Command]GETSECTORATTRIBUTE:SectorName:AttributeName
+ Add [MochaQ:GetRun Command]GETSTACKATTRIBUTE:StackName:AttributeName
+ Add [MochaQ:GetRun Command]GETSTACKITEMATTRIBUTE:StackName:Path:AttributeName
+ Add [MochaQ:GetRun Command]GETCOLUMNATTRIBUTE:TableName:ColumnName:AttributeName
+ Add [MochaQ:GetRun Command]#REMOVETABLEATTRIBUTE:TableName:AttributeName
+ Add [MochaQ:GetRun Command]#REMOVESECTORATTRIBUTE:SectorName:AttributeName
+ Add [MochaQ:GetRun Command]#REMOVESTACKATTRIBUTE:StackName:AttributeName
+ Add [MochaQ:GetRun Command]#REMOVESTACKITEMATTRIBUTE:StackName:Path:AttributeName
+ Add [MochaQ:GetRun Command]#REMOVECOLUMNATTRIBUTE:TableName:ColumnName:AttributeName

# rlsv3.4.3 [ 30 March 2020 ]

+ Rename [Namespace]Querying to Mochaq
+ In the path property in the connection string, the extension .mochadb is added automatically if it does not exist.
+ Add [Feature]Mhql
+ Add [Namespace]Mhql
+ Add [Class:Exception]MochaException
+ Add extension query functions as [Namespace]Querying
+ Add [Class]MochaDbCommand
+ Add [Class]MochaTableResult
+ Add [Interface]IMochaDatabaseItem
+ Add [Interface]IMochaDatabaseItem support for [Interface]IMochaTable
+ Add [Interface]IMochaDatabaseItem support for [Interface]IMochaSector
+ Add [Interface]IMochaDatabaseItem support for [Interface]IMochaStack
+ Replace [Class]MochaQCommand -> [Struct]MochaQCommand
+ Add [Interface]IMhqlCommand
+ Add [Struct]MhqlCommand
+ Add [MochaDatabase:Returnable Method]RemoveDatabaseItem
+ Add [MochaQFormatter:Returnable Method]UpperCaseKeywords
+ Add [MochaQFormatter:Returnable Method]LowerCaseKeywords
+ Add [Class]MhqlFormatter

# rlsv3.4.2 [ 18 March 2020 ]

+ Upgrade F# compatibility
+ Add IMochaElement interface
+ Add MochaElement
+ Improvements for ToString functions
+ Optimizations and improvements for MochaQuery
+ Fix null command bug in MochaQCommand
+ Improvements for streams
+ Add async functions in streams
+ Fix Write function's count parameter never used on streams
+ Add IEnumerable interface compatibility for collections
+ Add IEnumerable interface compatibility for collections
+ Add ICollection interface compatibility for collections
+ Improvements for castings
+ Add copy functions for MochaDatabase
+ Add ``>SOURCEDIR<subcount`` connection string functions
+ Add MochaReadonlyCollection base class
+ Fix column data listing on MochaTable
+ Fix column data sets on MochaTable
+ Fix data count disagreement on MochaTable and MochaColumn
+ Performance improvements on MochaDatabase functions
+ Add IMochaWriter interface
+ Add MochaWriter
+ Add MochaStreamWriter
+ Add IMochaArray interface
+ Add MochaArray
+ Add element functions for MochaDatabase
+ Add element functions for compatibility objects
+ Improvements for enumerators

# rlsv3.4.1 [ 15 March 2020 ]

+ Improvements for logs
+ Add remove command support for GetRun
+ Add GetRun support for FileSystem remove functions
+ Add MochaQ commands and improvements for logs
+ Minor optimizations

# rlsv3.4.0 [ 14 March 2020 ]

+ Bug fixes
+ New features
+ Improvements
+ Optimizations
+ Logs
+ Database Schemas

And more...

# rlsv3.3.0 [ 12 March 2020 ]

+ Important bug fixes
+ Optimizations
+ Improvements
+ New features

And more...

# rlsv3.2.0 [ 8 March 2020 ]

+ Update compatibility
+ Improvements
+ Optimizations
+ New features
+ Important bug fixes

# rlsv3.1.2 [ 5 March 2020 ]

+ Fix name protections of collections
+ Optimizations in MochaScriptDebugger

# rlsv3.1.1 [ 1 March 2020 ]

+ Bug fixes for MochaScript
+ Optimization for MochaScript
+ Added MochaScript functions

# rlsv3.1.0 [ 1 March 2020 ]

+ Bug fixes
+ Optimization
+ Minor additions

# rlsv3.0.0 [ 23 February 2020 ]

+ Important bug fixes
+ Performance improvements
+ Stability improvements
+ FileSystem
+ Improvements for LINQ support
+ Object intefaces
+ Queryable returners
+ Objects for query results
+ Providers and other connection innovations
+ Queryable collections
+ New MochaQ commands
+ New data types for database
+ New variable types for MochaScript

# rlsv2.0.0 [ 9 February 2020 ]

Optimization, performance improvements, bug fixes and new features.

# rlsv1.0.0 [ 27 January 2020 ]

First release.
