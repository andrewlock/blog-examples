
# The builder image
FROM mcr.microsoft.com/dotnet/sdk:5.0.100-alpine3.12 AS builder

WORKDIR /sln

# Just copy everything
COPY . .

# Do the restore/publish/build in one step
RUN dotnet publish -c Release -o /sln/artifacts -r linux-x64 -p:PublishTrimmed=True

# The deployment image
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.0-alpine3.12

# Copy across the published app
WORKDIR /app
ENTRYPOINT ["dotnet", "IdentityServerTestApp.dll"]
COPY --from=builder ./sln/artifacts .