FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.12 AS build
WORKDIR /sln

# Copy project file and restore
COPY "./src/TestApp.Cli/TestApp.Cli.csproj" "./src/TestApp.Cli/"
RUN dotnet restore ./src/TestApp.Cli/TestApp.Cli.csproj

# Copy the actual source code
COPY "./src/TestApp.Cli" "./src/TestApp.Cli"

# Build and publish the app
RUN dotnet publish "./src/TestApp.Cli/TestApp.Cli.csproj" -c Release -o /app/publish

#FINAL image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.12
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TestApp.Cli.dll"]