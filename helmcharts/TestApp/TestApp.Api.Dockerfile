FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.12 AS build
WORKDIR /sln

# Copy project file and restore
COPY "./src/TestApp.Api/TestApp.Api.csproj" "./src/TestApp.Api/"
RUN dotnet restore ./src/TestApp.Api/TestApp.Api.csproj

# Copy the actual source code
COPY "./src/TestApp.Api" "./src/TestApp.Api"

# Build and publish the app
RUN dotnet publish "./src/TestApp.Api/TestApp.Api.csproj" -c Release -o /app/publish

#FINAL image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.12
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TestApp.Api.dll"]