# Copyright 2020 The MochaDB Authors.
# Use of this source code is governed by a MIT
# license that can be found in the LICENSE file.

#!/bin/bash

# Compile fsharpmhql
dotnet restore ./fsharpmhql/fsharpmhql.fsproj
dotnet build ./fsharpmhql/fsharpmhql.fsproj

echo "Press enter for close..."
read
