# Mertcan DAVULCU
#!/bin/bash

# Compile fsharptest
dotnet restore ./fsharptest/fsharptest.fsproj
dotnet build ./fsharptest/fsharptest.fsproj

echo "Press enter for close.."
read
