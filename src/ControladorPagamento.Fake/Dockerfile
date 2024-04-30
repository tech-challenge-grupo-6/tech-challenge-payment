FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5083

ENV ASPNETCORE_URLS=http://+:5083

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/ControladorPagamento.Fake/ControladorPagamento.Fake.csproj", "src/ControladorPagamento.Fake/"]
RUN dotnet restore "src/ControladorPagamento.Fake/ControladorPagamento.Fake.csproj"
COPY . .
WORKDIR "/src/src/ControladorPagamento.Fake"
RUN dotnet build "ControladorPagamento.Fake.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ControladorPagamento.Fake.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControladorPagamento.Fake.dll"]
