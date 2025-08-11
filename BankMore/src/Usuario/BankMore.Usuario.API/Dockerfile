# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BankMore.Usuario.API/*.csproj ./BankMore.Usuario.API/
COPY BankMore.Usuario.Domain/*.csproj ./BankMore.Usuario.Domain/
COPY BankMore.Usuario.Infrastructure/*.csproj ./BankMore.Usuario.Infrastructure/
COPY BankMore.Usuario.Application/*.csproj ./BankMore.Usuario.Application/

RUN dotnet restore ./BankMore.Usuario.API/BankMore.Usuario.API.csproj

COPY . .
WORKDIR /src/BankMore.Usuario.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "BankMore.Usuario.API.dll"]
