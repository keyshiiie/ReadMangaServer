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

# Открываем порт 5000 (или порт, который вы используете в коде)
EXPOSE 5000

# Не задаём ASPNETCORE_URLS здесь, т.к. в коде уже настроен Kestrel на PORT

# Запускаем приложение
ENTRYPOINT ["dotnet", "ReadMangaWS.dll"]
