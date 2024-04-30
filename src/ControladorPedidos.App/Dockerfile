FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

ENV ASPNETCORE_URLS=http://+:5003

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/ControladorPedidos.App/ControladorPedidos.App.csproj", "src/ControladorPedidos.App/"]
RUN dotnet restore "src/ControladorPedidos.App/ControladorPedidos.App.csproj"
COPY . .
WORKDIR "/src/src/ControladorPedidos.App"
RUN dotnet build "ControladorPedidos.App.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ControladorPedidos.App.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControladorPedidos.App.dll"]
