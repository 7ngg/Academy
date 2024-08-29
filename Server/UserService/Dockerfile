FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ./UserService/UserService.csproj ./UserService/
COPY ./DataLayer/DataLayer.csproj ./DataLayer/
COPY ./JwtPreset/JwtPreset.csproj ./JwtPreset/

RUN dotnet restore ./UserService/UserService.csproj

COPY ./DataLayer/ ./DataLayer/
COPY ./JwtPreset/ ./JwtPreset/
COPY ./UserService/ ./UserService/

WORKDIR /app/UserService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/UserService/out .
ENTRYPOINT ["dotnet", "UserService.dll"]
