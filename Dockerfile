FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2019 AS build
WORKDIR /app

COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-windowsservercore-ltsc2019
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "ReadMangaWS.dll"]
