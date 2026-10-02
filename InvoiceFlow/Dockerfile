FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/InvoiceFlow.Api ./src/InvoiceFlow.Api
RUN dotnet publish src/InvoiceFlow.Api/InvoiceFlow.Api.csproj -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet","InvoiceFlow.Api.dll"]
