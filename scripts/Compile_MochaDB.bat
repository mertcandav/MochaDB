:: Copyright 2020 The MochaDB Authors.
:: Use of this source code is governed by a MIT
:: license that can be found in the LICENSE file.

:: Compile MochaDB
@dotnet restore ./src/MochaDB.csproj
@dotnet build ./src/MochaDB.csproj

@pause
