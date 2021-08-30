# MIT License
# 
# Copyright (c) 2020 Mertcan Davulcu
# 
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
# 
# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.
# 
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# 
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.

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

# terminal
if ($terminal -eq 'Y') {
    powershell -ExecutionPolicy Bypass -File "Compile_terminal.ps1"
}

exit
