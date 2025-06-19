# Используем официальный образ SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.sln .
COPY YourProjectName/*.csproj ./YourProjectName/
RUN dotnet restore

# Копируем остальные файлы и собираем проект
COPY YourProjectName/. ./YourProjectName/
WORKDIR /app/YourProjectName
RUN dotnet publish -c Release -o out

# Используем официальный образ ASP.NET Core Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/YourProjectName/out ./

# Открываем порт 80
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "YourProjectName.dll"]
