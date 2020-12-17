Tags allow you to extend the functionality of the command and can be used for various purposes. <br>
When using Tag, parameters or any other content cannot be used! Each tag is written with ``@`` per tag. <br>
Example tag definitions: <br>

```
module @tag1            // TRUE
module tag1             // FALSE
module @tag -param1     // FALSE
```
