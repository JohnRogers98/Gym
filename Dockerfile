FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Копируем всё
COPY . .

# Восстанавливаем зависимости для всех проектов
RUN for proj in $(find . -name "*.csproj" -type f); do \
      echo "Restoring $proj"; \
      dotnet restore "$proj" --verbosity minimal || true; \
    done

# Собираем основной проект
RUN dotnet build "Gym.WebApi/Gym.WebApi.csproj" -c Release

# Публикуем
RUN dotnet publish "Gym.WebApi/Gym.WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Gym.WebApi.dll"]