# Mertcan DAVULCU
#!bin/bash

# Compile MochaDB
dotnet restore ./src/MochaDB.csproj
dotnet build ./src/MochaDB.csproj

echo "Press enter for close..."
read
