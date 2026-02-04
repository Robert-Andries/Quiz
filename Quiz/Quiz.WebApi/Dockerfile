FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Quiz.WebApi/Quiz.WebApi.csproj", "Quiz.WebApi/"]
COPY ["Quiz.ApplicationLayer/Quiz.ApplicationLayer.csproj", "Quiz.ApplicationLayer/"]
COPY ["Quiz.DomainLayer/Quiz.DomainLayer.csproj", "Quiz.DomainLayer/"]
COPY ["Quiz.PersistenceLayer/Quiz.PersistenceLayer.csproj", "Quiz.PersistenceLayer/"]
COPY ["Quiz.Shared/Quiz.Shared.csproj", "Quiz.Shared/"]
RUN dotnet restore "Quiz.WebApi/Quiz.WebApi.csproj"
COPY . .
WORKDIR "/src/Quiz.WebApi"
RUN dotnet build "./Quiz.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quiz.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiz.WebApi.dll"]
