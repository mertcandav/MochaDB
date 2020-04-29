:: MIT License
:: 
:: Copyright (c) 2020 Mertcan Davulcu
:: 
:: Permission is hereby granted, free of charge, to any person obtaining a copy
:: of this software and associated documentation files (the "Software"), to deal
:: in the Software without restriction, including without limitation the rights
:: to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
:: copies of the Software, and to permit persons to whom the Software is
:: furnished to do so, subject to the following conditions:
::  
:: The above copyright notice and this permission notice shall be included in all
:: copies or substantial portions of the Software.
:: 
:: THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
:: IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
:: 
:: FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
:: AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
:: LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
:: OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
:: SOFTWARE.

:: Sure?
@set /P MochaDB="Compile MochaDB(Y): "
@set /P fsharptest="Compile fsharptest(Y): "
@set /P vbnetmhql="Compile vbnetmhql(Y): "
@set /P fsharpmhql="Compile fsharpmhql(Y): "

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
