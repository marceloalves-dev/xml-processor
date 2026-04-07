# XML Processor

REST API for processing Brazilian fiscal documents (NF-e, CT-e, NFS-e) from XML, built with .NET 10 and MongoDB.

## Architecture

Clean Architecture with separation of concerns across four layers:

```
├── TaxDocumentProcessor.Domain          → Entities, Value Objects, Repository interfaces
├── TaxDocumentProcessor.Application     → Use Cases, DTOs, Service interfaces, Event contracts
├── TaxDocumentProcessor.Infrastructure  → MongoDB persistence, XML parser, RabbitMQ messaging
└── TaxDocumentProcessor.Api             → REST Controllers, Authentication
```

## Features

- Upload and parse fiscal document XML (NF-e, CT-e, NFS-e)
- Retrieve document by access key (Chave de Acesso)
- List documents with filters (CNPJ/CPF, Razão Social, emission date) and pagination
- Delete document by access key
- Duplicate detection on save — returns `201 Created` for new documents, `200 OK` if already exists
- JWT Bearer authentication — all endpoints require a valid token
- Swagger UI available at `/swagger` in development
- Event publishing via RabbitMQ on each new document processed
- Async consumer persists a processing log to a separate MongoDB collection
- Retry with exponential backoff (3 attempts) and automatic dead-letter queue on failure

## Tech Stack

- **.NET 10** — ASP.NET Core Web API
- **MongoDB** — document persistence
- **RabbitMQ + MassTransit** — event publishing and async consumption with retry policy
- **JWT Bearer** — authentication via `Microsoft.AspNetCore.Authentication.JwtBearer`
- **Swashbuckle** — Swagger UI
- **NUnit + FluentAssertions + NSubstitute** — unit testing

## Endpoints

### Authentication

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/auth/token` | Get a JWT token — body: `{ "username": "...", "password": "..." }` |

### Fiscal Documents (requires `Authorization: Bearer <token>`)

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/notafiscal` | Upload XML (`multipart/form-data`) and save document (`201` new, `200` duplicate) |
| `GET` | `/api/notafiscal/{chave}` | Get document by access key |
| `GET` | `/api/notafiscal` | List documents with filters |
| `PUT` | `/api/notafiscal/{chave}` | Update `razaoSocial` and/or `totalValue` |
| `DELETE` | `/api/notafiscal/{chave}` | Delete document by access key |

### Query filters for `GET /api/notafiscal`

| Parameter | Type | Description |
|-----------|------|-------------|
| `cnpjEmit` | string | Issuer CNPJ |
| `razaoSocial` | string | Company name |
| `dtEmission` | date | Emission date |
| `page` | int | Page number |
| `pageSize` | int | Page size |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community) running locally or a connection string
- [RabbitMQ](https://www.rabbitmq.com/download.html) running locally (or via Docker)

```bash
docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

RabbitMQ Management UI available at `http://localhost:15672` (default credentials: `guest` / `guest`).

### Configuration

Set the MongoDB connection, JWT and RabbitMQ settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "MongoDB": {
    "DatabaseName": "TaxDocumentProcessor"
  },
  "Jwt": {
    "Secret": "your-secret-key-min-32-chars",
    "Issuer": "TaxDocumentProcessor",
    "Audience": "TaxDocumentProcessor",
    "ExpirationMinutes": 60
  },
  "Auth": {
    "Username": "admin",
    "Password": "your-password"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "VirtualHost": "/",
    "Username": "guest",
    "Password": "guest"
  }
}
```

> **Note:** Never commit real secrets to source control. Use environment variables or a secrets manager in production.

### Running

```bash
dotnet run --project TaxDocumentProcessor.Api
```

API available at `http://localhost:5112`.

Swagger UI available at `http://localhost:5112/swagger`.

## Running Tests

```bash
dotnet test
```

## Error Handling

All errors return a `ProblemDetails` response with no stack trace:

| Scenario | Status |
|----------|--------|
| Invalid access key, invalid CNPJ/CPF | `400 Bad Request` |
| Document not found | `404 Not Found` |
| Unexpected error | `500 Internal Server Error` |

## Domain Model

- **NotaFiscal** — abstract base entity (NF-e, CT-e, NFS-e)
- **ChaveNota** — value object for the access key — 44 digits (NF-e/CT-e) or 50 digits (NFS-e)
- **CnpjOrCpf** — value object that validates both CNPJ and CPF with check digit algorithm

## Messaging

Every new document saved successfully triggers a `DocumentProcessedEvent` published to RabbitMQ via MassTransit.

The `DocumentProcessedConsumer` picks up the event and writes a `ProcessingLog` entry to the `processing_logs` MongoDB collection. If the consumer fails, MassTransit retries with exponential backoff (up to 3 attempts: 1s → 5s → 15s). After exhausting retries, the message is moved to the `document-processed_error` dead-letter queue automatically.
