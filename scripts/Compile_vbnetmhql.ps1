# Copyright 2020 The MochaDB Authors.
# Use of this source code is governed by a MIT
# license that can be found in the LICENSE file.

#!bin/bash

# Compile vbnetmhql
dotnet restore ./vbnetmhql/vbnetmhql.vbproj
dotnet build ./vbnetmhql/vbnetmhql.vbproj

pause
