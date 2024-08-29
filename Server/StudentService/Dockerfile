FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ./StudentService/StudentService.csproj ./StudentService/
COPY ./DataLayer/DataLayer.csproj ./DataLayer/
COPY ./JwtPreset/JwtPreset.csproj ./JwtPreset/

RUN dotnet restore ./StudentService/StudentService.csproj

COPY ./DataLayer/ ./DataLayer/
COPY ./JwtPreset/ ./JwtPreset/
COPY ./StudentService/ ./StudentService/

WORKDIR /app/StudentService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/StudentService/out .
ENTRYPOINT ["dotnet", "StudentService.dll"]

