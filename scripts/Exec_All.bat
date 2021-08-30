:: Copyright 2020 The MochaDB Authors.
:: Use of this source code is governed by a MIT
:: license that can be found in the LICENSE file.

:: Sure?
@set /P MochaDB="Compile MochaDB(Y): "
@set /P fsharptest="Compile fsharptest(Y): "
@set /P vbnetmhql="Compile vbnetmhql(Y): "
@set /P fsharpmhql="Compile fsharpmhql(Y): "
@set /P terminal="Compile terminal(Y): "

:: MochaDB
if %MochaDB% == Y (
    @start Compile_MochaDB.bat
)

:: fsharptest
if %fsharptest% == Y (
    @start Compile_fsharptest.bat
)

:: vbnetmhql
if %vbnetmhql% == Y (
    @start Compile_vbnetmhql.bat
)

:: fsharpmhql
if %fsharpmhql% == Y (
    @start Compile_fsharpmhql.bat
)

exit
