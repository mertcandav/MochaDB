# Copyright 2020 The MochaDB Authors.
# Use of this source code is governed by a MIT
# license that can be found in the LICENSE file.

#!bin/bash

# Compile solution
dotnet restore MochaDB.sln
dotnet build MochaDB.sln

echo "Press enter for close..."
read
