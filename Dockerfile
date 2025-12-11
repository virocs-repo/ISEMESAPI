#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ISEMES.API/ISEMES.API.csproj", "src/ISEMES.API/"]
COPY ["src/ISEMES.Models/ISEMES.Models.csproj", "src/ISEMES.Models/"]
COPY ["src/ISEMES.Repositories/ISEMES.Repositories.csproj", "src/ISEMES.Repositories/"]
COPY ["src/ISEMES.Services/ISEMES.Services.csproj", "src/ISEMES.Services/"]
RUN dotnet restore "./src/ISEMES.API/ISEMES.API.csproj"
COPY . .
WORKDIR "/src/src/ISEMES.API"
RUN dotnet build "./ISEMES.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ISEMES.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ISEMES.API.dll"]
