
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BankMore.ContaCorrente.API/*.csproj ./BankMore.ContaCorrente.API/
COPY BankMore.ContaCorrente.Domain/*.csproj ./BankMore.ContaCorrente.Domain/
COPY BankMore.ContaCorrente.Application/*.csproj ./BankMore.ContaCorrente.Application/
COPY BankMore.ContaCorrente.Infrastructure/*.csproj ./BankMore.ContaCorrente.Infrastructure/

RUN dotnet restore ./BankMore.ContaCorrente.API/BankMore.ContaCorrente.API.csproj

COPY . .

WORKDIR /src/BankMore.ContaCorrente.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "BankMore.ContaCorrente.API.dll"]
