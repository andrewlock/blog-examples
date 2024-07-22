$ErrorActionPreference = "Stop"
dotnet build -c Release ./SourceOnlyExample/SourceOnlyExample.csproj
dotnet pack -c Release ./SourceOnlyExample/SourceOnlyExample.csproj
rm -r -Force ./packages/* 
dotnet remove ./TestApp/TestApp.csproj package  SourceOnlyExample
dotnet add ./TestApp/TestApp.csproj package SourceOnlyExample --package-directory ./packages --source ./SourceOnlyExample/bin/Release/
dotnet build ./TestApp/TestApp.csproj --packages ./packages
dotnet run --no-build --project ./TestApp/TestApp.csproj