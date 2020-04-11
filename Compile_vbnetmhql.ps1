# Mertcan DAVULCU
#!bin/bash

# Compile vbnetmhql
dotnet restore ./vbnetmhql/vbnetmhql.vbproj
dotnet build ./vbnetmhql/vbnetmhql.vbproj

pause
