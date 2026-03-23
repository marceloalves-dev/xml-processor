# XML Processor

REST API for processing Brazilian fiscal documents (NF-e, NFC-e, NFS-e) from XML, built with .NET 10 and MongoDB.

## Architecture

Clean Architecture with separation of concerns across four layers:

```
├── Domain          → Entities, Value Objects, Repository interfaces
├── Application     → Use Cases, DTOs, Service interfaces
├── Infrastructure  → MongoDB persistence, XML parser
└── API             → REST Controllers
```

## Features

- Upload and parse fiscal document XML (NF-e, NFC-e, NFS-e)
- Retrieve document by access key (Chave de Acesso)
- List documents with filters (CNPJ, Razão Social, emission date) and pagination
- Delete document by access key
- Duplicate detection on save

## Tech Stack

- **.NET 10** — ASP.NET Core Web API
- **MongoDB** — document persistence
- **NUnit + FluentAssertions + NSubstitute** — unit testing

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/notafiscal` | Upload XML and save document |
| `GET` | `/api/notafiscal/{chave}` | Get document by access key |
| `GET` | `/api/notafiscal` | List documents with filters |
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

## Running Tests

```bash
dotnet test
```

## Domain Model

- **NotaFiscal** — abstract base entity (NF-e, NFC-e, NFS-e)
- **ChaveNota** — value object for the 44-digit access key with format validation
- **Cnpj** — value object with digit verification algorithm validation
