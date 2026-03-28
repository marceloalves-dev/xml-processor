# XML Processor

REST API for processing Brazilian fiscal documents (NF-e, CT-e, NFS-e) from XML, built with .NET 10 and MongoDB.

## Architecture

Clean Architecture with separation of concerns across four layers:

```
├── Domain          → Entities, Value Objects, Repository interfaces
├── Application     → Use Cases, DTOs, Service interfaces
├── Infrastructure  → MongoDB persistence, XML parser
└── API             → REST Controllers
```

## Features

- Upload and parse fiscal document XML (NF-e, CT-e, NFS-e)
- Retrieve document by access key (Chave de Acesso)
- List documents with filters (CNPJ/CPF, Razão Social, emission date) and pagination
- Delete document by access key
- Duplicate detection on save — returns `201 Created` for new documents, `200 OK` if already exists
- Swagger UI available at `/swagger` in development

## Tech Stack

- **.NET 10** — ASP.NET Core Web API
- **MongoDB** — document persistence
- **Swashbuckle** — Swagger UI
- **NUnit + FluentAssertions + NSubstitute** — unit testing

## Endpoints

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

### Configuration

Set the MongoDB connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "MongoDB": {
    "DatabaseName": "xml-processor"
  }
}
```

### Running

```bash
dotnet run --project "Tax Document Processor"
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
