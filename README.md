# Chat

ASP.NET Core 9 real-time chat API built with Clean Architecture across 4 projects, Azure SignalR Service, and JWT authentication backed by Azure Key Vault.

## Stack

| Layer | Technology |
|-------|------------|
| Runtime | ASP.NET Core 9, .NET 9 Alpine |
| Real-time | Azure SignalR Service |
| Auth | JWT Bearer, Microsoft Identity, Azure Key Vault |
| ORM | EF Core |
| Docs | Scalar (OpenAPI) |
| Container | Docker (multi-stage, Alpine) |

## Features

- **Real-time messaging** via Azure SignalR Service - users auto-join their chat groups on connection
- **JWT authentication** with refresh token support and claim-based user identification
- **Azure Key Vault** for secrets management - no sensitive config in source code
- **Clean Architecture** - `Application`, `Domain`, `Infrastructure`, `ChatApi` projects with clear dependency boundaries
- **Generic Orchestrator pattern** for coordinating cross-service operations in the hub layer
- **Scalar UI** for interactive API exploration

## Architecture

```
Chat/
├── ChatApi/          # Presentation: controllers, SignalR hub, middleware
├── Application/      # Use cases: services, DTOs, orchestrations, interfaces
├── Domain/           # Entities, repository interfaces
└── Infrastructure/   # EF Core, repository implementations
```

## Running locally

### Prerequisites

- .NET 9 SDK
- Docker
- Azure subscription (Key Vault + SignalR Service) **or** local overrides via `.env`

### With Docker

```bash
cp .env.example .env   # fill in your values
docker build -t chat-api -f Chat/Dockerfile .
docker run -p 8080:${ASPNETCORE_HTTP_PORT} --env-file .env chat-api
```

### Without Docker

```bash
cd Chat
dotnet restore
dotnet run
```

API docs available at `http://localhost:{port}/scalar`.

## Configuration

The app reads secrets from **Azure Key Vault** at startup. Set the vault URI via environment variable or `.env`:

```
SchoolChatSecretsUri=https://your-vault.vault.azure.net/
```

Required secrets in the vault:

| Key | Description |
|-----|-------------|
| `Jwt--SecretKey` | JWT signing key |
| `ConnectionStrings--DefaultConnection` | Database connection string |
| `Azure--SignalR--ConnectionString` | Azure SignalR connection string |
