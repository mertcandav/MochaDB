:: Copyright 2020 The MochaDB Authors.
:: Use of this source code is governed by a MIT
:: license that can be found in the LICENSE file.

:: Compile fsharptest
@dotnet restore ./fsharptest/fsharptest.fsproj
@dotnet build ./fsharptest/fsharptest.fsproj

@pause
