FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.12 AS build
WORKDIR /sln

# Copy project file and restore
COPY "./src/TestApp.Service/TestApp.Service.csproj" "./src/TestApp.Service/"
RUN dotnet restore ./src/TestApp.Service/TestApp.Service.csproj

# Copy the actual source code
COPY "./src/TestApp.Service" "./src/TestApp.Service"

# Build and publish the app
RUN dotnet publish "./src/TestApp.Service/TestApp.Service.csproj" -c Release -o /app/publish

#FINAL image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.12
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TestApp.Service.dll"]