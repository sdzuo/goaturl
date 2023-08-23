# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore the dependencies
COPY ../../urlgoatbackend/urlgoatbackend.csproj ./urlgoatbackend/
RUN dotnet restore ./urlgoatbackend/urlgoatbackend.csproj --disable-parallel
COPY ../../urlgoatbackend ./urlgoatbackend/
RUN dotnet publish ./urlgoatbackend/urlgoatbackend.csproj -c release -o /app --no-restore

# Final Image Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 7203

ENTRYPOINT ["dotnet", "urlgoatbackend.dll"]
