# Copyright 2020 The MochaDB Authors.
# Use of this source code is governed by a MIT
# license that can be found in the LICENSE file.

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

exit
