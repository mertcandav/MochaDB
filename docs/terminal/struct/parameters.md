Parameters make it easy to use the command in different ways. <br>
It is written with a hyphen per parameter and cannot contain a dash and a space in it. <br>
Example parameter definitions: <br>

```
module -param1 -param2 -param3 -param4     // TRUE
module -param1-param2 -param3 - param4     // FALSE
module param1 param2 param3                // FALSE
```