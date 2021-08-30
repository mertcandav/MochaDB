# Copyright 2020 The MochaDB Authors.
# Use of this source code is governed by a MIT
# license that can be found in the LICENSE file.

#!bin/bash

# Compile MochaDB
dotnet restore ./src/MochaDB.csproj
dotnet build ./src/MochaDB.csproj

echo "Press enter for close..."
read
