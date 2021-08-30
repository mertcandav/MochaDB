# Copyright 2020 The MochaDB Authors.
# Use of this source code is governed by a MIT
# license that can be found in the LICENSE file.

#!bin/bash

# Sure?
$MochaDB = Read-Host 'Compile MochaDB(Y):'
$fsharptest = Read-Host 'Compile fsharptest(Y):'
$vbnetmhql = Read-Host 'Compile vbnetmhql(Y):'
$fsharpmhql = Read-Host 'Compile fsharpmhql(Y):'
$terminal = Read-Host 'Compile terminal(Y):'

# MochaDB
if ($MochaDB -eq 'Y') {
    powershell -ExecutionPolicy Bypass -File "Compile_MochaDB.ps1"
}

# fsharptest
if ($fsharptest -eq 'Y') {
    powershell -ExecutionPolicy Bypass -File "Compile_fsharptest.ps1"
}

# vbnetmhql
if ($vbnetmhql -eq 'Y') {
    powershell -ExecutionPolicy Bypass -File "Compile_vbnetmhql.ps1"
}

# fsharpmhql
if ($fsharpmhql -eq 'Y') {
    powershell -ExecutionPolicy Bypass -File "Compile_fsharpmhql.ps1"
}

exit
