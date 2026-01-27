# Этап 1: Сборка бэкенда
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем только необходимые файлы
COPY *.sln .
COPY Gym.WebApi/*.csproj ./Gym.WebApi/
COPY Gym.Application/*.csproj ./Gym.Application/
COPY Gym.Domain/*.csproj ./Gym.Domain/
COPY Gym.Infrastructure/*.csproj ./Gym.Infrastructure/
COPY Gym.CompositionRoot/*.csproj ./Gym.CompositionRoot/
COPY Gym.WebDto/*.csproj ./Gym.WebDto/

# Восстанавливаем зависимости
RUN dotnet restore "Gym.WebApi/Gym.WebApi.csproj"

# Копируем весь код
COPY . .

# Собираем и публикуем
RUN dotnet publish "Gym.WebApi/Gym.WebApi.csproj" -c Release -o /app/publish

# Этап 2: Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Копируем опубликованное приложение
COPY --from=build /app/publish .

# Устанавливаем порт
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Gym.WebApi.dll"]