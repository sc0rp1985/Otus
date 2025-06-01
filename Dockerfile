# ===== СТАДИЯ 1: Сборка =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем sln и проекты
COPY Otus.SocNet.WebApi.sln .
COPY Otus.SocNet.WebApi/Otus.SocNet.WebApi.csproj ./Otus.SocNet.WebApi/
COPY Otus.SocNet.DAL/Otus.SocNet.DAL.csproj ./Otus.SocNet.DAL/

# Восстанавливаем зависимости
RUN dotnet restore Otus.SocNet.WebApi.sln

# Копируем весь код
COPY Otus.SocNet.WebApi/ ./Otus.SocNet.WebApi/
COPY Otus.SocNet.DAL/ ./Otus.SocNet.DAL/

# Публикуем
WORKDIR /src/Otus.SocNet.WebApi
RUN dotnet publish -c Release -o /app/publish

# ===== СТАДИЯ 2: Финальный образ =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# Установка curl (Debian-based образы используют apt-get)
RUN apt-get update && \
    apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Otus.SocNet.WebApi.dll"]