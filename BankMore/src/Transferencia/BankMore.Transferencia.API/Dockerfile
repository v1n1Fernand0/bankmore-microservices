FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Transferencia/BankMore.Transferencia.Domain/*.csproj         src/Transferencia/BankMore.Transferencia.Domain/
COPY src/Transferencia/BankMore.Transferencia.Application/*.csproj    src/Transferencia/BankMore.Transferencia.Application/
COPY src/Transferencia/BankMore.Transferencia.Infrastructure/*.csproj src/Transferencia/BankMore.Transferencia.Infrastructure/
COPY src/Transferencia/BankMore.Transferencia.API/*.csproj            src/Transferencia/BankMore.Transferencia.API/

RUN dotnet restore src/Transferencia/BankMore.Transferencia.API/BankMore.Transferencia.API.csproj

COPY . .

RUN dotnet publish src/Transferencia/BankMore.Transferencia.API/BankMore.Transferencia.API.csproj \
    -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production

VOLUME ["/data"]

COPY --from=build /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "BankMore.Transferencia.API.dll"]
