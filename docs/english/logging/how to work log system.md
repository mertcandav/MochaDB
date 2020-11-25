It is a system that is activated when the log system is set to true on the provider. An encrypted, one-to-one copy of the database is copied into the database with a unique ID without any changes made before each change.<br>
The ID algorithm is Hash16, which is included in the ID algorithms offered by MochaDB.

## Warnings
<b>Since an exact copy of the database is kept, if the database is large, there may be a lot of size increase and when logging is activated, slowness may occur while receiving the log.<b>