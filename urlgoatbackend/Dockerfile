# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY . .
RUN dotnet restore urlgoatbackend.csproj --disable-parallel
RUN dotnet publish urlgoatbackend.csproj -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 7203

ENTRYPOINT ["dotnet", "urlgoatbackend.dll"]