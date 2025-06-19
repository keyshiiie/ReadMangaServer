# Используем официальный образ SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем решение и проект
COPY *.sln ./
COPY ReadMangaWS/*.csproj ./ReadMangaWS/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем остальные файлы проекта
COPY ReadMangaWS/. ./ReadMangaWS/
WORKDIR /app/ReadMangaWS

# Публикуем проект в папку out
RUN dotnet publish -c Release -o out

# Используем официальный образ ASP.NET Core Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Копируем опубликованные файлы из стадии сборки
COPY --from=build /app/ReadMangaWS/out ./

# Открываем порт 80 (стандартный HTTP порт)
EXPOSE 80

# Устанавливаем переменную окружения ASPNETCORE_URLS, чтобы приложение слушало на порту из переменной PORT или 80 по умолчанию
ENV ASPNETCORE_URLS=http://*:${PORT:-80}

# Запускаем приложение
ENTRYPOINT ["dotnet", "ReadMangaWS.dll"]
