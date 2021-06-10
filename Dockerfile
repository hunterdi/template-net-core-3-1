#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./RestApi/RestApi.csproj", "RestApi/"]
COPY ["./BusinessMappings/BusinessMappings.csproj", "BusinessMappings/"]
COPY ["./Business/Business.csproj", "Business/"]
COPY ["./Architecture/Architecture.csproj", "Architecture/"]
COPY ["./Services/Services.csproj", "Services/"]
COPY ["./Repositories/Repositories.csproj", "Repositories/"]
COPY ["./Seed/Seed.csproj", "Seed/"]
COPY ["./Tests/Tests.csproj", "Tests/"]
RUN dotnet restore "./RestApi/RestApi.csproj"
COPY . .
WORKDIR "/src/RestApi"
RUN dotnet build "RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestApi.dll"]