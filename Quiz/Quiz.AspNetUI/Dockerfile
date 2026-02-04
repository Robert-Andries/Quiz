FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Quiz.AspNetUI/Quiz.AspNetUI.csproj", "Quiz.AspNetUI/"]
COPY ["Quiz.DomainLayer/Quiz.DomainLayer.csproj", "Quiz.DomainLayer/"]
COPY ["Quiz.PersistenceLayer/Quiz.PersistenceLayer.csproj", "Quiz.PersistenceLayer/"]
RUN dotnet restore "Quiz.AspNetUI/Quiz.AspNetUI.csproj"
COPY . .
WORKDIR "/src/Quiz.AspNetUI"
RUN dotnet build "./Quiz.AspNetUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quiz.AspNetUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiz.AspNetUI.dll"]
