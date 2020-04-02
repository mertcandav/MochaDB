# Mertcan DAVULCU
#!bin/bash

# Compile solution
dotnet restore MochaDB.sln
dotnet build MochaDB.sln

echo "Press enter for close..."
read
