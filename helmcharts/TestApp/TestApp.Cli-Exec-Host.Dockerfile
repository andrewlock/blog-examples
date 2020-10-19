# Build standard .NET Core application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /app

# WARNING: This is completely unoptimised!
COPY . .

# Publish the CLI project to the path /app/output/cli
RUN dotnet publish ./src/TestApp.Cli -c Release -o /app/output/cli

###################

# Runtime image 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

# Copy the background script that keeps the pod alive
WORKDIR /background
COPY ./keep_alive.sh ./keep_alive.sh
# Ensure the file is executable
RUN chmod +x /background/keep_alive.sh

# Set the command that runs when the pod is started
CMD "/background/keep_alive.sh"

WORKDIR /app

# Copy the CLI tool into this container
COPY --from=builder ./app/output/cli .