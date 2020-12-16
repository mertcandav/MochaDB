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
#!/bin/bash

# Sure?
read -p 'Compile MochaDB(Y): ' MochaDB
read -p 'Compile fsharptest(Y): ' fsharptest
read -p 'Compile vbnetmhql(Y): ' vbnetmhql
read -p 'Compile fsharpmhql(Y): ' fsharpmhql
read -p 'Compile terminal(Y)': terminal

# MochaDB
if [[ $MochaDB != "" && $MochaDB == "Y" ]]
then
    Compile_MochaDB.sh
fi

# fsharptest
if [[ $fsharptest != "" && $fsharptest == "Y" ]]
then
    Compile_fsharptest.sh
fi

# vbnetmhql
if [[ $vbnetmhql != "" && $vbnetmhql == "Y" ]]
then
    Compile_vbnetmhql.sh
fi

# fsharpmhql
if [[ %fsharpmhql != "" && %fsharpmhql == "Y" ]]
then
     Compile_fsharpmhql.sh
fi

# terminal
if [[ %terminal != "" && %terminal == "Y" ]]
then
    Compile_terminal.sh
fi

exit
