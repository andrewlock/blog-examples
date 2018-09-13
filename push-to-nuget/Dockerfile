FROM microsoft/dotnet:2.1.300-rc1-sdk AS builder

ARG Version  
WORKDIR /sln

COPY . .

RUN dotnet restore
RUN dotnet build /p:Version=$Version -c Release --no-restore  
RUN dotnet pack /p:Version=$Version -c Release --no-restore --no-build -o /sln/artifacts 

ENTRYPOINT ["dotnet", "nuget", "push", "/sln/artifacts/*.nupkg"]
CMD ["--source", "https://api.nuget.org/v3/index.json"] 