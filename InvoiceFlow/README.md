# InvoiceFlow

Cloud-native accounts payable workflow demo aligned to a modern C#/.NET/Azure backend stack.

## Stack
- C# / .NET 8 / ASP.NET Core Web API
- Azure Functions + Durable Functions orchestration
- SQL Server via EF Core
- Cosmos DB audit/event store integration point
- Azure Blob Storage integration point
- React dashboard
- REST APIs
- xUnit automated tests
- GitHub Actions CI
- Docker
- AI invoice extraction service abstraction (Azure OpenAI / Document Intelligence integration point)

## Workflow
1. Upload invoice metadata/document.
2. Persist invoice record in SQL Server.
3. Store document through Blob Storage service abstraction.
4. Start Durable Functions orchestration.
5. Extract invoice fields through AI service abstraction.
6. Validate and update invoice status.
7. Write workflow events to an audit store (Cosmos DB integration point).
8. Expose status through REST API and React dashboard.

## Run API locally
```bash
dotnet restore
dotnet run --project src/InvoiceFlow.Api
```
Swagger is available in development.

## Run tests
```bash
dotnet test
```

## Run frontend
```bash
cd frontend
npm install
npm run dev
```

## Important
This repository is a portfolio implementation. Cloud service classes are structured as integration points so Azure credentials are never committed. Replace local/mock implementations with Azure SDK implementations when deploying.
