# #Get base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

#Copy csproj and restore
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["urlgoatbackend.csproj", "./"]
RUN dotnet restore "./urlgoatbackend.csproj"

# #Copy other files and build
COPY . .
RUN dotnet build "urlgoatbackend.csproj" -c Release -o /app

#Publish
FROM build AS publish
RUN dotnet publish "urlgoatbackend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "urlgoatbackend.dll"]