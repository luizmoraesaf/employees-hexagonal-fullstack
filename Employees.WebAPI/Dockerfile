FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
# Expõe as portas que a API usará
EXPOSE 12270
EXPOSE 12271

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Employees.WebAPI/Employees.WebAPI.csproj", "Employees.WebAPI/"]
RUN dotnet restore "Employees.WebAPI/Employees.WebAPI.csproj"
COPY . .
WORKDIR "/src/Employees.WebAPI"
RUN dotnet build "Employees.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Employees.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employees.WebAPI.dll"]
