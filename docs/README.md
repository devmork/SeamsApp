# SEAMS API Documentation

**SEAMS** (Student Event Attendance Management System) is an ASP.NET Core 8 Web API for managing student applications, officers, events, attendance sessions, and QR-based attendance logging.

This documentation is the onboarding entry point for developers working on the backend.

## Docs index

| Document | Purpose |
| -------- | ------- |
| [GETTING-STARTED.md](GETTING-STARTED.md) | Prerequisites, configuration, run, Swagger, database |
| [ARCHITECTURE.md](ARCHITECTURE.md) | Folder layout, layers, DbContext, DI, CORS, output cache |
| [AUTHENTICATION.md](AUTHENTICATION.md) | JWT login flow, roles, claims helpers, authorization attributes |
| [FEATURES.md](FEATURES.md) | Domain modules, service pattern, main endpoints |
| [CONVENTIONS.md](CONVENTIONS.md) | Naming, soft-delete/status flags, mapping, seeding |

## Quick orientation

- **Stack**: .NET 8, EF Core 9 (SQL Server), JWT Bearer, AutoMapper, Scrutor, Swashbuckle, QRCoder / SkiaSharp.QrCode
- **Entry point**: `Program.cs`
- **Data access**: `SeamsDbContext` + service classes under `Services/Commands/`
- **HTTP surface**: Controllers under `Controllers/` with route prefix `api/...`
- **Frontend origins allowed**: `https://seams-web.vercel.app`, `http://localhost:5173`

## Roles

| Role | Typical capabilities |
| ---- | -------------------- |
| `Admin` | Full CRUD on events, students, officers, applications; delete attendance records |
| `Officer` | View students/events, create attendance records, manage attendance sessions |
| `Student` | Own profile, own attendance history |

An initial Admin is seeded at startup (see GETTING-STARTED.md).

## Where to start

1. Read [GETTING-STARTED.md](GETTING-STARTED.md) and run the API locally.
2. Open Swagger at the running base URL (default `http://localhost:5035/swagger`).
3. Skim [ARCHITECTURE.md](ARCHITECTURE.md) and [AUTHENTICATION.md](AUTHENTICATION.md).
4. Use [FEATURES.md](FEATURES.md) as the inventory of what the API actually exposes.
