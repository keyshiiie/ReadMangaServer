# Используем официальный образ SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем решение и проект (файлы лежат в текущей папке)
COPY *.sln ./
COPY *.csproj ./

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем остальные файлы проекта
COPY . ./

WORKDIR /app

# Публикуем проект в папку out
RUN dotnet publish -c Release -o out

# Используем официальный образ ASP.NET Core Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Копируем опубликованные файлы из стадии сборки
COPY --from=build /app/out ./

# Открываем порт 5000 (или порт, который вы используете в коде)
EXPOSE 5000

# Запускаем приложение
ENTRYPOINT ["dotnet", "ReadMangaWS.dll"]
