SLN_DIR="$(pwd)"
dotnet clean
dotnet publish -c Release -o "output" BlazorApp1
RenderOutputDirectory="${SLN_DIR}/output/wwwroot" \
dotnet test -c Release --filter Category=PreRender 