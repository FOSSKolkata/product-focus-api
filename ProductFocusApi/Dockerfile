#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ProductFocusApi/ProductFocusApi.csproj", "ProductFocusApi/"]
COPY ["ProductFocus.Domain/ProductFocus.Domain.csproj", "ProductFocus.Domain/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["ProductFocus.Persistence/ProductFocus.Persistence.csproj", "ProductFocus.Persistence/"]
RUN dotnet restore "ProductFocusApi/ProductFocusApi.csproj"
COPY . .
WORKDIR "/src/ProductFocusApi"
RUN dotnet build "ProductFocusApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductFocusApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductFocusApi.dll", "--server.urls", "http://+:80;https://+:443"]