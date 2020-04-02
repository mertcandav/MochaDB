# Mertcan DAVULCU
#!/bin/bash

# Compile fsharpmhql
dotnet restore ./fsharpmhql/fsharpmhql.fsproj
dotnet build ./fsharpmhql/fsharpmhql.fsproj

echo "Press enter for close..."
read
